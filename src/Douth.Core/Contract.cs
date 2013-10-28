using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Newtonsoft.Json;
using Coldew.Core;

namespace Douth.Core
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
                NumberMetadataValue value = this.GetProperty(DouthObjectConstCode.CONTRACT_FIELD_NAME_ExpiredComputeDays).Value as NumberMetadataValue;
                return (int)value.Number.Value;
            } 
        }

        public DateTime EndDate 
        { 
            get 
            {
                DateMetadataValue dateValue = this.GetProperty(DouthObjectConstCode.CONTRACT_FIELD_NAME_END_DATE).Value as DateMetadataValue;
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
                return this.GetProperty(DouthObjectConstCode.CONTRACT_FIELD_NAME_OWNER_USERS).Value as UserListMetadataValue;
            }
        }

        public List<User> Owners
        {
            get
            {
                return OwnersValue.Users;
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

        public override bool CanPreview(User user)
        {
            if (user == this.Creator)
            {
                return true;
            }

            if (this.Owners.Contains(user))
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

            if (this.Owners.Contains(user))
            {
                return true;
            }

            return false;
        }
    }
}
