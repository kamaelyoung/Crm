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
            string token = WebHelper.AuthenticationService.SignIn("admin", "123456", "1270.0.0.1");
            WebHelper.SetCurrentUserToken(token, false);
            List<FormInfo> forms = WebHelper.FormService.GetForms();
            List<GridViewInfo> views = WebHelper.GridViewService.GetGridViews(forms[0].ID, "admin");
            return this.RedirectToAction("Index", "Metadata", new { formId = forms[0].ID, viewId = views[0].ID});
        }

    }
}
