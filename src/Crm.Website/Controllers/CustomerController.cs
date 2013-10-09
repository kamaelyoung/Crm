using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crm.Api;
using Crm.Website.Models;
using Newtonsoft.Json;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Data;
using Newtonsoft.Json.Linq;
using Crm.Api.Organization;
using System.Text.RegularExpressions;

namespace Crm.Website.Controllers
{
    public class CustomerController : BaseController
    {

        public ActionResult Index()
        {
            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(GridViewType.CustomerManage, WebHelper.CurrentUserAccount);

            List<DataGridColumnModel> columns = viewInfo.Columns.Select(x => new DataGridColumnModel(x)).ToList();
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);
            return View();
        }

        public ActionResult SelectDialog()
        {
            return this.PartialView();
        }

        public ActionResult Favorite()
        {
            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(GridViewType.CustomerFavorite, WebHelper.CurrentUserAccount);

            List<DataGridColumnModel> columns = viewInfo.Columns.Select(x => new DataGridColumnModel(x)).ToList();
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);
            return View();
        }

        public ActionResult ImportFirst()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadImportFile()
        {
            string jsonFilePath = ImportExportHelper.GetUploadImportFileJsonFile(FormType.Customer, this);
            return Redirect(string.Format("~/Customer/ImportSecond?tempFileName={0}", Path.GetFileName(jsonFilePath)));
        }

        public ActionResult ImportSecond()
        {
            List<DataGridColumnModel> columns = ImportExportHelper.GetImportColumns(FormType.Customer);
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);

            return View();
        }

        [HttpPost]
        public ActionResult Import(string tempFileName)
        {

            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                string tempFilePath = Path.Combine(Server.MapPath("~/Temp"), tempFileName);
                List<JObject> importModels = JsonConvert.DeserializeObject<List<JObject>>(System.IO.File.ReadAllText(tempFilePath));

                foreach (JObject model in importModels)
                {
                    PropertySettingDictionary propertys = new PropertySettingDictionary();
                    List<FieldInfo> fieldInfos = WebHelper.FormService.GetFields(FormType.Customer).Where(x => x.CanInput).ToList();
                    foreach (FieldInfo filed in fieldInfos)
                    {
                        propertys.Add(filed.Code, model[filed.Code].ToString());
                    }
                    try
                    {
                        WebHelper.CustomerService.Create(WebHelper.CurrentUserAccount, propertys);

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

        public ActionResult DownloadImportTemplate()
        {
            string tempPath = ImportExportHelper.GetImportTemplate(FormType.Customer, this);
            return File(tempPath, "application/octet-stream", "Customer Import Template.xls");
        }

        [HttpGet]
        public ActionResult ViewSetup(GridViewType viewType)
        {
            this.ViewBag.viewType = viewType;
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                JObject model = JsonConvert.DeserializeObject<JObject>(json);
                PropertySettingDictionary propertys = ExtendHelper.MapPropertySettingDictionary(model);
                WebHelper.CustomerService.Create(WebHelper.CurrentUserAccount, propertys);
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
        public ActionResult Edit(string customerId)
        {
            MetadataInfo customerInfo = WebHelper.CustomerService.GetCustomerById(customerId);
            CustomerEditModel editModel = new CustomerEditModel(customerInfo);
            this.ViewBag.customerInfoJson = JsonConvert.SerializeObject(editModel);

            return View();
        }

        [HttpPost]
        public ActionResult EditPost(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                JObject model = JsonConvert.DeserializeObject<JObject>(json);
                PropertySettingDictionary propertys = ExtendHelper.MapPropertySettingDictionary(model);
                string id = propertys["id"];
                propertys.Remove("id");
                WebHelper.CustomerService.Modify(WebHelper.CurrentUserAccount, id, propertys);
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
        public ActionResult Delete(string customerIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<string> customerIds = JsonConvert.DeserializeObject<List<string>>(customerIdsJson);
                WebHelper.CustomerService.Delete(WebHelper.CurrentUserAccount, customerIds);
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
        public ActionResult Favorite(string customerIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<string> customerIds = JsonConvert.DeserializeObject<List<string>>(customerIdsJson);
                WebHelper.CustomerService.Favorite(WebHelper.CurrentUserAccount, customerIds);
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
        public ActionResult CancelFavorite(string customerIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<string> customerIds = JsonConvert.DeserializeObject<List<string>>(customerIdsJson);
                WebHelper.CustomerService.CancelFavorite(WebHelper.CurrentUserAccount, customerIds);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Customers(string searchInfoJson, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount;
                List<MetadataInfo> customerInfos = null;
                if (string.IsNullOrEmpty(searchInfoJson))
                {
                    customerInfos = WebHelper.CustomerService.GetCustomers(WebHelper.CurrentUserAccount, start, size, out totalCount);
                }
                else
                {
                    CustomerSearchModel searchModel = JsonConvert.DeserializeObject<CustomerSearchModel>(searchInfoJson);
                    MetadataSearchInfo serachInfo = searchModel.Map();
                    customerInfos = WebHelper.CustomerService.Search(WebHelper.CurrentUserAccount, serachInfo, start, size, out  totalCount);
                }
                List<MetadataInfo> favorites = WebHelper.CustomerService.GetFavorites(WebHelper.CurrentUserAccount);
                List<CustomerGridJObjectModel> models = customerInfos.Select(x => new CustomerGridJObjectModel(x, favorites.ToDictionary(f => f.ID), this)).ToList();
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

        public ActionResult SelectCustomers(string keyword, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount;
                List<MetadataInfo> customerInfos = null;
                if (string.IsNullOrEmpty(keyword))
                {
                    customerInfos = WebHelper.CustomerService.GetCustomers(WebHelper.CurrentUserAccount, start, size, out totalCount);
                }
                else
                {
                    CustomerSearchModel searchModel = new CustomerSearchModel();
                    searchModel.keyword = keyword;
                    customerInfos = WebHelper.CustomerService.Search(WebHelper.CurrentUserAccount, searchModel.Map(), start, size, out  totalCount);
                }
                List<CustomerGridJObjectModel> models = customerInfos.Select(x => new CustomerGridJObjectModel(x)).ToList();
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

        public ActionResult Favorites(int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<MetadataInfo> favorites = WebHelper.CustomerService.GetFavorites(WebHelper.CurrentUserAccount);
                List<CustomerGridJObjectModel> models = favorites.Skip(start).Take(size).Select(x => new CustomerGridJObjectModel(x, this)).ToList();
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

        public ActionResult Export(string keyword)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<MetadataInfo> customerInfos = null;
                if (string.IsNullOrEmpty(keyword))
                {
                    customerInfos = WebHelper.CustomerService.GetCustomers(WebHelper.CurrentUserAccount);
                }
                else
                {
                    CustomerSearchModel searchModel = new CustomerSearchModel();
                    searchModel.keyword = keyword;
                    customerInfos = WebHelper.CustomerService.Search(WebHelper.CurrentUserAccount, searchModel.Map());
                }
                List<JObject> models = customerInfos.Select(x => new CustomerGridJObjectModel(x) as JObject).ToList();
                string tempPath = ImportExportHelper.Export(models, FormType.Customer);
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

        public ActionResult DownloadExportFile(string fileName)
        {
            string filePath =Path.Combine(this.Server.MapPath("~/Temp"), fileName);
            return File(filePath, "application/octet-stream", string.Format("Customer Export {0}.xls", DateTime.Now.ToString("yyyy-MM-dd")));
        }
    }
}
