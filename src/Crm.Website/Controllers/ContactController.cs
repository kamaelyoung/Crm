using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crm.Api;
using Crm.Website.Models;
using Newtonsoft.Json;
using Crm.Api.Organization;
using System.IO;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using System.Data;
using System.Text.RegularExpressions;

namespace Crm.Website.Controllers
{
    public class ContactController : BaseController
    {
        public ActionResult Index()
        {
            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(GridViewType.ContactManage, WebHelper.CurrentUserAccount);

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
            string jsonFilePath = ImportExportHelper.GetUploadImportFileJsonFile(FormType.Contact, this);
            return Redirect(string.Format("~/Contact/ImportSecond?tempFileName={0}", Path.GetFileName(jsonFilePath)));
        }

        public ActionResult ImportSecond()
        {
            List<DataGridColumnModel> columns = ImportExportHelper.GetImportColumns(FormType.Contact);
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);

            return View();
        }

        [HttpPost]
        public ActionResult Import(string tempFileName)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                Dictionary<string, CustomerInfo> customers = WebHelper.CustomerService.GetCustomers(WebHelper.CurrentUserAccount).ToDictionary(x => x.Name);
                string tempFilePath = Path.Combine(Server.MapPath("~/Temp"), tempFileName);
                List<JObject> importModels = JsonConvert.DeserializeObject<List<JObject>>(System.IO.File.ReadAllText(tempFilePath));

                foreach (JObject model in importModels)
                {
                    string contactName = model["name"].ToString();
                    string customerId = "";
                    string customerName = model["customer"].ToString();
                    if (customers.ContainsKey(customerName))
                    {
                        customerId = customers[customerName].ID;
                    }
                    else
                    {
                        model["importResult"] = false;
                        model["importMessage"] = "导入失败，客户名称不正确";
                        continue;
                    }
                    List<PropertyOperationInfo> propertys = new List<PropertyOperationInfo>();
                    List<FieldInfo> fieldInfos = WebHelper.FormService.GetFields(FormType.Contact).Where(x => !x.CanModify && x.CanImport).ToList();
                    foreach (FieldInfo filed in fieldInfos)
                    {
                        propertys.Add(new PropertyOperationInfo { Code = filed.Code, Value = model[filed.Code].ToString() });
                    }
                    try
                    {
                        WebHelper.ContactService.Create(WebHelper.CurrentUserAccount, contactName, customerId, propertys);

                        model["importResult"] = true;
                        model["importMessage"] = "导入成功";
                    }
                    catch (Exception ex)
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

        public ActionResult GetImportContacts(string tempFileName, int start, int size)
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
            string tempPath = ImportExportHelper.GetImportTemplate(FormType.Contact, this);
            return File(tempPath, "application/octet-stream", "Contact Import Template.xls");
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
                ContactCreateModel model = JsonConvert.DeserializeObject<ContactCreateModel>(json);
                List<PropertyOperationInfo> propertys = ExtendHelper.MapPropertyOperationInfos(model.extends);
                WebHelper.ContactService.Create(WebHelper.CurrentUserAccount, model.name, model.customerId, propertys);
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
        public ActionResult Edit(string contactId)
        {
            ContactInfo contactInfo = WebHelper.ContactService.GetContactById(contactId);
            ContactEditModel editModel = new ContactEditModel(contactInfo);
            this.ViewBag.contactInfoJson = JsonConvert.SerializeObject(editModel);

            return View();
        }

        [HttpPost]
        public ActionResult EditPost(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                ContactEditPostModel model = JsonConvert.DeserializeObject<ContactEditPostModel>(json);
                List<PropertyOperationInfo> propertys = ExtendHelper.MapPropertyOperationInfos(model.extends);
                WebHelper.ContactService.Modify(WebHelper.CurrentUserAccount, model.id, model.name, propertys);
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
        public ActionResult Delete(string contactId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                WebHelper.ContactService.Delete(WebHelper.CurrentUserAccount, contactId);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contacts(string keyword, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount;
                List<ContactInfo> contactInfos = null;
                if (string.IsNullOrEmpty(keyword))
                {
                    contactInfos = WebHelper.ContactService.GetContacts(WebHelper.CurrentUserAccount, start, size, out totalCount);
                }
                else
                {
                    Regex regex = new Regex("\\s*");
                    keyword = regex.Replace(keyword, " ");
                    contactInfos = WebHelper.ContactService.Search(WebHelper.CurrentUserAccount, keyword.Split(' ').ToList(), start, size, out  totalCount);
                }
                List<ContactGridJObjectModel> models = contactInfos.Select(x => new ContactGridJObjectModel(x, this)).ToList();
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

        public ActionResult SelectDialog()
        {
            return this.PartialView();
        }

        public ActionResult SelectCustomers(string keyword, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount;
                List<ContactInfo> contactInfos = null;
                if (string.IsNullOrEmpty(keyword))
                {
                    contactInfos = WebHelper.ContactService.GetContacts(WebHelper.CurrentUserAccount, start, size, out totalCount);
                }
                else
                {
                    Regex regex = new Regex("\\s*");
                    keyword = regex.Replace(keyword, " ");
                    contactInfos = WebHelper.ContactService.Search(WebHelper.CurrentUserAccount, keyword.Split(' ').ToList(), start, size, out  totalCount);
                }
                List<ContactGridJObjectModel> models = contactInfos.Select(x => new ContactGridJObjectModel(x)).ToList();
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

        public ActionResult Export(string keyword)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<ContactInfo> contactInfos = null;
                if (string.IsNullOrEmpty(keyword))
                {
                    contactInfos = WebHelper.ContactService.GetContacts(WebHelper.CurrentUserAccount);
                }
                else
                {
                    Regex regex = new Regex("\\s*");
                    keyword = regex.Replace(keyword, " ");
                    contactInfos = WebHelper.ContactService.Search(WebHelper.CurrentUserAccount, keyword.Split(' ').ToList());
                }
                List<JObject> models = contactInfos.Select(x => new ContactGridJObjectModel(x) as JObject).ToList();
                string tempPath = ImportExportHelper.Export(models, FormType.Contact);
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
            string filePath = Path.Combine(this.Server.MapPath("~/Temp"), fileName);
            return File(filePath, "application/octet-stream", string.Format("Contact Export {0}.xls", DateTime.Now.ToString("yyyy-MM-dd")));
        }
    }
}
