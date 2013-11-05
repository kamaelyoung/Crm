using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api.Workflow;
using Coldew.Api.Organization;
using Coldew.Website.Controllers;

namespace Coldew.Website.Models
{
    public class LiuchengMobanModel
    {
        public LiuchengMobanModel(LiuchengMobanXinxi liucheng, UserInfo currentUser, WorkflowController controller)
        {
            this.id = liucheng.ID;
            this.mingcheng = liucheng.Mingcheng;
            string transferUrl = controller.Url.Content(liucheng.TransferUrl);
            string faqiUrl = "";
            if (transferUrl.IndexOf("?") > -1)
            {
                faqiUrl = string.Format("{0}&mobanId={1}", transferUrl, this.id);
            }
            else
            {
                faqiUrl = string.Format("{0}?mobanId={1}", transferUrl, this.id);
            }
            this.faqiLink = string.Format("<a target='_blank' href='{0}'>{1}</a>", faqiUrl, this.mingcheng);
            this.shuoming = liucheng.Shuoming;
        }


        public LiuchengMobanModel(LiuchengMobanXinxi liucheng, UserInfo currentUser)
        {
            this.id = liucheng.ID;
            this.mingcheng = liucheng.Mingcheng;
            this.shuoming = liucheng.Shuoming;
        }

        public string id;

        public string mingcheng;

        public string faqiLink;

        public string shuoming;
    }
}