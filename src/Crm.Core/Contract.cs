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
    public class Contract : Metadata
    {
        public Contract(string id, List<MetadataProperty> propertys, ColdewObject form)
            :base(id, propertys, form)
        {

        }

        public int ExpiredComputeDays 
        {
            get 
            {
                NumberMetadataValue value = this.GetProperty(CrmObjectConstCode.CONTRACT_FIELD_NAME_ExpiredComputeDays).Value as NumberMetadataValue;
                return (int)value.Number.Value;
            } 
        }

        public DateTime EndDate 
        { 
            get 
            {
                DateMetadataValue dateValue = this.GetProperty(CrmObjectConstCode.CONTRACT_FIELD_NAME_END_DATE).Value as DateMetadataValue;
                return dateValue.Date.Value;
            } 
        }

        public bool Expiring
        {
            get
            {
                int days = (this.EndDate.Date - DateTime.Now.Date).Days;
                return days > 0 && days < this.ExpiredComputeDays;
            }
        }

        public bool Expired
        {
            get
            {
                return this.EndDate.Date <= DateTime.Now.Date;
            }
        }

        private UserListMetadataValue OwnersValue
        {
            get
            {
                return this.GetProperty(CrmObjectConstCode.CONTRACT_FIELD_NAME_OWNER_USERS).Value as UserListMetadataValue;
            }
        }

        public List<User> Owners
        {
            get
            {
                return OwnersValue.Users;
            }
        }

        public Customer Customer
        {
            get
            {
                MetadataProperty property = this.GetProperty(CrmObjectConstCode.FIELD_NAME_CUSTOMER);
                MetadataRelatedValue value = property.Value as MetadataRelatedValue;
                return value.Metadata as Customer;
            }
        }

        //public void SetEmailNotified(bool notified)
        //{
        //    ContractModel model = NHibernateHelper.CurrentSession.Get<ContractModel>(this.ID);
        //    model.EmailNotified = notified;

        //    NHibernateHelper.CurrentSession.Update(model);
        //    NHibernateHelper.CurrentSession.Flush();

        //    this.EmailNotified = notified;
        //}

        public bool CanPreview(User user)
        {
            if (user == this.Creator)
            {
                return true;
            }

            if (this.Owners.Contains(user))
            {
                return true;
            }

            if (this.Customer.CanPreview(user))
            {
                return true;
            }

            return false;
        }

        public bool CanDelete(User user)
        {
            if (user == this.Creator)
            {
                return true;
            }

            if (this.Owners.Contains(user))
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
