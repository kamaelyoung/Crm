using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Website.Models;

namespace Coldew.Website.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignIn(string account, string password, bool remember, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = this.Url.Action("Index", "Customer");
            }
            ControllerResultModel resultModel = new ControllerResultModel();
            try
            {
                string token = WebHelper.AuthenticationService.SignIn(account, password, Request.UserHostAddress);
                resultModel.data = this.Url.Action("Redirect", new {url = returnUrl, token = token, remember = remember});
            }
            catch (Exception ex)
            {
                resultModel.result = ControllerResult.Error;
                resultModel.message = ex.Message;
                WebHelper.Logger.Error(ex.Message, ex);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignOut()
        {
            WebHelper.AuthenticationService.SignOut(WebHelper.CurrentUserToken);
            return this.RedirectToAction("Index");
        }

        public ActionResult Redirect(string url, string token, bool remember)
        {
            WebHelper.SetCurrentUserToken(token, remember);
            return this.Redirect(url);
        }
    }
}
