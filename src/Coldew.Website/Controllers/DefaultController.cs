using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Api;

namespace Coldew.Website.Controllers
{
    public class DefaultController : Controller
    {
        //
        // GET: /Default/

        public ActionResult Index()
        {
#if DEBUG
            string token = WebHelper.AuthenticationService.SignIn("admin", "123456", "1270.0.0.1");
            WebHelper.SetCurrentUserToken(token, false);
            List<ColdewObjectInfo> forms = WebHelper.ColdewObjectService.GetForms();
            List<GridViewInfo> views = WebHelper.GridViewService.GetGridViews(forms[0].ID, "admin");
            return this.RedirectToAction("Index", "Metadata", new { objectId = forms[0].ID, viewId = views[0].ID });
#else
            return this.RedirectToAction("Index", "Login");
#endif


        }

    }
}
