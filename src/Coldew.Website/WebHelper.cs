using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Coldew.Api.Organization;
using Coldew.Api;
using System.Text;
using Coldew.Api.UI;

namespace Coldew.Website
{
    public class WebHelper
    {
        static WebHelper()
        {
            Spring.Context.IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            FormService = (IFormService)ctx["FormService"];
            MetadataService = (IMetadataService)ctx["MetadataService"];
            ColdewConfigService = (IColdewConfigService)ctx["ColdewConfigService"];
            ColdewObjectService = (IColdewObjectService)ctx["ColdewObjectService"];
            UserService = (IUserService)ctx["UserService"];
            PositionService = (IPositionService)ctx["PositionService"];
            AuthenticationService = (IAuthenticationService)ctx["AuthenticationService"];
            GridViewService = (IGridViewService)ctx["GridViewService"];
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger("logger");
            ColdewInputFactory = (ColdewInputFactory)ctx["ColdewInputFactory"];
        }

        public static ColdewInputFactory ColdewInputFactory { set; get; }

        public static ILog Logger { private set; get; }

        public static IUserService UserService { private set; get; }

        public static IPositionService PositionService { private set; get; }

        public static IAuthenticationService AuthenticationService { private set; get; }

        public static IMetadataService MetadataService { private set; get; }

        public static IColdewConfigService ColdewConfigService { private set; get; }

        public static IColdewObjectService ColdewObjectService { private set; get; }

        public static IGridViewService GridViewService { private set; get; }

        public static IFormService FormService { private set; get; }

        public static UserInfo CurrentUserInfo
        {
            get
            {
                UserInfo userInfo = AuthenticationService.GetAuthenticatedUser(CurrentUserToken);
                if (userInfo == null)
                {
                    HttpContext.Current.Response.Redirect("~/Login?returnUrl=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.ToString()));
                }
                return userInfo;
            }
        }

        public static string CurrentUserAccount
        {
            get
            {
                return CurrentUserInfo.Account;
            }
        }

        public static string CurrentUserToken
        {
            get
            {
                string token = HttpContext.Current.Request["token"];

                return token;
            }
        }

        public static void SetCurrentUserToken(string token ,bool remember)
        {
            HttpCookie tokenCookie = null;
            if (HttpContext.Current.Response.Cookies["token"] != null)
            {
                tokenCookie = HttpContext.Current.Response.Cookies["token"];
                tokenCookie.Value = token;
            }
            else
            {
                tokenCookie = new HttpCookie("token", token);
            }
            if (remember)
            {
                tokenCookie.Expires = DateTime.Now.AddYears(1);
            }
            HttpContext.Current.Response.Cookies.Add(tokenCookie);
        }

        public static bool IsAdmin
        {
            get
            {
                return CurrentUserInfo.Role == UserRole.Administrator;
            }
        }

        public static string UsersCheckboxList(string name, bool selectCurrentUser, bool requried)
        {
            string requriedAttr = "";
            if (requried)
            {
                requriedAttr = "data-required";
            }
            IList<UserInfo> users = UserService.GetAllNormalUser().ToList();
            StringBuilder sb = new StringBuilder();
            foreach (UserInfo user in users)
            {
                if (user.Account == CurrentUserAccount)
                {
                    sb.AppendFormat("<label class='checkbox'><input type='checkbox' name='{0}' checked='checked' {3} value='{1}' />{2}</label>", name, user.Account, user.Name, requriedAttr);
                }
                else
                {
                    sb.AppendFormat("<label class='checkbox'><input type='checkbox' name='{0}' {3} value='{1}' />{2}</label>", name, user.Account, user.Name, requriedAttr);
                }
            }
            return sb.ToString();
        }

        public static List<ColdewObjectInfo> ColdewObjects
        {
            get
            {
                return ColdewObjectService.GetForms();
            }
        }
    }
}