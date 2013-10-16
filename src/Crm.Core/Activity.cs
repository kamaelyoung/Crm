using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Crm.Data;
using Crm.Api;
using Crm.Api.Exceptions;
using Newtonsoft.Json;
using Coldew.Core;

namespace Crm.Core
{
    public class Activity : Metadata
    {
        public Activity(string id, List<MetadataProperty> propertys, ColdewObject form)
            : base(id, propertys, form)
        {

            
        }

        protected override List<MetadataProperty> GetVirtualPropertys()
        {
            List<MetadataProperty> propertys = new List<MetadataProperty>();
            MetadataField customerField = this.ColdewObject.GetFieldByCode(CrmObjectConstCode.FIELD_NAME_CUSTOMER) as MetadataField;
            MetadataProperty customerProperty = new MetadataProperty(new FunctionMetadataRelatedValue(delegate() { return this.Contact.Customer;}, customerField));
            propertys.Add(customerProperty);
            return propertys;
        }

        public Contact Contact
        {
            get
            {
                MetadataRelatedValue value = this.GetProperty(CrmObjectConstCode.FIELD_NAME_CONTACT).Value as MetadataRelatedValue;
                return value.Metadata as Contact;
            }
        }

        protected override void UpdateDB(List<MetadataProperty> propertys)
        {
            ActivityModel model = NHibernateHelper.CurrentSession.Get<ActivityModel>(this.ID);
            model.PropertysJson = MetadataPropertyListHelper.ToPropertyModelJson(propertys);

            NHibernateHelper.CurrentSession.Update(model);
        }

        protected override void DeleteDB()
        {
            ActivityModel model = NHibernateHelper.CurrentSession.Get<ActivityModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        public override bool CanPreview(User user)
        {

            if (user == this.Creator)
            {
                return true;
            }

            if (this.Contact.Customer.CanPreview(user))
            {
                return true;
            }

            return false;
        }

        public override bool CanDelete(User user)
        {

            if (user == this.Creator)
            {
                return true;
            }

            if (this.Contact.Customer.CanDelete(user))
            {
                return true;
            }

            return false;
        }
    }
}
