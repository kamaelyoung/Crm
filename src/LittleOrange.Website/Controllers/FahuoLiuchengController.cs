using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Website.Models;
using Newtonsoft.Json;
using Coldew.Website.Controllers;
using Coldew.Website;
using Coldew.Api;
using Newtonsoft.Json.Linq;
using Coldew.Api.Workflow;
using Crm.Website.Models;

namespace LittleOrange.Website.Controllers
{
    public class FahuoLiuchengController : BaseController
    {
        //
        // GET: /FahuoLiucheng/

        public ActionResult Index(string renwuId, string liuchengId, string mobanId)
        {
            if (string.IsNullOrEmpty(renwuId))
            {
                return this.RedirectToAction("Faqi", new { mobanId = mobanId });
            }
            RenwuXinxi renwu = WebHelper.RenwuFuwu.GetRenwu(liuchengId, renwuId);
            if (renwu.Zhuangtai == RenwuZhuangtai.Wanchengle)
            {
                return this.RedirectToAction("Mingxi", new { renwuId = renwuId, liuchengId = liuchengId });
            }
            else if (renwu.Bianhao == "caiwu_shenhe")
            {
                return this.RedirectToAction("Caiwu", new { renwuId = renwuId, liuchengId = liuchengId });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Faqi()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Faqi(string mobanId, string biaodanJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                ColdewObjectInfo objectInfo = WebHelper.ColdewObjectService.GetFormByCode("FahuoLiucheng");
                JObject biandanInfo = JsonConvert.DeserializeObject<JObject>(biaodanJson);

                PropertySettingDictionary propertys = ExtendHelper.MapPropertySettingDictionary(biandanInfo);
                propertys.Add("name", "发货流程"+ DateTime.Now.ToString());
                MetadataInfo biaodan = WebHelper.MetadataService.Create(objectInfo.ID, this.CurrentUser.Account, propertys);

                LiuchengXinxi liucheng = WebHelper.LiuchengFuwu.FaqiLiucheng(mobanId, "caiwu_faqi", "发起", "", this.CurrentUser.Account, false, "", biaodan.ID);
                WebHelper.RenwuFuwu.ChuangjianXingdong(liucheng.Guid, "caiwu_shenhe", "财务审核", new List<string> { "admin" }, "", null);
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
        public ActionResult Caiwu(string renwuId, string liuchengId)
        {
            LiuchengXinxi liucheng = WebHelper.LiuchengFuwu.GetLiucheng(liuchengId);
            MetadataEditModel model = new MetadataEditModel(liucheng.Biaodan);
            this.ViewBag.biaodanJson = JsonConvert.SerializeObject(model);

            return View();
        }

        [HttpPost]
        public ActionResult Caiwu(string renwuId, string liuchengId, string biaodanJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                LiuchengXinxi liucheng = WebHelper.LiuchengFuwu.GetLiucheng(liuchengId);

                ColdewObjectInfo objectInfo = WebHelper.ColdewObjectService.GetFormByCode("FahuoLiucheng");
                JObject biandanInfo = JsonConvert.DeserializeObject<JObject>(biaodanJson);

                PropertySettingDictionary propertys = ExtendHelper.MapPropertySettingDictionary(biandanInfo);
                WebHelper.MetadataService.Modify(objectInfo.ID, this.CurrentUser.Account, liucheng.Biaodan.ID, propertys);

                WebHelper.RenwuFuwu.WanchengRenwu(liuchengId, this.CurrentUser.Account, renwuId, "");
                WebHelper.LiuchengFuwu.Wancheng(liuchengId);
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
        public ActionResult Mingxi(string renwuId, string liuchengId)
        {
            LiuchengXinxi liucheng = WebHelper.LiuchengFuwu.GetLiucheng(liuchengId);

            return View(liucheng.Biaodan);
        }

        [HttpGet]
        public ActionResult Details(string metadataId, string objectId)
        {
            MetadataInfo metadataInfo = WebHelper.MetadataService.GetMetadataById(objectId, metadataId);

            return View("Mingxi", metadataInfo);
        }
    }
}
