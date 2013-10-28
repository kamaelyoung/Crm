using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Coldew.Api.Workflow;
using NHibernate.Criterion;
using log4net;
using System.IO;
using System.Drawing;
using System.Collections;
using Coldew.Api.Workflow.Exceptions;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public abstract class Yinqing
    {
        List<LiuchengMoban> _liuchengList;
        Dictionary<int, LiuchengMoban> _liuchengDictionary;
        ColdewManager _coldewManager;

        public Yinqing(ColdewManager coldewManager)
        {
            this.Logger = coldewManager.Logger;
            this._coldewManager = coldewManager;

            this._liuchengDictionary = new Dictionary<int, LiuchengMoban>();
            this._liuchengList = new List<LiuchengMoban>();
            this._liuchengList = this.JiazaiLiuchengMoban();
            if (this._liuchengList == null)
            {
                throw new LiuchengMobanJiazaiNullException();
            }
            foreach (LiuchengMoban liucheng in this._liuchengList)
            {
                if (this._liuchengDictionary.ContainsKey(liucheng.Id))
                {
                    throw new LiuchengMobanIdChongfuException();
                }
                this._liuchengDictionary.Add(liucheng.Id, liucheng);
                liucheng.Yinqing = this;
                liucheng.Jiazai();
            }

            this.JianglaiZhipaiManager = new Workflow.JianglaiZhipaiManager(this._coldewManager.OrgManager);
            this.ZhipaiManager = new Workflow.ZhipaiManager(this._coldewManager.OrgManager);

            Stream stream = typeof(Yinqing).Assembly.GetManifestResourceStream("Coldew.Core.Workflow.Images.buzhou.gif");
            byte[] buzhouBytes = new byte[stream.Length];
            stream.Read(buzhouBytes, 0, buzhouBytes.Length);
            stream.Close();
            this.BuzhouImage = Image.FromStream(new MemoryStream(buzhouBytes));

            stream = typeof(Yinqing).Assembly.GetManifestResourceStream("Coldew.Core.Workflow.Images.end.gif");
            byte[] jieshuBuzhouBytes = new byte[stream.Length];
            stream.Read(jieshuBuzhouBytes, 0, jieshuBuzhouBytes.Length);
            stream.Close();
            this.JieshuBuzhouImage = Image.FromStream(new MemoryStream(jieshuBuzhouBytes));

            stream = typeof(Yinqing).Assembly.GetManifestResourceStream("Coldew.Core.Workflow.Images.begin.gif");
            byte[] kaishiBuzhouBytes = new byte[stream.Length];
            stream.Read(kaishiBuzhouBytes, 0, kaishiBuzhouBytes.Length);
            stream.Close();
            this.KaishiBuzhouImage = Image.FromStream(new MemoryStream(kaishiBuzhouBytes));

        }

        public ZhipaiManager ZhipaiManager { private set; get; }

        public JianglaiZhipaiManager JianglaiZhipaiManager { private set; get; }

        public Image BuzhouImage { private set; get; }

        public Image JieshuBuzhouImage { private set; get; }

        public Image KaishiBuzhouImage { private set; get; }

        private object _lock = new object();

        void Liucheng_Shanchuhou(LiuchengMoban liucheng)
        {
            List<LiuchengMoban> liuchengList = this._liuchengList.ToList();
            liuchengList.Remove(liucheng);
            this._liuchengList = liuchengList;
        }

        public List<LiuchengMoban> LiuchengMobanList
        {
            get
            {
                return this._liuchengList.ToList();
            }
        }

        public LiuchengMoban GetLiuchengMoban(int id)
        {
            return this._liuchengList.Find(x => x.Id == id);
        }

        public Renwu GetRenwu(int renwuId)
        {
            foreach (LiuchengMoban liuchengMoban in this._liuchengList)
            {
                foreach (Liucheng liucheng in liuchengMoban.LiuchengList)
                {
                    foreach (Xingdong renwu in liucheng.XingdongList)
                    {
                        Renwu xingdong = renwu.RenwuList.Find(x => x.Id == renwuId);
                        if (xingdong != null)
                        {
                            return xingdong;
                        }
                    }
                }
            }
            return null;
        }

        public Renwu GetRenwu(string renwuId)
        {
            foreach (LiuchengMoban liuchengMoban in this._liuchengList)
            {
                foreach (Liucheng liucheng in liuchengMoban.LiuchengList)
                {
                    foreach (Xingdong renwu in liucheng.XingdongList)
                    {
                        Renwu xingdong = renwu.RenwuList.Find(x => x.Guid == renwuId);
                        if (xingdong != null)
                        {
                            return xingdong;
                        }
                    }
                }
            }
            return null;
        }

        public Liucheng GetLiucheng(string id)
        {
            foreach (LiuchengMoban liuchengMoban in this._liuchengList)
            {
                Liucheng liucheng = liuchengMoban.GetLiucheng(id);
                if (liucheng != null)
                {
                    return liucheng;
                }
            }
            return null;
        }

        public Xingdong GetXingdong(string guid)
        {
            foreach (LiuchengMoban liuchengMoban in this._liuchengList)
            {
                foreach (Liucheng liucheng in liuchengMoban.LiuchengList)
                {
                    Xingdong renwu = liucheng.GetRenwu(guid);
                    if (renwu != null)
                    {
                        return renwu;
                    }
                }
            }
            return null;
        }

        public User GetYonghu(string zhanghao)
        {
            return this._coldewManager.OrgManager.UserManager.GetUserByAccount(zhanghao);
        }

        public User GetYonghuByGuid(string guid)
        {
            return this._coldewManager.OrgManager.UserManager.GetUserById(guid);
        }

        public ILog Logger { private set; get; }

        protected abstract List<LiuchengMoban> JiazaiLiuchengMoban();
    }
}
