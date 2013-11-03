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
            this.faqiUrl = controller.Url.Action("Faqi", new { objectCode = liucheng.ObjectCode, formCode = liucheng.FaqiFormCode });
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

        public string faqiUrl;

        public string shuoming;
    }
}