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
            return View();
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

        public ActionResult SousuoYonghu(string bumenId, string keyword)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    IList<UserInfo> users = WebHelper.UserService.GetUsersInPosition(bumenId);
                    resultModel.data = users.Select(x => new UserGridModel(x));
                }
                else
                {
                    IList<UserInfo> users = WebHelper.UserService.SearchUser(keyword);
                    resultModel.data = users.Select(x => new UserGridModel(x));
                }
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
