using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api.Workflow;
using System.Web.Mvc;
using Coldew.Api.Organization;

namespace Coldew.Website.Models
{
    public class LiuchengModel
    {
        public LiuchengModel(LiuchengXinxi liucheng, Controller controller, UserInfo currentUser)
        {
            this.faqiren = liucheng.Faqiren.Name;
            this.faqiShijian = liucheng.FaqiShijian.ToString();
            this.id = liucheng.Id;
            this.guid = liucheng.Guid;
            if (liucheng.JieshuShijian.HasValue)
            {
                this.jieshuShijian = liucheng.JieshuShijian.Value.ToString();
            }
            this.mingcheng = liucheng.Mingcheng;
            this.zhuangtai = this.Map(liucheng.Zhuangtai);
            //this.url = string.Format("{0}?liuchengId={1}&uid={2}", liucheng.Liucheng.GuidangUrl, liucheng.Guid, currentUser.ID);
            this.zhaiyao = liucheng.Zhaiyao;
        }

        private string Map(LiuchengZhuangtai zhuangtai)
        {
            switch (zhuangtai)
            {
                case LiuchengZhuangtai.Chulizhong: return "处理中";
                case LiuchengZhuangtai.Wanchengle: return "已完成";
            }
            return "";
        }

        public int id;

        public string guid;

        public string mingcheng;

        public string faqiren;

        public string faqiShijian;

        public string jieshuShijian;

        public string zhuangtai;

        public string url;

        public string zhaiyao;
    }
}