using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Website.Models;
using Coldew.Api.Workflow;
using Coldew.Api.Organization;

namespace Coldew.Website.Controllers
{
    public class LiuchengGuanliController : BaseController
    {
        //
        // GET: /LiuchengGuanli/

        public ActionResult Index()
        {
            List<LiuchengMobanXinxi> liuchengModebanList = WebHelper.YinqingFuwu.GetSuoyouLiuchengMoban();
            UserInfo currentUser = WebHelper.CurrentUserInfo;
            this.ViewBag.liuchengModebanList = liuchengModebanList.Select(x => new LiuchengMobanModel(x, currentUser));
            return View();
        }

        public ActionResult LiuchengList(string liuchengMobanId, DateTime? faqiKaishiShijian, DateTime? faqiJieshuShijian,
            DateTime? jieshuKaishiShijian, DateTime? jieshuJieshuShijian, string zhaiyao, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (faqiJieshuShijian.HasValue)
                {
                    faqiJieshuShijian = faqiJieshuShijian.Value.AddHours(24);
                }
                if (jieshuJieshuShijian.HasValue)
                {
                    jieshuJieshuShijian = jieshuJieshuShijian.Value.AddHours(24);
                }
                ShijianFanwei faqiShijianFanwei = new ShijianFanwei(faqiKaishiShijian, faqiJieshuShijian);
                ShijianFanwei jieshuShijianFanwei = new ShijianFanwei(jieshuKaishiShijian, jieshuJieshuShijian);
                int count;
                List<LiuchengXinxi> liuchengList = WebHelper.LiuchengFuwu.GetLiuchengXinxiList(liuchengMobanId, faqiShijianFanwei, jieshuShijianFanwei, zhaiyao, start, size, out count);

                DatagridModel<LiuchengModel> gridModel = new DatagridModel<LiuchengModel>();
                gridModel.list = liuchengList.Select(x => new LiuchengModel(x, this, this.CurrentUser));
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

        public ActionResult RenwuList(string liuchengId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {

                var models = WebHelper.RenwuFuwu.GetLiuchengRenwu(liuchengId).Select(x => new RenwuModel(x, this, this.CurrentUser));
                resultModel.data = models;
            }
            catch (Exception ex)
            {
                WebHelper.Logger.Error(ex.Message, ex);
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult XiugaiRenChuliren(string renwuId, string chuliren)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                WebHelper.RenwuFuwu.XiugaiRenwuChuliren(renwuId, chuliren);
            }
            catch (Exception ex)
            {
                WebHelper.Logger.Error(ex.Message, ex);
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
    }
}
