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
    public class CustomerManager : MetadataManager
    {

        public CustomerManager(Form form, OrganizationManagement orgManger)
            :base(form, orgManger)
        {
            
        }

        public int GetAreaCustomerCount(CustomerArea args)
        {
            int count = this._metadataList.Where(x => ((Customer)x).Area == args).Count();
            return count;
        }

        protected override Metadata CreateAndSaveDB(MetadataPropertyList propertys)
        {
            CustomerModel model = new CustomerModel();
            model.PropertysJson = propertys.ToJson();
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            Customer metadata = new Customer(model.ID, propertys, this.Form);
            return metadata;
        }

        protected override List<Metadata> LoadFromDB()
        {
            List<Metadata> metadatas = new List<Metadata>();

            IList<CustomerModel> models = NHibernateHelper.CurrentSession.QueryOver<CustomerModel>().List();
            foreach (CustomerModel model in models)
            {
                Customer metadata = new Customer(model.ID, MetadataPropertyList.GetPropertys(model.PropertysJson, this.Form), this.Form);

                metadatas.Add(metadata);
            }
            return metadatas;
        }
    }
}
