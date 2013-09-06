using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crm.Website.Controllers
{
    public class DefaultController : Controller
    {
        //
        // GET: /Default/

        public ActionResult Index()
        {
            string token = WebHelper.AuthenticationService.SignIn("admin", "123456", "1270.0.0.1");
            WebHelper.SetCurrentUserToken(token, false);
            return this.RedirectToAction("Index", "Customer");
        }

    }
}
