using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Newtonsoft.Json;
using Coldew.Core;
using LittleOrange.Data;

namespace LittleOrange.Core
{
    public class Ziranren : Metadata
    {
        public Ziranren(string id, List<MetadataProperty> propertys, ColdewObject form)
            : base(id, propertys, form)
        {

        }

        protected override void UpdateDB(List<MetadataProperty> propertys)
        {
            ZhiranrenModel model = NHibernateHelper.CurrentSession.Get<ZhiranrenModel>(this.ID);
            model.PropertysJson = MetadataPropertyListHelper.ToPropertyModelJson(propertys);

            NHibernateHelper.CurrentSession.Update(model);
        }

        protected override void DeleteDB()
        {
            ZhiranrenModel model = NHibernateHelper.CurrentSession.Get<ZhiranrenModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();
        }
    }
}
