using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Website.Models;
using Coldew.Api;
using Newtonsoft.Json;

namespace Coldew.Website.Controllers
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

        public ActionResult CreateField()
        {
            return View();
        }

        public ActionResult Extend(string formId)
        {
            FormInfo formInfo = WebHelper.FormService.GetFormById(formId);
            this.ViewBag.formId = formId;
            this.ViewBag.Title = formInfo.Name + "扩展";

            return View();
        }

        public ActionResult GetFields(string formId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<FieldInfo> fields = WebHelper.FormService.GetFields(formId).Where(x => !x.CanModify).ToList();
                resultModel.data = fields.Select(x => new FieldGridModel(x, this, formId));
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
        public ActionResult CreateStringField(string formId)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formId) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateStringField(string formId, string name, bool? required, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.CreateStringField(formId, name, required.Value, defaultValue, index);
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
        public ActionResult CreateDateField(string formId)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formId) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateDateField(string formId, string name, bool? required, bool defaultValueIsToday, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.CreateDateField(formId, name, required.Value, defaultValueIsToday, index);
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
        public ActionResult CreateNumberField(string formId)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formId) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateNumberField(string formId, string name, bool? required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.CreateNumberField(formId, name, required.Value, defaultValue, max, min, precision, index);
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
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new StringFieldEditModel(field as StringFieldInfo));
            return View();
        }

        [HttpPost]
        public ActionResult EditStringField(string formId, int fieldId, string name, bool? required, string defaultValue, int index)
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
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new DateFieldEditModel(field as DateFieldInfo));
            return View();
        }

        [HttpPost]
        public ActionResult EditDateField(string formId, int fieldId, string name, bool? required, bool defaultValueIsToday, int index)
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
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new NumberFieldEditModel(field as NumberFieldInfo));
            return View();
        }

        [HttpPost]
        public ActionResult EditNumberField(string formId, int fieldId, string name, bool? required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index)
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
        public ActionResult CreateTextField(string formId)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formId) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTextField(string formId, string name, bool? required, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (!required.HasValue)
                {
                    required = false;
                }
                WebHelper.FormService.CreateTextField(formId, name, required.Value, defaultValue, index);
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
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new StringFieldEditModel(field as StringFieldInfo));
            return View();
        }

        [HttpPost]
        public ActionResult EditTextField(string formId, int fieldId, string name, bool? required, string defaultValue, int index)
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
        public ActionResult CreateDropdownListField(string formId)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formId) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateDropdownListField(string formId, string name, bool? required, string selectList, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                selectList = selectList.Replace("，", ",");
                if (!required.HasValue)
                {
                    required = false;
                }
                
                WebHelper.FormService.CreateDropdownField(formId, name, required.Value, defaultValue, selectList.Split(',').ToList(), index);
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
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new ListFieldEditModel(field as ListFieldInfo));
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
        public ActionResult CreateRadioboxListField(string formId)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formId) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateRadioboxListField(string formId, string name, bool? required, string selectList, string defaultValue, int index)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                selectList = selectList.Replace("，", ",");
                if (!required.HasValue)
                {
                    required = false;
                }

                WebHelper.FormService.CreateRadioListField(formId, name, required.Value, defaultValue, selectList.Split(',').ToList(), index);
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
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new ListFieldEditModel(field as ListFieldInfo));
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
        public ActionResult CreateCheckboxListField(string formId)
        {
            this.ViewBag.FieldIndex = WebHelper.FormService.GetFieldMaxIndex(formId) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult CreateCheckboxListField(string formId, string name, bool? required, string selectList, string defaultValue, int index)
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
                WebHelper.FormService.CreateCheckboxListField(formId, name, required.Value, defaultValues, selectList.Split(',').ToList(), index);
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
            this.ViewBag.modelJson = JsonConvert.SerializeObject(new ListFieldEditModel(field as CheckboxFieldInfo));
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
