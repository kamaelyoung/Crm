using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Coldew.Api.Workflow;
using System.IO;
using System.Drawing.Imaging;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class LiuchengMoban
    {
        public LiuchengMoban(string id, string code, string mingcheng, string transferUrl, string shuoming, LiuchengYinqing yingqing, ColdewObject cobject)
        {
            this.ID = id;
            this.Mingcheng = mingcheng;
            this.TransferUrl = transferUrl;
            this.Shuoming = shuoming;
            this.Yingqing = yingqing;
            this.ColdewObject = cobject;
        }

        public ColdewObject ColdewObject { private set; get; }

        public LiuchengYinqing Yingqing { private set; get; }

        public string ID { protected set; get; }

        public string Code { protected set; get; }

        public string Mingcheng { protected set; get; }

        public string TransferUrl { protected set; get; }

        public string Shuoming { protected set; get; }

        public bool NengFaqi(User yong)
        {
            return true;
        }

        public LiuchengMobanXinxi Map()
        {
            return new LiuchengMobanXinxi
            {
                ID = this.ID,
                Mingcheng = this.Mingcheng,
                Code = this.Code,
                TransferUrl = this.TransferUrl,
                Shuoming = this.Shuoming
            };
        }
    }
}
