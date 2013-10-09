using System;
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

namespace Coldew.Website.Controllers
{
    public class MetadataController : BaseController
    {

        public ActionResult Index(string formId)
        {
            List<GridViewInfo> views = WebHelper.GridViewService.GetGridViews(formId, WebHelper.CurrentUserAccount);
            GridViewInfo viewInfo = views.Find(x => x.Type == GridViewType.Manage);

            List<DataGridColumnModel> columns = viewInfo.Columns.Select(x => new DataGridColumnModel(x)).ToList();
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);
            this.ViewBag.viewId = viewInfo.ID;
            this.ViewBag.Title = viewInfo.Name;
            return View();
        }

        public ActionResult SelectDialog()
        {
            return this.PartialView();
        }

        public ActionResult Favorite(string viewId)
        {
            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(viewId);

            List<DataGridColumnModel> columns = viewInfo.Columns.Select(x => new DataGridColumnModel(x)).ToList();
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);
            this.ViewBag.viewId = viewInfo.ID;
            this.ViewBag.Title = viewInfo.Name;
            return View();
        }

        public ActionResult Favorites(string formId, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<MetadataInfo> favorites = WebHelper.MetadataService.GetFavorites(formId, WebHelper.CurrentUserAccount);
                List<MetadataGridJObjectModel> models = favorites.Skip(start).Take(size).Select(x => new MetadataGridJObjectModel(formId, x, this)).ToList();
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
        public ActionResult Favorites(string formId, string metadataIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<string> metadataIds = JsonConvert.DeserializeObject<List<string>>(metadataIdsJson);
                WebHelper.MetadataService.Favorite(formId, WebHelper.CurrentUserAccount, metadataIds);
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
        public ActionResult CancelFavorite(string formId, string metadataIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<string> metadataIds = JsonConvert.DeserializeObject<List<string>>(metadataIdsJson);
                WebHelper.MetadataService.CancelFavorite(formId, WebHelper.CurrentUserAccount, metadataIds);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImportFirst()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadImportFile(string formId)
        {
            string jsonFilePath = ImportExportHelper.GetUploadImportFileJsonFile(formId, this);
            return Redirect(string.Format("~/Customer/ImportSecond?tempFileName={0}", Path.GetFileName(jsonFilePath)));
        }

        public ActionResult ImportSecond(string formId)
        {
            List<DataGridColumnModel> columns = ImportExportHelper.GetImportColumns(formId);
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);

            return View();
        }

        [HttpPost]
        public ActionResult Import(string tempFileName, string formId)
        {

            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                string tempFilePath = Path.Combine(Server.MapPath("~/Temp"), tempFileName);
                List<JObject> importModels = JsonConvert.DeserializeObject<List<JObject>>(System.IO.File.ReadAllText(tempFilePath));

                foreach (JObject model in importModels)
                {
                    PropertySettingDictionary propertys = new PropertySettingDictionary();
                    List<FieldInfo> fieldInfos = WebHelper.FormService.GetFields(formId).Where(x => x.CanInput).ToList();
                    foreach (FieldInfo filed in fieldInfos)
                    {
                        propertys.Add(filed.Code, model[filed.Code].ToString());
                    }
                    try
                    {
                        WebHelper.MetadataService.Create(formId, WebHelper.CurrentUserAccount, propertys);

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

        public ActionResult GetImportCustomers(string tempFileName, int start, int size)
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

        public ActionResult DownloadImportTemplate(string formId)
        {
            string tempPath = ImportExportHelper.GetImportTemplate(formId, this);
            return File(tempPath, "application/octet-stream", "Customer Import Template.xls");
        }

        [HttpGet]
        public ActionResult Create(string formId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string formId, string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                JObject model = JsonConvert.DeserializeObject<JObject>(json);
                PropertySettingDictionary propertys = ExtendHelper.MapPropertySettingDictionary(model);
                WebHelper.MetadataService.Create(formId, WebHelper.CurrentUserAccount, propertys);
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
        public ActionResult Edit(string formId, string metadataId)
        {
            MetadataInfo metadataInfo = WebHelper.MetadataService.GetMetadataById(formId, metadataId);
            MetadataEditModel editModel = new MetadataEditModel(metadataInfo);
            this.ViewBag.metadataInfoJson = JsonConvert.SerializeObject(editModel);

            return View();
        }

        [HttpPost]
        public ActionResult EditPost(string formId, string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                JObject model = JsonConvert.DeserializeObject<JObject>(json);
                PropertySettingDictionary propertys = ExtendHelper.MapPropertySettingDictionary(model);
                string id = propertys["id"];
                propertys.Remove("id");
                WebHelper.MetadataService.Modify(formId, WebHelper.CurrentUserAccount, id, propertys);
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
        public ActionResult Delete(string formId, string metadataIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<string> metadataIds = JsonConvert.DeserializeObject<List<string>>(metadataIdsJson);
                WebHelper.MetadataService.Delete(formId, WebHelper.CurrentUserAccount, metadataIds);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Metadatas(string formId, string searchInfoJson, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount;
                List<MetadataInfo> metadataInfos = null;
                if (string.IsNullOrEmpty(searchInfoJson))
                {
                    metadataInfos = WebHelper.MetadataService.GetMetadatas(formId, WebHelper.CurrentUserAccount, start, size, out totalCount);
                }
                else
                {
                    metadataInfos = WebHelper.MetadataService.Search(formId, WebHelper.CurrentUserAccount, searchInfoJson, start, size, out  totalCount);
                }
                List<MetadataInfo> favorites = WebHelper.MetadataService.GetFavorites(formId, WebHelper.CurrentUserAccount);
                List<MetadataGridJObjectModel> models = metadataInfos.Select(x => new MetadataGridJObjectModel(formId, x, favorites.ToDictionary(f => f.ID), this)).ToList();
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

        public ActionResult SelectMetadatas(string formId, string keyword, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount = 0;
                List<MetadataInfo> metadataInfos = null;
                if (string.IsNullOrEmpty(keyword))
                {
                    metadataInfos = WebHelper.MetadataService.GetMetadatas(formId, WebHelper.CurrentUserAccount, start, size, out totalCount);
                }
                else
                {
                    metadataInfos = WebHelper.MetadataService.Search(formId, WebHelper.CurrentUserAccount, string.Format("{{keyword: \"{0}\"}}", keyword), start, size, out totalCount);
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

        public ActionResult Export(string formId, string searchInfoJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<MetadataInfo> metadataInfos = null;
                if (string.IsNullOrEmpty(searchInfoJson))
                {
                    metadataInfos = WebHelper.MetadataService.GetMetadatas(formId, WebHelper.CurrentUserAccount);
                }
                else
                {
                    metadataInfos = WebHelper.MetadataService.Search(formId, WebHelper.CurrentUserAccount, searchInfoJson);
                }
                List<JObject> models = metadataInfos.Select(x => new MetadataGridJObjectModel(x) as JObject).ToList();
                string tempPath = ImportExportHelper.Export(models, formId);
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

        public ActionResult ExportFavorite(string formId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<MetadataInfo> metadataInfos = WebHelper.MetadataService.GetFavorites(formId, WebHelper.CurrentUserAccount);
                List<JObject> models = metadataInfos.Select(x => new MetadataGridJObjectModel(x) as JObject).ToList();
                string tempPath = ImportExportHelper.Export(models, formId);
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

        public ActionResult ExportCustomized(string formId, string viewId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<MetadataInfo> metadataInfos = WebHelper.MetadataService.GetMetadatas(formId, viewId, WebHelper.CurrentUserAccount);
                List<JObject> models = metadataInfos.Select(x => new MetadataGridJObjectModel(x) as JObject).ToList();
                string tempPath = ImportExportHelper.Export(models, formId);
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

        public ActionResult DownloadExportFile(string formId, string fileName)
        {
            string filePath =Path.Combine(this.Server.MapPath("~/Temp"), fileName);
            return File(filePath, "application/octet-stream", string.Format("Customer Export {0}.xls", DateTime.Now.ToString("yyyy-MM-dd")));
        }

        public ActionResult GridViewManage(string formId)
        {
            return View();
        }

        public ActionResult GetGridViews(string formId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<GridViewInfo> views = WebHelper.GridViewService.GetMyGridViews(formId, WebHelper.CurrentUserAccount);
                var models = views.Select(x => new GridViewGridModel(x, this, formId));
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

        public ActionResult CreateGridView(string formId)
        {
            List<ViewSetupFieldModel> fields = new List<ViewSetupFieldModel>();
            List<FieldInfo> fieldInfos = WebHelper.FormService.GetFields(formId);
            fields.AddRange(fieldInfos.Select(x => new ViewSetupFieldModel { fieldId = x.ID, name = x.Name }));
            this.ViewBag.fields = fields;
            return View();
        }

        [HttpPost]
        public ActionResult CreateGridView(string formId, string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                GridViewCreateModel model = JsonConvert.DeserializeObject<GridViewCreateModel>(json);
                List<GridViewColumnSetupInfo> columns = model.columns.Select(x => new GridViewColumnSetupInfo { FieldId = x.fieldId, Width = x.width }).ToList();
                string searchJson = JsonConvert.SerializeObject(model.search);
                WebHelper.GridViewService.Create(model.name, formId, WebHelper.CurrentUserAccount, false, searchJson, columns);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGridView(string formId, string viewId)
        {
            List<ViewSetupFieldModel> fields = new List<ViewSetupFieldModel>();
            List<FieldInfo> fieldInfos = WebHelper.FormService.GetFields(formId);
            fields.AddRange(fieldInfos.Select(x => new ViewSetupFieldModel { fieldId = x.ID, name = x.Name }));
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
                List<GridViewColumnSetupInfo> columns = model.columns.Select(x => new GridViewColumnSetupInfo { FieldId = x.fieldId, Width = x.width }).ToList();
                string searchJson = JsonConvert.SerializeObject(model.search);
                WebHelper.GridViewService.Modify(model.id, model.name, false, searchJson, columns);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Customized(string viewId)
        {
            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(viewId);

            List<DataGridColumnModel> columns = viewInfo.Columns.Select(x => new DataGridColumnModel(x)).ToList();
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);
            this.ViewBag.viewId = viewInfo.ID;
            this.ViewBag.Title = viewInfo.Name;
            return View();
        }

        [HttpGet]
        public ActionResult ViewSetup(string viewId, string formId)
        {
            this.ViewBag.viewId = viewId;
            this.ViewBag.formId = formId;
            return View();
        }
    }
}
