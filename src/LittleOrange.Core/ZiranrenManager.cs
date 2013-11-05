using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Coldew.Core;
using Coldew.Core.Organization;
using Coldew.Data;
using LittleOrange.Data;

namespace LittleOrange.Core
{
    public class ZiranrenManager : MetadataManager
    {

        public ZiranrenManager(ColdewObject form, OrganizationManagement orgManger)
            :base(form, orgManger)
        {
            
        }

        protected override Metadata CreateAndSaveDB(List<MetadataProperty> propertys)
        {
            ZhiranrenModel model = new ZhiranrenModel();
            model.PropertysJson = MetadataPropertyListHelper.ToPropertyModelJson(propertys);
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            Ziranren metadata = new Ziranren(model.ID, propertys, this.ColdewObject);
            return metadata;
        }

        protected override List<Metadata> LoadFromDB()
        {
            List<Metadata> metadatas = new List<Metadata>();

            IList<ZhiranrenModel> models = NHibernateHelper.CurrentSession.QueryOver<ZhiranrenModel>().List();
            foreach (ZhiranrenModel model in models)
            {
                Ziranren metadata = new Ziranren(model.ID, MetadataPropertyListHelper.GetPropertys(model.PropertysJson, this.ColdewObject), this.ColdewObject);

                metadatas.Add(metadata);
            }
            return metadatas;
        }
    }
}
