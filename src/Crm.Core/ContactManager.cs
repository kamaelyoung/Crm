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
    public class ContactManager : MetadataManager
    {
        public ContactManager(Form form, OrganizationManagement orgManger)
            :base(form, orgManger)
        {
            
        }

        protected override Metadata CreateAndSaveDB(MetadataPropertyList propertys)
        {
            ContactModel model = new ContactModel();
            model.PropertysJson = propertys.ToJson();
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            Contact metadata = new Contact(model.ID, propertys, this.Form);
            return metadata;
        }

        protected override List<Metadata> LoadFromDB()
        {
            List<Metadata> metadatas = new List<Metadata>();

            IList<ContactModel> models = NHibernateHelper.CurrentSession.QueryOver<ContactModel>().List();
            foreach (ContactModel model in models)
            {
                Contact metadata = new Contact(model.ID, MetadataPropertyList.GetPropertys(model.PropertysJson, this.Form), this.Form);

                metadatas.Add(metadata);
            }
            return metadatas;
        }
    }
}
