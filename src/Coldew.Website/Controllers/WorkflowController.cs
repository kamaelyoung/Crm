using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Website.Models;
using Coldew.Api.Workflow;
using Newtonsoft.Json;

namespace Coldew.Website.Controllers
{
    public class WorkflowController : BaseController
    {
        //
        // GET: /Workflow/

        public ActionResult Daibande()
        {
            return View();
        }

        public ActionResult JianglaiZhipaiDialog()
        {
            return View();
        }

        public ActionResult DaibanRenwu(string liuchengMobanId, DateTime? kaishiShijian, DateTime? jieshuShijian, string zhaiyao, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (jieshuShijian.HasValue)
                {
                    jieshuShijian = jieshuShijian.Value.AddHours(24);
                }
                int count;
                List<RenwuXinxi> renwuList = WebHelper.RenwuFuwu.GetChulizhongdeRenwu(WebHelper.CurrentUserAccount, liuchengMobanId, kaishiShijian, jieshuShijian, zhaiyao, start, size, out count);

                DatagridModel<RenwuModel> gridModel = new DatagridModel<RenwuModel>();
                gridModel.list = renwuList.Select(x => new RenwuModel(x, this, this.CurrentUser));
                gridModel.count = count;
                resultModel.data = gridModel;
            }
            catch (Exception ex)
            {
                WebHelper.Logger.Error(ex.Message, ex);
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Zhipai(string argsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                RenwuZhipaiModel model = JsonConvert.DeserializeObject<RenwuZhipaiModel>(argsJson);
                WebHelper.RenwuFuwu.ZhipaiRenwu(model.renwuId, model.yonghuZhanghao);
            }
            catch (Exception ex)
            {
                WebHelper.Logger.Error(ex.Message, ex);
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ZhipaiSuoyou(string yonghuZhanghao)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                WebHelper.RenwuFuwu.ZhipaiSuoyouRenwu(WebHelper.CurrentUserAccount, yonghuZhanghao);
            }
            catch (Exception ex)
            {
                WebHelper.Logger.Error(ex.Message, ex);
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult JianglaiZhipai()
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                JianglaiZhipaiXinxi zhipai = WebHelper.RenwuFuwu.GetJianglaiZhipai(WebHelper.CurrentUserAccount);
                if (zhipai != null)
                {
                    resultModel.data = new JianglaiZhipaiModel(zhipai);
                }
            }
            catch (Exception ex)
            {
                WebHelper.Logger.Error(ex.Message, ex);
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult JianglaiZhipai(string dailirenZhanghao, DateTime? kaishiShijian, DateTime? jieshuShijian)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                WebHelper.RenwuFuwu.SetJianglaiZhipai(WebHelper.CurrentUserAccount, dailirenZhanghao, kaishiShijian, jieshuShijian);
            }
            catch (Exception ex)
            {
                WebHelper.Logger.Error(ex.Message, ex);
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Faqide()
        {
            return View();
        }

        public ActionResult FaqideRenwu(string liuchengMobanId, DateTime? kaishiShijian, DateTime? jieshuShijian, string zhaiyao, int start, int size)
        {

            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (jieshuShijian.HasValue)
                {
                    jieshuShijian = jieshuShijian.Value.AddHours(24);
                }
                int count;
                var models = WebHelper.RenwuFuwu.GetFaqideRenwu(WebHelper.CurrentUserAccount, liuchengMobanId, kaishiShijian, jieshuShijian, zhaiyao, start, size, out count)
                    .Select(x => new RenwuModel(x, this, this.CurrentUser));
                DatagridModel<RenwuModel> gridModel = new DatagridModel<RenwuModel>();
                gridModel.list = models;
                gridModel.count = count;
                resultModel.data = gridModel;
            }
            catch (Exception ex)
            {
                WebHelper.Logger.Error(ex.Message, ex);
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Chexiao(string argsJosn)
        {

            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                LiuchengChexiaoModel model = JsonConvert.DeserializeObject<LiuchengChexiaoModel>(argsJosn);
                WebHelper.LiuchengFuwu.Chexiao(model.liuchengId);
            }
            catch (Exception ex)
            {
                WebHelper.Logger.Error(ex.Message, ex);
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FaqiLiucheng()
        {
            return View();
        }

        public ActionResult Liucheng()
        {

            List<LiuchengMobanXinxi> list = WebHelper.YinqingFuwu.GetLiuchengMobanByYonghu(this.CurrentUser.Account);
            return Json(list.Select(x => new LiuchengMobanModel(x, this.CurrentUser)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Guidangde()
        {
            return View();
        }

        public ActionResult GuidangdeRenwu(string liuchengMobanId, DateTime? wanchengKaishiShijian, DateTime? wanchengJieshuShijian, string zhaiyao, int start, int size)
        {

            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (wanchengJieshuShijian.HasValue)
                {
                    wanchengJieshuShijian = wanchengJieshuShijian.Value.AddHours(24);
                }
                int count;
                var models = WebHelper.RenwuFuwu.GetGuidangdeRenwu(WebHelper.CurrentUserAccount, liuchengMobanId, wanchengKaishiShijian, wanchengJieshuShijian, zhaiyao, start, size, out count)
                    .Select(x => new RenwuModel(x, this, this.CurrentUser));
                DatagridModel<RenwuModel> gridModel = new DatagridModel<RenwuModel>();
                gridModel.list = models;
                gridModel.count = count;
                resultModel.data = gridModel;
            }
            catch (Exception ex)
            {
                WebHelper.Logger.Error(ex.Message, ex);
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Yibande()
        {
            return View();
        }

        public ActionResult YibandeRenwu(string liuchengMobanId, DateTime? wanchengKaishiShijian, DateTime? wanchengJieshuShijian, string zhaiyao, int start, int size)
        {

            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (wanchengJieshuShijian.HasValue)
                {
                    wanchengJieshuShijian = wanchengJieshuShijian.Value.AddHours(24);
                }
                int count;
                var models = WebHelper.RenwuFuwu.GetWanchengdeRenwu(WebHelper.CurrentUserAccount, liuchengMobanId, wanchengKaishiShijian, wanchengJieshuShijian, zhaiyao, start, size, out count)
                    .Select(x => new RenwuModel(x, this, this.CurrentUser));
                DatagridModel<RenwuModel> gridModel = new DatagridModel<RenwuModel>();
                gridModel.list = models;
                gridModel.count = count;
                resultModel.data = gridModel;
            }
            catch (Exception ex)
            {
                WebHelper.Logger.Error(ex.Message, ex);
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Zhipaide()
        {
            return View();
        }

        public ActionResult ZhipaideRenwu(int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                int count;
                List<RenwuXinxi> xingdongList = WebHelper.RenwuFuwu.GetZhipaideRenwu(WebHelper.CurrentUserAccount, start, size, out count);

                DatagridModel<RenwuModel> gridModel = new DatagridModel<RenwuModel>();
                gridModel.list = xingdongList.Select(x => new RenwuModel(x, this, this.CurrentUser));
                gridModel.count = count;
                resultModel.data = gridModel;
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult QuxiaoZhipai(string argsJson)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                QuxiaoZhipaiModel model = JsonConvert.DeserializeObject<QuxiaoZhipaiModel>(argsJson);
                WebHelper.RenwuFuwu.QuxiaoZhipai(model.renwuId);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult Liuchengtu(string liuchengId)
        {
            byte[] bytes = WebHelper.LiuchengFuwu.GetPngLiuchengtu(liuchengId);
            FileContentResult result = new FileContentResult(bytes, "image/png");
            return result;
        }
    }
}
