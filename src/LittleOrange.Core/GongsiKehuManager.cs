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
    public class GongsiKehuManager : MetadataManager
    {

        public GongsiKehuManager(ColdewObject form, OrganizationManagement orgManger)
            :base(form, orgManger)
        {
            
        }

        protected override Metadata CreateAndSaveDB(List<MetadataProperty> propertys)
        {
            GongsiKehuModel model = new GongsiKehuModel();
            model.PropertysJson = MetadataPropertyListHelper.ToPropertyModelJson(propertys);
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            Customer metadata = new Customer(model.ID, propertys, this.ColdewObject);
            return metadata;
        }

        protected override List<Metadata> LoadFromDB()
        {
            List<Metadata> metadatas = new List<Metadata>();

            IList<GongsiKehuModel> models = NHibernateHelper.CurrentSession.QueryOver<GongsiKehuModel>().List();
            foreach (GongsiKehuModel model in models)
            {
                Customer metadata = new Customer(model.ID, MetadataPropertyListHelper.GetPropertys(model.PropertysJson, this.ColdewObject), this.ColdewObject);

                metadatas.Add(metadata);
            }
            return metadatas;
        }
    }
}
