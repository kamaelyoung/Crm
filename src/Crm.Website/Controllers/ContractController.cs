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
    public class ContractController : BaseController
    {
        public ActionResult Index()
        {
            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(GridViewType.ContractManage, WebHelper.CurrentUserAccount);

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
            string jsonFilePath = ImportExportHelper.GetUploadImportFileJsonFile(FormType.Contract, this);
            return Redirect( string.Format("{0}?tempFileName={1}", this.Url.Action("ImportSecond"), Path.GetFileName(jsonFilePath)));
        }

        public ActionResult ImportSecond()
        {
            List<DataGridColumnModel> columns = ImportExportHelper.GetImportColumns(FormType.Contract);
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);

            return View();
        }

        [HttpPost]
        public ActionResult Import(string tempFileName)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                IList<UserInfo> userList = WebHelper.UserService.GetAllUser();
                Dictionary<string, UserInfo> userAccountDic = userList.ToDictionary(x => x.Account);
                Dictionary<string, UserInfo> userNameDic = userList.ToDictionary(x => x.Name);

                Dictionary<string, CustomerInfo> customers = WebHelper.CustomerService.GetCustomers(WebHelper.CurrentUserAccount).ToDictionary(x => x.Name);
                string tempFilePath = Path.Combine(Server.MapPath("~/Temp"), tempFileName);
                List<JObject> importModels = JsonConvert.DeserializeObject<List<JObject>>(System.IO.File.ReadAllText(tempFilePath));

                foreach (JObject model in importModels)
                {
                    string contractName = model["name"].ToString();
                    string customerId = "";
                    string customerName = model["customer"].ToString();
                    DateTime startDate;
                    if (!DateTime.TryParse(model["startDate"].ToString(), out startDate))
                    {
                        model["importResult"] = false;
                        model["importMessage"] = "导入失败，开始时间格式不正确";
                        continue;
                    }
                    DateTime endDate;
                    if (!DateTime.TryParse(model["endDate"].ToString(), out endDate))
                    {
                        model["importResult"] = false;
                        model["importMessage"] = "导入失败，结束时间格式不正确";
                        continue;
                    }
                    int expiredComputeDays;
                    if (!int.TryParse(model["expiredComputeDays"].ToString(), out expiredComputeDays))
                    {
                        model["importResult"] = false;
                        model["importMessage"] = "导入失败，到期计算天数格式不正确";
                        continue;
                    }
                    float value;
                    if (!float.TryParse(model["value"].ToString(), out value))
                    {
                        model["importResult"] = false;
                        model["importMessage"] = "导入失败，合同金额格式不正确";
                        continue;
                    }
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
                    List<string> ownerAccounts = null;
                    try
                    {
                        ownerAccounts = ImportExportHelper.GetUserAccounts(model["owners"].ToString().Split(','), userAccountDic, userNameDic);
                    }
                    catch (Exception ex)
                    {
                        model["importMessage"] = "导入失败," + ex.Message;
                        model["importResult"] = false;
                        continue;
                    }
                    List<PropertyOperationInfo> propertys = new List<PropertyOperationInfo>();
                    List<FieldInfo> fieldInfos = WebHelper.FormService.GetFields(FormType.Contract).Where(x => !x.CanModify && x.CanImport).ToList();
                    foreach (FieldInfo filed in fieldInfos)
                    {
                        propertys.Add(new PropertyOperationInfo { Code = filed.Code, Value = model[filed.Code].ToString() });
                    }
                    try
                    {
                        WebHelper.ContractService.Create(WebHelper.CurrentUserAccount, contractName, customerId, startDate, endDate,
                            expiredComputeDays, value, ownerAccounts, propertys);

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

        public ActionResult GetImportContracts(string tempFileName, int start, int size)
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
            string tempPath = ImportExportHelper.GetImportTemplate(FormType.Contract, this);
            return File(tempPath, "application/octet-stream", "Contract Import Template.xls");
        }

        public ActionResult Expired()
        {
            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(GridViewType.ExpiredContract, WebHelper.CurrentUserAccount);

            List<DataGridColumnModel> columns = viewInfo.Columns.Select(x => new DataGridColumnModel(x)).ToList();
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);
            return View();
        }

        public ActionResult Expiring()
        {
            GridViewInfo viewInfo = WebHelper.GridViewService.GetGridView(GridViewType.ExpiringContract, WebHelper.CurrentUserAccount);

            List<DataGridColumnModel> columns = viewInfo.Columns.Select(x => new DataGridColumnModel(x)).ToList();
            this.ViewBag.columnsJson = JsonConvert.SerializeObject(columns);
            return View();
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
                ContractCreateModel model = JsonConvert.DeserializeObject<ContractCreateModel>(json);
                List<PropertyOperationInfo> propertys = MetadataHelper.MapPropertyOperationInfos(model.extends);
                WebHelper.ContractService.Create(WebHelper.CurrentUserAccount, model.name, model.customerId, model.startDate, 
                    model.endDate, model.expiredComputeDays, model.value, model.owners, propertys);
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
        public ActionResult Edit(string contractId)
        {
            ContractInfo contractInfo = WebHelper.ContractService.GetContractById(contractId);
            ContractEditModel editModel = new ContractEditModel(contractInfo);
            this.ViewBag.contractInfoJson = JsonConvert.SerializeObject(editModel);

            return View();
        }

        [HttpPost]
        public ActionResult EditPost(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                ContractEditPostModel model = JsonConvert.DeserializeObject<ContractEditPostModel>(json);
                List<PropertyOperationInfo> propertys = MetadataHelper.MapPropertyOperationInfos(model.extends);
                WebHelper.ContractService.Modify(WebHelper.CurrentUserAccount, model.id, model.name, model.startDate, model.endDate,
                    model.expiredComputeDays, model.value, model.owners, propertys);
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
        public ActionResult Delete(string contractId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                WebHelper.ContractService.Delete(WebHelper.CurrentUserAccount, contractId);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contracts(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                ContractSearchModel searchModel = JsonConvert.DeserializeObject<ContractSearchModel>(json);
                ContractSearchInfo searchInfo = searchModel.Map();
                int totalCount;
                List<ContractInfo> contractInfos = null;
                if (searchInfo == null)
                {
                    contractInfos = WebHelper.ContractService.GetContracts(WebHelper.CurrentUserAccount, searchModel.start, searchModel.size, out totalCount);
                }
                else
                {
                    contractInfos = WebHelper.ContractService.Search(WebHelper.CurrentUserAccount, searchInfo, searchModel.start, searchModel.size, out  totalCount);
                }
                List<ContractGridJObjectModel> models = contractInfos.Select(x => new ContractGridJObjectModel(x, this)).ToList();
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

        public ActionResult ExpiredContracts(int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount;
                List<ContractInfo> contractInfos = WebHelper.ContractService.GetExpiredContracts(WebHelper.CurrentUserAccount, start, size, out totalCount);
                List<ContractGridJObjectModel> models = contractInfos.Select(x => new ContractGridJObjectModel(x, this)).ToList();
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

        public ActionResult ExpiringContracts(int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int totalCount;
                List<ContractInfo> contractInfos = WebHelper.ContractService.GetExpiringContracts(WebHelper.CurrentUserAccount, start, size, out totalCount);
                List<ContractGridJObjectModel> models = contractInfos.Select(x => new ContractGridJObjectModel(x, this)).ToList();
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

        public ActionResult Export(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                ContractSearchInfo searchInfo = null;
                if (!string.IsNullOrEmpty(json))
                {
                    ContractSearchModel searchModel = JsonConvert.DeserializeObject<ContractSearchModel>(json); 
                    searchInfo = searchModel.Map();
                }
                List<ContractInfo> contractInfos = null;
                if (searchInfo == null)
                {
                    contractInfos = WebHelper.ContractService.GetContracts(WebHelper.CurrentUserAccount);
                }
                else
                {
                    contractInfos = WebHelper.ContractService.Search(WebHelper.CurrentUserAccount, searchInfo);
                }
                List<JObject> models = contractInfos.Select(x => new ContractGridJObjectModel(x) as JObject).ToList();
                string tempPath = ImportExportHelper.Export(models, FormType.Contract);
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
            return File(filePath, "application/octet-stream", string.Format("Contract Export {0}.xls", DateTime.Now.ToString("yyyy-MM-dd")));
        }

    }
}
