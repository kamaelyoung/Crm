﻿using System;
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
    public class Contact : Metadata
    {
        public Contact(string id, MetadataPropertyList propertys, Form form)
            : base(id, propertys, form)
        {

        }

        public Customer Customer
        {
            get
            {
                MetadataProperty property = this.GetProperty(CrmFormConstCode.FIELD_NAME_CUSTOMER);
                MetadataMetadataValue value = property.Value as MetadataMetadataValue;
                return value.Metadata as Customer;
            }
        }

        protected override void UpdateDB(MetadataPropertyList propertys)
        {
            ContactModel model = NHibernateHelper.CurrentSession.Get<ContactModel>(this.ID);
            model.PropertysJson = propertys.ToJson();

            NHibernateHelper.CurrentSession.Update(model);
        }

        protected override void DeleteDB()
        {
            ContactModel model = NHibernateHelper.CurrentSession.Get<ContactModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        public override bool CanPreview(User user)
        {

            if (user == this.Creator)
            {
                return true;
            }

            if (this.Customer.CanPreview(user))
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

            if (this.Customer.CanDelete(user))
            {
                return true;
            }

            return false;
        }
    }
}
