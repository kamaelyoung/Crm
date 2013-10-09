using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data;
using Crm.Api;
using System.Text.RegularExpressions;
using Crm.Api.Exceptions;
using Newtonsoft.Json;
using Coldew.Core;
using Coldew.Core.Organization;

namespace Crm.Core
{
    public class ActivityManager : MetadataManager
    {
        public ActivityManager(Form form, OrganizationManagement orgManger)
            :base(form, orgManger)
        {
            
        }

        protected override Metadata CreateAndSaveDB(MetadataPropertyList propertys)
        {
            ActivityModel model = new ActivityModel();
            model.PropertysJson = propertys.ToJson();
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            Activity metadata = new Activity(model.ID, propertys, this.Form);
            return metadata;
        }

        protected override List<Metadata> LoadFromDB()
        {
            List<Metadata> metadatas = new List<Metadata>();

            IList<ActivityModel> models = NHibernateHelper.CurrentSession.QueryOver<ActivityModel>().List();
            foreach (ActivityModel model in models)
            {
                Activity metadata = new Activity(model.ID, MetadataPropertyList.GetPropertys(model.PropertysJson, this.Form), this.Form);

                metadatas.Add(metadata);
            }
            return metadatas;
        }
    }
}
