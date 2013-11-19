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
            if (renwu == null)
            {
                this.ViewBag.error = "找不到该任务，或者该任务已经被取消！";
                return View("Error");
            }

            if (renwu.Zhuangtai == RenwuZhuangtai.Wanchengle)
            {
                return this.RedirectToAction("Mingxi", new { renwuId = renwuId, liuchengId = liuchengId });
            }
            else if (renwu.Bianhao == "shenhe")
            {
                return this.RedirectToAction("Shenhe", new { renwuId = renwuId, liuchengId = liuchengId });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Faqi()
        {
            ColdewObjectInfo objectInfo = WebHelper.ColdewObjectService.GetFormByCode(this.CurrentUser.Account, "FahuoLiucheng");
            this.ViewBag.objectInfo = objectInfo;

            return View();
        }

        [HttpPost]
        public ActionResult Faqi(string mobanId, string biaodanJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                ColdewObjectInfo objectInfo = WebHelper.ColdewObjectService.GetFormByCode(this.CurrentUser.Account, "FahuoLiucheng");

                MetadataInfo biaodan = WebHelper.MetadataService.Create(objectInfo.ID, this.CurrentUser.Account, biaodanJson);

                LiuchengXinxi liucheng = WebHelper.LiuchengFuwu.FaqiLiucheng(mobanId, "yewuyuan", "业务员", "", this.CurrentUser.Account, false, "", biaodan.ID);
                WebHelper.RenwuFuwu.ChuangjianXingdong(liucheng.Guid, "shenhe", "审核", new List<string> { "admin", "user1" }, "", null);

                JObject modifyObject = new JObject();
                modifyObject.Add("liuchengId", liucheng.Guid);
                WebHelper.MetadataService.Modify(objectInfo.ID, this.CurrentUser.Account, biaodan.ID, JsonConvert.SerializeObject(modifyObject));
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
        public ActionResult Shenhe(string renwuId, string liuchengId)
        {
            ColdewObjectInfo objectInfo = WebHelper.ColdewObjectService.GetFormByCode(this.CurrentUser.Account, "FahuoLiucheng");
            this.ViewBag.objectInfo = objectInfo;

            LiuchengXinxi liucheng = WebHelper.LiuchengFuwu.GetLiucheng(liuchengId);
            List<RenwuXinxi> renwuXinxi = WebHelper.RenwuFuwu.GetLiuchengRenwu(liuchengId);
            this.ViewBag.renwuModelsJson = JsonConvert.SerializeObject(renwuXinxi.Select(x => new RenwuModel(x, this, this.CurrentUser)).ToList());

            MetadataInfo biaodan = WebHelper.MetadataService.GetMetadataById(this.CurrentUser.Account, objectInfo.ID, liucheng.BiaodanId);
            this.ViewBag.biaodan = biaodan;

            MetadataEditModel model = new MetadataEditModel(biaodan);
            this.ViewBag.biaodanJson = JsonConvert.SerializeObject(model);

            return View();
        }

        [HttpPost]
        public ActionResult Shenhe(string renwuId, string liuchengId, string wanchengShuoming)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                LiuchengXinxi liucheng = WebHelper.LiuchengFuwu.GetLiucheng(liuchengId);

                ColdewObjectInfo objectInfo = WebHelper.ColdewObjectService.GetFormByCode(this.CurrentUser.Account, "FahuoLiucheng");

                RenwuXinxi renwuXinxi = WebHelper.RenwuFuwu.GetRenwu(liuchengId, renwuId);
                WebHelper.RenwuFuwu.WanchengRenwu(liuchengId, this.CurrentUser.Account, renwuId, wanchengShuoming);

                WebHelper.RenwuFuwu.WanchengXingdong(liuchengId, renwuXinxi.Xingdong.Guid);
                WebHelper.LiuchengFuwu.Wancheng(liuchengId);

                ColdewObjectInfo dingdanZhongbiaoObject = WebHelper.ColdewObjectService.GetFormByCode(this.CurrentUser.Account, "dingdanZhongbiao");
                MetadataInfo biaodan = WebHelper.MetadataService.GetMetadataById(this.CurrentUser.Account, objectInfo.ID, liucheng.BiaodanId);
                JArray chanpinList = JsonConvert.DeserializeObject<JArray>(biaodan.GetProperty("chanpinList").EditValue);
                foreach (JObject chanpin in chanpinList)
                {
                    JObject dingdanPropertys = new JObject();
                    dingdanPropertys.Add("yewuyuan", liucheng.Faqiren.Account);

                    this.AddPropertyToJObject(dingdanPropertys, biaodan.GetProperty("shengfen"));
                    this.AddPropertyToJObject(dingdanPropertys, biaodan.GetProperty("diqu"));
                    this.AddPropertyToJObject(dingdanPropertys, biaodan.GetProperty("fahuoRiqi"));
                    this.AddPropertyToJObject(dingdanPropertys, biaodan.GetProperty("huikuanRiqi"));
                    this.AddPropertyToJObject(dingdanPropertys, biaodan.GetProperty("huikuanJine"));
                    this.AddPropertyToJObject(dingdanPropertys, biaodan.GetProperty("huikuanLeixing"));
                    this.AddPropertyToJObject(dingdanPropertys, biaodan.GetProperty("huikuanDanwei"));
                    this.AddPropertyToJObject(dingdanPropertys, biaodan.GetProperty("daokuanDanwei"));
                    this.AddPropertyToJObject(dingdanPropertys, biaodan.GetProperty("kaipiaoDanwei"));
                    this.AddPropertyToJObject(dingdanPropertys, biaodan.GetProperty("shouhuoDizhi"));
                    this.AddPropertyToJObject(dingdanPropertys, biaodan.GetProperty("shouhuoren"));

                    foreach (JProperty property in chanpin.Properties())
                    {
                        dingdanPropertys.Add(property.Name, property.Value.ToString());
                    }
                    WebHelper.MetadataService.Create(dingdanZhongbiaoObject.ID, this.CurrentUser.Account, JsonConvert.SerializeObject(dingdanPropertys));
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

        private void AddPropertyToJObject(JObject jobject, PropertyInfo property)
        {
            jobject.Add(property.Code, property.EditValue);
        }


        [HttpGet]
        public ActionResult Mingxi(string liuchengId)
        {
            ColdewObjectInfo objectInfo = WebHelper.ColdewObjectService.GetFormByCode(this.CurrentUser.Account, "FahuoLiucheng");
            this.ViewBag.objectInfo = objectInfo;

            LiuchengXinxi liucheng = WebHelper.LiuchengFuwu.GetLiucheng(liuchengId);

            List<RenwuXinxi> renwuXinxi = WebHelper.RenwuFuwu.GetLiuchengRenwu(liuchengId);
            this.ViewBag.renwuModelsJson = JsonConvert.SerializeObject(renwuXinxi.Select(x => new RenwuModel(x, this, this.CurrentUser)).ToList());

            MetadataInfo biaodan = WebHelper.MetadataService.GetMetadataById(this.CurrentUser.Account, objectInfo.ID, liucheng.BiaodanId);
            MetadataEditModel model = new MetadataEditModel(biaodan);
            this.ViewBag.biaodanJson = JsonConvert.SerializeObject(model);

            return View(biaodan);
        }

        [HttpGet]
        public ActionResult Details(string metadataId, string objectId)
        {
            MetadataInfo metadataInfo = WebHelper.MetadataService.GetMetadataById(this.CurrentUser.Account, objectId, metadataId);
            string liuchengId = metadataInfo.GetProperty("liuchengId").EditValue;
            return this.RedirectToAction("Mingxi", new { liuchengId = liuchengId });
        }
    }
}
