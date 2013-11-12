using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Website.Models;
using Coldew.Website;
using Coldew.Api.Organization;

namespace Coldew.Website.Controllers
{
    public class ChengyuanController : Controller
    {
        //
        // GET: /ZuzhiApi/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dialog()
        {
            return this.PartialView();
        }

        public ActionResult DingjiBumen()
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                PositionTreeModel model = new PositionTreeModel(WebHelper.PositionService.GetTopPosition());
                resultModel.data = model;
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult XiajiBumen(string bumenId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                resultModel.data = WebHelper.PositionService.GetChildPositions(bumenId).Select(x => new PositionTreeModel(x)).ToList();
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SousuoYonghu(string bumenId, string zhanghaoHuoXingming, bool? baohanXiajiBumen, int start, int size)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                UserFilterInfo filterInfo = new UserFilterInfo();
                filterInfo.AccountOrName = zhanghaoHuoXingming;
                filterInfo.OrganizationId = bumenId;
                filterInfo.OrganizationType = OrganizationType.Position;
                if (baohanXiajiBumen.HasValue)
                {
                    filterInfo.Recursive = baohanXiajiBumen.Value;
                }
                IList<UserInfo> modelList = WebHelper.UserService.SearchUser(filterInfo);
                var userModels = modelList.Skip(start).Take(size).Select(x => new UserGridModel(x)).ToList();
                DatagridModel model = new DatagridModel { list = userModels, count = modelList.Count };
                resultModel.data = model;
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

    }
}
