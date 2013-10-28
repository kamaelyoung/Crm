using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Coldew.Api.Workflow;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class JianglaiRenwuZhipai
    {
        public JianglaiRenwuZhipai(int id, User zhipairen, User dailiren, DateTime? kaishiShijian, DateTime? jieshuShijian)
        {
            this.Id = id;
            this.Zhipairen = zhipairen;
            this.Dailiren = dailiren;
            this.KaishiShijian = kaishiShijian;
            this.JieshuShijian = jieshuShijian;
        }

        public int Id { private set; get; }

        public User Zhipairen { private set; get; } 

        public User Dailiren { private set; get; } 

        public DateTime? KaishiShijian { private set;get; }

        public DateTime? JieshuShijian { private set; get; }

        public void Xiugai(User dailiren, DateTime? kaishiShijian, DateTime? jieshuShijian)
        {
            JianglaiRenwuZhipaiModel model = NHibernateHelper.CurrentSession.Get<JianglaiRenwuZhipaiModel>(this.Id);
            model.Dailiren = dailiren.Account;
            model.JieshuShijian = jieshuShijian;
            model.KaishiShijian = kaishiShijian;
            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Dailiren = dailiren;
            this.JieshuShijian = jieshuShijian;
            this.KaishiShijian = kaishiShijian;
        }

        public JianglaiZhipaiXinxi Map()
        {
            return new JianglaiZhipaiXinxi { Dailiren = this.Dailiren.MapUserInfo(), JieshuShijian = this.JieshuShijian, KaishiShijian = this.KaishiShijian };
        }
    }
}
