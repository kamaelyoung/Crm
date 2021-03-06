﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Api;
using Coldew.Website.Models;
using Newtonsoft.Json;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Data;
using Newtonsoft.Json.Linq;
using Coldew.Api.Organization;
using System.Text.RegularExpressions;
using Crm.Website.Models;
using Coldew.Api.UI;

namespace Coldew.Website.Controllers
{
    public class MetadataController : BaseController
    {

        public ActionResult Index(string objectId)
        {
            ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(objectId);
            this.ViewBag.coldewObject = coldewObject;

            List<GridViewInfo> views = WebHelper.GridViewService.GetGridViews(objectId, WebHelper.CurrentUserAccount);
            GridViewInfo viewInfo = views.Find(x => x.Type == GridViewType.Manage);

            List<DataGridColumnModel> columns = viewInfo.Columns.Select(x => new DataGridColumnModel(x)).ToList();
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);
            this.ViewBag.viewId = viewInfo.ID;
            this.ViewBag.Title = viewInfo.Name;
            this.ViewBag.canSettingView = viewInfo.Creator.Account == this.CurrentUser.Account;
            return View();
        }

        public ActionResult SelectDialog()
        {
            return this.PartialView();
        }

        public ActionResult Favorite(string objectId, string viewId)
        {
            ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(objectId);
            this.ViewBag.coldewObject = coldewObject;

            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(viewId);

            List<DataGridColumnModel> columns = viewInfo.Columns.Select(x => new DataGridColumnModel(x)).ToList();
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);
            this.ViewBag.viewId = viewInfo.ID;
            this.ViewBag.Title = viewInfo.Name;
            this.ViewBag.canSettingView = viewInfo.Creator.Account == this.CurrentUser.Account;
            return View();
        }

        public ActionResult Favorites(string objectId, int start, int size, string orderBy)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<MetadataInfo> favorites = WebHelper.MetadataService.GetFavorites(objectId, WebHelper.CurrentUserAccount, orderBy);
                List<MetadataGridJObjectModel> models = favorites.Skip(start).Take(size).Select(x => new MetadataGridJObjectModel(objectId, x, this)).ToList();
                resultModel.data = new DatagridModel { count = favorites.Count, list = models };
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Favorites(string objectId, string metadataIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<string> metadataIds = JsonConvert.DeserializeObject<List<string>>(metadataIdsJson);
                WebHelper.MetadataService.Favorite(objectId, WebHelper.CurrentUserAccount, metadataIds);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CancelFavorite(string objectId, string metadataIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<string> metadataIds = JsonConvert.DeserializeObject<List<string>>(metadataIdsJson);
                WebHelper.MetadataService.CancelFavorite(objectId, WebHelper.CurrentUserAccount, metadataIds);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImportFirst(string objectId)
        {
            ColdewObjectInfo form = WebHelper.ColdewObjectService.GetFormById(objectId);
            this.ViewBag.Title = "导入" + form.Name;
            return View();
        }

        public ActionResult DownloadImportTemplate(string objectId)
        {
            string tempPath = ImportExportHelper.GetImportTemplate(objectId, this);
            return File(tempPath, "application/octet-stream", "Import Template.xls");
        }

        [HttpPost]
        public ActionResult UploadImportFile(string objectId)
        {
            string jsonFilePath = ImportExportHelper.GetUploadImportFileJsonFile(objectId, this);
            return Redirect(this.Url.Action("ImportSecond", new { tempFileName = Path.GetFileName(jsonFilePath), objectId = objectId }));
        }

        public ActionResult ImportSecond(string objectId)
        {
            ColdewObjectInfo form = WebHelper.ColdewObjectService.GetFormById(objectId);
            this.ViewBag.Title = "导入" + form.Name;
            List<DataGridColumnModel> columns = ImportExportHelper.GetImportColumns(objectId);
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);

            return View();
        }

        [HttpPost]
        public ActionResult Import(string tempFileName, string objectId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                string tempFilePath = Path.Combine(Server.MapPath("~/Temp"), tempFileName);
                List<JObject> importModels = JsonConvert.DeserializeObject<List<JObject>>(System.IO.File.ReadAllText(tempFilePath));
                ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(objectId);
                foreach (JObject model in importModels)
                {
                    PropertySettingDictionary propertys = new PropertySettingDictionary();
                    List<FieldInfo> fieldInfos = coldewObject.Fields.Where(x => x.CanInput).ToList();
                    foreach (FieldInfo filed in fieldInfos)
                    {
                        propertys.Add(filed.Code, model[filed.Code].ToString());
                    }
                    try
                    {
                        WebHelper.MetadataService.Create(objectId, WebHelper.CurrentUserAccount, propertys);

                        model["importResult"] = true;
                        model["importMessage"] = "导入成功";
                    }
                    catch(Exception ex)
                    {
                        WebHelper.Logger.Error(ex.Message, ex);
                        model["importResult"] = false;
                        model["importMessage"] = "导入失败" + ex.Message;
                    }
                }

                StreamWriter jsonStreamWriter = new StreamWriter(tempFilePath);
                jsonStreamWriter.Write(JsonConvert.SerializeObject(importModels));
                jsonStreamWriter.Close();
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetImportMetadatas(string tempFileName, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                string tempFilePath = Path.Combine(Server.MapPath("~/Temp"), tempFileName);
                List<JObject> importModels = JsonConvert.DeserializeObject<List<JObject>>(System.IO.File.ReadAllText(tempFilePath))
                    .OrderBy(x => x["importResult"]).ToList();

                resultModel.data = new DatagridModel { count = importModels.Count, list = importModels.Skip(start).Take(size) };
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create(string objectId)
        {
            FormInfo formInfo = WebHelper.FormService.GetForm(objectId, FormConstCode.CreateFormCode);
            this.ViewBag.formInfo = formInfo;
            return View();
        }

        [HttpPost]
        public ActionResult Create(string objectId, string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                JObject model = JsonConvert.DeserializeObject<JObject>(json);
                PropertySettingDictionary propertys = ExtendHelper.MapPropertySettingDictionary(model);
                WebHelper.MetadataService.Create(objectId, WebHelper.CurrentUserAccount, propertys);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(string objectId, string metadataId)
        {
            MetadataInfo metadataInfo = WebHelper.MetadataService.GetMetadataById(objectId, metadataId);
            MetadataEditModel editModel = new MetadataEditModel(metadataInfo);
            this.ViewBag.metadataInfoJson = JsonConvert.SerializeObject(editModel);

            FormInfo formInfo = WebHelper.FormService.GetForm(objectId, FormConstCode.EditFormCode);
            this.ViewBag.formInfo = formInfo;
            return View();
        }

        [HttpPost]
        public ActionResult EditPost(string objectId, string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                JObject model = JsonConvert.DeserializeObject<JObject>(json);
                PropertySettingDictionary propertys = ExtendHelper.MapPropertySettingDictionary(model);
                string id = propertys["id"];
                propertys.Remove("id");
                WebHelper.MetadataService.Modify(objectId, WebHelper.CurrentUserAccount, id, propertys);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(string objectId, string metadataId)
        {
            ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(objectId);
            this.ViewBag.coldewObject = coldewObject;

            FormInfo formInfo = WebHelper.FormService.GetForm(objectId, FormConstCode.DetailsFormCode);
            this.ViewBag.formInfo = formInfo;

            Dictionary<RelatedObjectInfo, List<MetadataInfo>> relateds = new Dictionary<RelatedObjectInfo, List<MetadataInfo>>();
            foreach (RelatedObjectInfo relatedObject in formInfo.Relateds)
            {
                List<MetadataInfo> relatedList = WebHelper.MetadataService.GetRelatedMetadatas(relatedObject.Object.ID, objectId, metadataId, "");
                relateds.Add(relatedObject, relatedList);
            }
            this.ViewBag.relateds = relateds;
            MetadataInfo metadataInfo = WebHelper.MetadataService.GetMetadataById(objectId, metadataId);
            this.ViewBag.metadataInfo = metadataInfo;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string objectId, string metadataIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<string> metadataIds = JsonConvert.DeserializeObject<List<string>>(metadataIdsJson);
                WebHelper.MetadataService.Delete(objectId, WebHelper.CurrentUserAccount, metadataIds);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Metadatas(string objectId, string searchInfoJson, int start, int size, string orderBy)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount;
                List<MetadataInfo> metadataInfos = null;
                if (string.IsNullOrEmpty(searchInfoJson))
                {
                    metadataInfos = WebHelper.MetadataService.GetMetadatas(objectId, WebHelper.CurrentUserAccount, start, size, orderBy, out totalCount);
                }
                else
                {
                    metadataInfos = WebHelper.MetadataService.Search(objectId, WebHelper.CurrentUserAccount, searchInfoJson, start, size, orderBy, out  totalCount);
                }
                List<MetadataInfo> favorites = WebHelper.MetadataService.GetFavorites(objectId, WebHelper.CurrentUserAccount, "");
                List<MetadataGridJObjectModel> models = metadataInfos.Select(x => new MetadataGridJObjectModel(objectId, x, favorites.ToDictionary(f => f.ID), this)).ToList();
                resultModel.data = new DatagridModel { count = totalCount, list = models };
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectMetadatas(string objectId, string keyword, int start, int size, string orderBy)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount = 0;
                List<MetadataInfo> metadataInfos = null;
                if (string.IsNullOrEmpty(keyword))
                {
                    metadataInfos = WebHelper.MetadataService.GetMetadatas(objectId, WebHelper.CurrentUserAccount, start, size, orderBy, out totalCount);
                }
                else
                {
                    metadataInfos = WebHelper.MetadataService.Search(objectId, WebHelper.CurrentUserAccount, string.Format("{{keyword: \"{0}\"}}", keyword), start, size, orderBy, out totalCount);
                }
                List<MetadataGridJObjectModel> models = metadataInfos.Select(x => new MetadataGridJObjectModel(x)).ToList();
                resultModel.data = new DatagridModel { count = totalCount, list = models };
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Export(string objectId, string searchInfoJson, string orderBy)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<MetadataInfo> metadataInfos = null;
                if (string.IsNullOrEmpty(searchInfoJson))
                {
                    metadataInfos = WebHelper.MetadataService.GetMetadatas(objectId, WebHelper.CurrentUserAccount, orderBy);
                }
                else
                {
                    metadataInfos = WebHelper.MetadataService.Search(objectId, WebHelper.CurrentUserAccount, searchInfoJson, orderBy);
                }
                List<JObject> models = metadataInfos.Select(x => new MetadataGridJObjectModel(x) as JObject).ToList();
                string tempPath = ImportExportHelper.Export(models, objectId);
                resultModel.data = System.IO.Path.GetFileName(tempPath);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportFavorite(string objectId, string orderBy)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<MetadataInfo> metadataInfos = WebHelper.MetadataService.GetFavorites(objectId, WebHelper.CurrentUserAccount, orderBy);
                List<JObject> models = metadataInfos.Select(x => new MetadataGridJObjectModel(x) as JObject).ToList();
                string tempPath = ImportExportHelper.Export(models, objectId);
                resultModel.data = System.IO.Path.GetFileName(tempPath);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportCustomized(string objectId, string viewId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<MetadataInfo> metadataInfos = WebHelper.MetadataService.GetMetadatas(objectId, viewId, WebHelper.CurrentUserAccount);
                List<JObject> models = metadataInfos.Select(x => new MetadataGridJObjectModel(x) as JObject).ToList();
                string tempPath = ImportExportHelper.Export(models, objectId);
                resultModel.data = System.IO.Path.GetFileName(tempPath);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadExportFile(string objectId, string fileName)
        {
            string filePath =Path.Combine(this.Server.MapPath("~/Temp"), fileName);
            return File(filePath, "application/octet-stream", string.Format("Customer Export {0}.xls", DateTime.Now.ToString("yyyy-MM-dd")));
        }

        public ActionResult GridViewManage(string objectId)
        {
            return View();
        }

        public ActionResult GetGridViews(string objectId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<GridViewInfo> views = WebHelper.GridViewService.GetMyGridViews(objectId, WebHelper.CurrentUserAccount);
                var models = views.Select(x => new GridViewGridModel(x, this, objectId));
                resultModel.data = models;
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateGridView(string objectId)
        {
            ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(objectId);
            this.ViewBag.coldewObject = coldewObject;

            List<ViewSetupFieldModel> fields = new List<ViewSetupFieldModel>();
            fields.AddRange(coldewObject.Fields.Select(x => new ViewSetupFieldModel(x, false, 80)));
            this.ViewBag.fields = fields;
            return View();
        }

        [HttpPost]
        public ActionResult CreateGridView(string objectId, string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                GridViewCreateModel model = JsonConvert.DeserializeObject<GridViewCreateModel>(json);
                List<GridViewColumnSetupInfo> columns = model.columns.Select(x => new GridViewColumnSetupInfo(x.fieldId, x.width)).ToList();
                string searchJson = JsonConvert.SerializeObject(model.search);
                WebHelper.GridViewService.Create(model.name, objectId, WebHelper.CurrentUserAccount, model.isShared, searchJson, columns);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGridView(string objectId, string viewId)
        {
            ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(objectId);
            this.ViewBag.coldewObject = coldewObject;

            List<ViewSetupFieldModel> fields = new List<ViewSetupFieldModel>();
            fields.AddRange(coldewObject.Fields.Select(x => new ViewSetupFieldModel(x, false, 80)));
            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(viewId);
            foreach (GridViewColumnInfo column in viewInfo.Columns)
            {
                ViewSetupFieldModel model = fields.Find(x => x.fieldId == column.FieldId);
                if (model != null)
                {
                    model.selected = true;
                    model.width = column.Width;
                }
            }
            this.ViewBag.fields = fields;
            this.ViewBag.viewInfoJson = JsonConvert.SerializeObject(new GridViewEditModel(viewInfo));
            return View();
        }

        [HttpPost]
        public ActionResult EditGridView(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                GridViewEditPostModel model = JsonConvert.DeserializeObject<GridViewEditPostModel>(json);
                List<GridViewColumnSetupInfo> columns = model.columns.Select(x => new GridViewColumnSetupInfo(x.fieldId, x.width)).ToList();
                string searchJson = JsonConvert.SerializeObject(model.search);
                WebHelper.GridViewService.Modify(model.id, model.name, model.isShared, searchJson, columns);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Customized(string objectId, string viewId)
        {
            ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(objectId);
            this.ViewBag.coldewObject = coldewObject;

            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(viewId);

            List<DataGridColumnModel> columns = viewInfo.Columns.Select(x => new DataGridColumnModel(x)).ToList();
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);
            this.ViewBag.viewId = viewInfo.ID;
            this.ViewBag.Title = viewInfo.Name;
            this.ViewBag.canSettingView = viewInfo.Creator.Account == this.CurrentUser.Account;
            return View();
        }

        public ActionResult CustomizedMetadatas(string objectId, string viewId, int start, int size, string orderBy)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount;
                List<MetadataInfo> metadataInfos = WebHelper.MetadataService.GetMetadatas(objectId, viewId, WebHelper.CurrentUserAccount, start, size, orderBy, out totalCount);
                List<MetadataInfo> favorites = WebHelper.MetadataService.GetFavorites(objectId, WebHelper.CurrentUserAccount, "");
                List<MetadataGridJObjectModel> models = metadataInfos.Select(x => new MetadataGridJObjectModel(objectId, x, favorites.ToDictionary(f => f.ID), this)).ToList();
                resultModel.data = new DatagridModel { count = totalCount, list = models };
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewSetup(string viewId, string objectId)
        {
            ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(objectId);

            this.ViewBag.viewId = viewId;
            this.ViewBag.objectId = objectId;

            List<ViewSetupFieldModel> fields = new List<ViewSetupFieldModel>();
            fields.AddRange(coldewObject.Fields.Select(x => new ViewSetupFieldModel(x, false, 80)));

            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(viewId);
            foreach (GridViewColumnInfo column in viewInfo.Columns)
            {
                ViewSetupFieldModel model = fields.Find(x => x.fieldId == column.FieldId);
                if (model != null)
                {
                    model.selected = true;
                    model.width = column.Width;
                }
            }

            this.ViewBag.fields = fields;
            return this.PartialView();
        }

        [HttpPost]
        public ActionResult SetViewSetup(string viewId, string columnsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<GridViweColumnSetupModel> columnModels = JsonConvert.DeserializeObject<List<GridViweColumnSetupModel>>(columnsJson);
                List<GridViewColumnSetupInfo> columns = columnModels.Select(x => new GridViewColumnSetupInfo(x.fieldId, x.width)).ToList();
                WebHelper.GridViewService.Modify(viewId, columns);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

    }
}
