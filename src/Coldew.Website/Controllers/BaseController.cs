using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Api.Organization;

namespace Coldew.Website.Controllers
{
    public class BaseController : Controller
    {
        private UserInfo _currentUser;
        protected UserInfo CurrentUser
        {
            get
            {
                if (this._currentUser == null)
                {
                    this._currentUser = WebHelper.CurrentUserInfo;
                }
                return this._currentUser;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.ViewBag.currentUserAccount = CurrentUser.Account;
            base.OnActionExecuting(filterContext);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new NewtonJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}
