using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api.Workflow;
using Coldew.Api.Organization;

namespace Coldew.Website.Models
{
    public class LiuchengMobanModel
    {
        public LiuchengMobanModel(LiuchengMobanXinxi liucheng, UserInfo currentUser)
        {
            this.id = liucheng.Id;
            this.guid = liucheng.Guid;
            this.mingcheng = liucheng.Mingcheng;
            this.faqiUrl = string.Format(string.Format("{0}?id={1}&uid={2}", liucheng.FaqiUrl, liucheng.Guid, currentUser.ID));
            this.shuoming = liucheng.Shuoming;
        }

        public int id;

        public string guid;

        public string mingcheng;

        public string faqiUrl;

        public string shuoming;
    }
}