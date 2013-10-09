using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Website.Models;
using Crm.Api;
using Crm.Website.Models;
using Newtonsoft.Json;
using Coldew.Website;
using Coldew.Website.Controllers;

namespace Crm.Website.Controllers
{
    public class CrmSetupController : SetupController
    {

        [HttpGet]
        public ActionResult CustomerAreas()
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<CustomerAreaInfo> areaInfos = CrmWebHelper.CustomerAreaService.GetAllArea();
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

        [HttpPost]
        public ActionResult DeleteCustomerAreas(string areaIdsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                List<int> areaIds = JsonConvert.DeserializeObject<List<int>>(areaIdsJson);
                foreach (int areaId in areaIds)
                {
                    CrmWebHelper.CustomerAreaService.Delete(areaId);
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

        [HttpPost]
        public ActionResult CreateCustomerArea(string json)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                CustomerAreaCreateModel model = JsonConvert.DeserializeObject<CustomerAreaCreateModel>(json);
                CrmWebHelper.CustomerAreaService.Create(model.name, model.managerAccounts);
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
            CustomerAreaInfo areaInfo = CrmWebHelper.CustomerAreaService.GetAreaById(areaId);
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
                CrmWebHelper.CustomerAreaService.Modify(model.id, model.name, model.managerAccounts);
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
