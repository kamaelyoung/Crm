using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crm.Website.Models;
using Newtonsoft.Json;
using Crm.Api.Organization;

namespace Crm.Website.Controllers
{
    public class OrgController : BaseController
    {
        //
        // GET: /Org/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TopPosition()
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                PositionInfo position = WebHelper.PositionService.GetTopPosition();
                resultModel.data = new PositionTreeModel(position);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Positions(string parentId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                IList<PositionInfo> positions = WebHelper.PositionService.GetChildPositions(parentId);
                resultModel.data = positions.Select(x => new PositionTreeModel(x));
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreatePosition(string parentId, string name)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                PositionInfo positionInfo = WebHelper.PositionService.Create(WebHelper.CurrentUserInfo.ID, new PositionCreateInfo { Name = name, ParentId = parentId});
                resultModel.data = new PositionTreeModel(positionInfo);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPosition(string id, string name)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                PositionInfo positionInfo = WebHelper.PositionService.GetPositionById(id);
                PositionChangeInfo changeInfo = new PositionChangeInfo(positionInfo); 
                changeInfo.Name = name;
                WebHelper.PositionService.ChangePositionInfo(WebHelper.CurrentUserInfo.ID, changeInfo);

                positionInfo = WebHelper.PositionService.GetPositionById(id);
                resultModel.data = new PositionTreeModel(positionInfo);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePosition(string positionId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                WebHelper.PositionService.DeletePositionById(WebHelper.CurrentUserInfo.ID, positionId);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateUser(string account, string name, string positionId, string password, string email)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                UserCreateInfo createInfo = new UserCreateInfo();
                createInfo.Account = account;
                createInfo.Name = name;
                createInfo.Password = password;
                createInfo.MainPositionId = positionId;
                createInfo.Email = email;
                WebHelper.UserService.Create(WebHelper.CurrentUserInfo.ID, createInfo);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUser(string id, string name, string email)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                UserInfo userInfo = WebHelper.UserService.GetUserById(id);
                UserChangeInfo changeInfo = new UserChangeInfo(userInfo);
                changeInfo.Name = name;
                changeInfo.Email = email;
                WebHelper.UserService.ChangeInfo(WebHelper.CurrentUserInfo.ID, changeInfo);
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MoveUser(string userIds, string positionId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                foreach (string userId in userIds.Split(','))
                {
                    UserInfo userInfo = WebHelper.UserService.GetUserById(userId);
                    WebHelper.PositionService.AddUserToPosition(WebHelper.CurrentUserInfo.ID, new UserPositionInfo { Main = true, PositionId = positionId, UserId = userId });
                    WebHelper.PositionService.RemoveUserFromPosition(WebHelper.CurrentUserInfo.ID, userInfo.MainPositionId, userId);
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

        public ActionResult LockUser(string userIds)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                foreach (string userId in userIds.Split(','))
                {
                    UserInfo userInfo = WebHelper.UserService.GetUserById(userId);
                    WebHelper.UserService.Lock(WebHelper.CurrentUserInfo.ID, userId);
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

        public ActionResult ActivateUser(string userIds)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                foreach (string userId in userIds.Split(','))
                {
                    UserInfo userInfo = WebHelper.UserService.GetUserById(userId);
                    WebHelper.UserService.Activate(WebHelper.CurrentUserInfo.ID, userId);
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

        public ActionResult ResetPassword(string userIds, string password)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                foreach(string userId in userIds.Split(','))
                {
                    UserInfo userInfo = WebHelper.UserService.GetUserById(userId);
                    WebHelper.UserService.ResetPassword(WebHelper.CurrentUserInfo.ID, userId, password);
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

        public ActionResult Users(string positionId)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                IList<UserInfo> users = WebHelper.UserService.GetUsersInPosition(positionId);
                resultModel.data = users.Select(x => new UserGridModel(x));
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchUsers(string keyword)
        {
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                IList<UserInfo> users = WebHelper.UserService.SearchUser(keyword);
                resultModel.data = users.Select(x => new UserGridModel(x));
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
