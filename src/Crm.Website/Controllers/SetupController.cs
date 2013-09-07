using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crm.Website.Models;
using Crm.Api;
using Newtonsoft.Json;

namespace Crm.Website.Controllers
{
    public class SetupController : BaseController
    {
        //
        // GET: /Setup/

        public ActionResult Index()
        {
            EmailConfigInfo info = WebHelper.CrmConfigService.GetEmailConfig();
            EmailConfigModel emailConfigModel = new EmailConfigModel(info);
            this.ViewBag.emailConfigModelJson = JsonConvert.SerializeObject(emailConfigModel);
            return View();
        }

        public ActionResult CustomerArea()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateCustomerArea()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SetEmailConfig(string account, string address, string password, string server)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                WebHelper.CrmConfigService.SetEmailConfig(account, address, password, server);
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
        public ActionResult TestEmailConfig(string account, string address, string password, string server)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                WebHelper.CrmConfigService.TestEmailConfig(account, address, password, server);
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
        public ActionResult CreateCustomerArea(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                CustomerAreaCreateModel model = JsonConvert.DeserializeObject<CustomerAreaCreateModel>(json);
                WebHelper.CustomerAreaService.Create(model.name, model.managerAccounts);
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
        public ActionResult EditCustomerArea(int areaId)
        {
            CustomerAreaInfo areaInfo = WebHelper.CustomerAreaService.GetAreaById(areaId);
            this.ViewBag.editModelJson = JsonConvert.SerializeObject(new CustomerAreaEditModel(areaInfo));
            return View();
        }

        [HttpPost]
        public ActionResult EditCustomerArea(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                CustomerAreaEditModel model = JsonConvert.DeserializeObject<CustomerAreaEditModel>(json);
                WebHelper.CustomerAreaService.Modify(model.id, model.name, model.managerAccounts);
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
        public ActionResult DeleteCustomerAreas(string areaIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<int> areaIds = JsonConvert.DeserializeObject<List<int>>(areaIdsJson);
                foreach (int areaId in areaIds)
                {
                    WebHelper.CustomerAreaService.Delete(areaId);
                }
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
        public ActionResult CustomerAreas()
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<CustomerAreaInfo> areaInfos = WebHelper.CustomerAreaService.GetAllArea();
                resultModel.data = areaInfos.Select(x => new CustomerAreaGridModel(x, this)).ToList();
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateField()
        {
            return View();
        }

        public ActionResult Extend(FormType formType)
        {
            FormInfo formInfo = WebHelper.FormService.GetFormByType(formType);
            this.ViewBag.formType = formType;
            this.ViewBag.Title = formInfo.Name + "扩展";

            return View();
        }

        public ActionResult GetFields(FormType formType)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<FieldInfo> fields = WebHelper.FormService.GetFields(formType).Where(x => !x.CanModify).ToList();
                resultModel.data = fields.Select(x => new FieldGridModel(x, this));
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFields(string fieldIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<int> fieldIds = JsonConvert.DeserializeObject<List<int>>(fieldIdsJson);
                foreach (int fielId in fieldIds)
                {
                    WebHelper.FormService.DeleteField(WebHelper.CurrentUserAccount, fielId);
                }
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
        public ActionResult CreateStringField(FormType formType)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formType) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateStringField(FormType formType, string name, bool? required, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.CreateStringField(formType, name, required.Value, defaultValue, index);
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
        public ActionResult CreateDateField(FormType formType)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formType) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateDateField(FormType formType, string name, bool? required, bool defaultValueIsToday, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.CreateDateField(formType, name, required.Value, defaultValueIsToday, index);
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
        public ActionResult CreateNumberField(FormType formType)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formType) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateNumberField(FormType formType, string name, bool? required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.CreateNumberField(formType, name, required.Value, defaultValue, max, min, precision, index);
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
        public ActionResult EditStringField(int fieldId)
        {
            FieldInfo field = WebHelper.FormService.GetField(fieldId);
            StringFieldConfigInfo config = field.ConfigInfo as StringFieldConfigInfo;
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new StringFieldEditModel(field, config));
            return View();
        }

        [HttpPost]
        public ActionResult EditStringField(FormType formType, int fieldId, string name, bool? required, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.ModifyStringField(fieldId, name, required.Value, defaultValue, index);
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
        public ActionResult EditDateField(int fieldId)
        {
            FieldInfo field = WebHelper.FormService.GetField(fieldId);
            DateFieldConfigInfo config = field.ConfigInfo as DateFieldConfigInfo;
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new DateFieldEditModel(field, config));
            return View();
        }

        [HttpPost]
        public ActionResult EditDateField(FormType formType, int fieldId, string name, bool? required, bool defaultValueIsToday, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.ModifyDateField(fieldId, name, required.Value, defaultValueIsToday, index);
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
        public ActionResult EditNumberField(int fieldId)
        {
            FieldInfo field = WebHelper.FormService.GetField(fieldId);
            NumberFieldConfigInfo config = field.ConfigInfo as NumberFieldConfigInfo;
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new NumberFieldEditModel(field, config));
            return View();
        }

        [HttpPost]
        public ActionResult EditNumberField(FormType formType, int fieldId, string name, bool? required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.ModifyNumberField(fieldId, name, required.Value, defaultValue, max, min, precision, index);
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
        public ActionResult CreateTextField(FormType formType)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formType) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTextField(FormType formType, string name, bool? required, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.CreateTextField(formType, name, required.Value, defaultValue, index);
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
        public ActionResult EditTextField(int fieldId)
        {
            FieldInfo field = WebHelper.FormService.GetField(fieldId);
            StringFieldConfigInfo config = field.ConfigInfo as StringFieldConfigInfo;
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new StringFieldEditModel(field, config));
            return View();
        }

        [HttpPost]
        public ActionResult EditTextField(FormType formType, int fieldId, string name, bool? required, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.ModifyTextField(fieldId, name, required.Value, defaultValue, index);
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
        public ActionResult CreateDropdownListField(FormType formType)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formType) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateDropdownListField(FormType formType, string name, bool? required, string selectList, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                selectList = selectList.Replace("，", ",");
                if (!required.HasValue)
                {
                    required = false;
                }
                
                WebHelper.FormService.CreateDropdownField(formType, name, required.Value, defaultValue, selectList.Split(',').ToList(), index);
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
        public ActionResult EditDropdownListField(int fieldId)
        {
            FieldInfo field = WebHelper.FormService.GetField(fieldId);
            ListFieldConfigInfo config = field.ConfigInfo as ListFieldConfigInfo;
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new ListFieldEditModel(field, config));
            return View();
        }

        [HttpPost]
        public ActionResult EditDropdownListField(int fieldId, string name, bool? required, string selectList, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                selectList = selectList.Replace("，", ",");
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.ModifyDropdownField(fieldId, name, required.Value, defaultValue, selectList.Split(',').ToList(), index);
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
        public ActionResult CreateRadioboxListField(FormType formType)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formType) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateRadioboxListField(FormType formType, string name, bool? required, string selectList, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                selectList = selectList.Replace("，", ",");
                if (!required.HasValue)
                {
                    required = false;
                }

                WebHelper.FormService.CreateRadioListField(formType, name, required.Value, defaultValue, selectList.Split(',').ToList(), index);
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
        public ActionResult EditRadioboxListField(int fieldId)
        {
            FieldInfo field = WebHelper.FormService.GetField(fieldId);
            ListFieldConfigInfo config = field.ConfigInfo as ListFieldConfigInfo;
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new ListFieldEditModel(field, config));
            return View();
        }

        [HttpPost]
        public ActionResult EditRadioboxListField(int fieldId, string name, bool? required, string selectList, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                selectList = selectList.Replace("，", ",");
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.ModifyRadioListField(fieldId, name, required.Value, defaultValue, selectList.Split(',').ToList(), index);
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
        public ActionResult CreateCheckboxListField(FormType formType)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formType) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateCheckboxListField(FormType formType, string name, bool? required, string selectList, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                selectList = selectList.Replace("，", ",");
                if (!required.HasValue)
                {
                    required = false;
                }
                List<string> defaultValues = new List<string>();
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    defaultValue = defaultValue.Replace("，", ",");
                    defaultValues = defaultValue.Split(',').ToList();
                }
                WebHelper.FormService.CreateCheckboxListField(formType, name, required.Value, defaultValues, selectList.Split(',').ToList(), index);
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
        public ActionResult EditCheckboxListField(int fieldId)
        {
            FieldInfo field = WebHelper.FormService.GetField(fieldId);
            CheckboxFieldConfigInfo config = field.ConfigInfo as CheckboxFieldConfigInfo;
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new ListFieldEditModel(field, config));
            return View();
        }

        [HttpPost]
        public ActionResult EditCheckboxListField(int fieldId, string name, bool? required, string selectList, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                selectList = selectList.Replace("，", ",");
                if (!required.HasValue)
                {
                    required = false;
                }
                List<string> defaultValues = new List<string>();
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    defaultValue = defaultValue.Replace("，", ",");
                    defaultValues = defaultValue.Split(',').ToList();
                }
                WebHelper.FormService.ModifyCheckboxListField(fieldId, name, required.Value, defaultValues, selectList.Split(',').ToList(), index);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateTextField()
        {
            return View();
        }

    }
}
