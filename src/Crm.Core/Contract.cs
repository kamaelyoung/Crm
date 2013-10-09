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
        public Contract(string id, MetadataPropertyList propertys, Form form)
            :base(id, propertys, form)
        {

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

        public List<User> Owners { private set; get; }

        public float Value { private set; get; }

        public User Creator { private set; get; }

        public DateTime CreateTime { private set; get; }

        public User ModifiedUser { private set; get; }

        public DateTime ModifiedTime { private set; get; }

        public Metadata Metadata { private set; get; }

        public bool EmailNotified { private set; get; }

        public string Content { private set; get; }

        private void BuildContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Name.ToLower());
            sb.Append(this.Customer.Name.ToLower());
            sb.Append(this.Creator.Name.ToLower());
            sb.Append(this.Creator.Account.ToLower());
            sb.Append(string.Join(",", this.Owners.Select(x => x.Account.ToLower())));
            sb.Append(string.Join(",", this.Owners.Select(x => x.Name.ToLower())));
            
            foreach (MetadataProperty property in this.Metadata.GetPropertys())
            {
                if (!string.IsNullOrEmpty(property.Value.ShowValue))
                {
                    sb.Append(property.Value.ShowValue.ToLower());
                }
            }
            this.Content = sb.ToString();
        }

        public event TEventHandler<Contract, ContractModifyInfo> Modifying;
        public event TEventHandler<Contract, ContractModifyInfo> Modified;

        public void Modify(ContractModifyInfo info)
        {
            if (this.Modifying != null)
            {
                this.Modifying(this, info);
            }

            this.Metadata.SetPropertys(info.PropertyInfos);

            bool endDateChanged = this.EndDate.Date != info.EndDate.Date;

            ContractModel model = NHibernateHelper.CurrentSession.Get<ContractModel>(this.ID);
            model.StartDate = info.StartDate;
            model.EndDate = info.EndDate;
            model.ExpiredComputeDays = info.ExpiredComputeDays;
            model.OwnerAccounts = string.Join(",", info.Owners.Select(x => x.Account));
            model.ModifiedTime = DateTime.Now;
            model.ModifiedUserId = info.OpUser.ID;
            model.Name = info.Name;
            model.Value = info.Value;
            if (endDateChanged)
            {
                model.EmailNotified = false;
            }

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.StartDate = info.StartDate;
            this.EndDate = info.EndDate;
            this.ExpiredComputeDays = info.ExpiredComputeDays;
            this.Owners = info.Owners;
            this.ModifiedTime = DateTime.Now;
            this.ModifiedUser = info.OpUser;
            this.Name = info.Name;
            this.EmailNotified = model.EmailNotified;
            this.Value = info.Value;

            this.BuildContent();

            if (this.Modified != null)
            {
                this.Modified(this, info);
            }
        }

        public void SetEmailNotified(bool notified)
        {
            ContractModel model = NHibernateHelper.CurrentSession.Get<ContractModel>(this.ID);
            model.EmailNotified = notified;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.EmailNotified = notified;
        }

        public event TEventHandler<Contract, User> Deleting;
        public event TEventHandler<Contract, User> Deleted;

        public void Delete(User opUser)
        {
            if (!this.CanDelete(opUser))
            {
                throw new CrmException("没有权限删除该合同");
            }

            this._Delete(opUser);
        }

        private void _Delete(User opUser)
        {
            if (this.Deleting != null)
            {
                this.Deleting(this, opUser);
            }

            ContractModel model = NHibernateHelper.CurrentSession.Get<ContractModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Metadata.Delete();

            if (this.Deleted != null)
            {
                this.Deleted(this, opUser);
            }
        }

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

        public ContractInfo Map()
        {
            return new ContractInfo
            {
                EndDate = this.EndDate,
                ExpiredComputeDays = this.ExpiredComputeDays,
                Owners = this.Owners.Select(x => x.MapUserInfo()).ToList(),
                StartDate = this.StartDate,
                CreateTime = this.CreateTime,
                Creator = this.Creator.MapUserInfo(),
                ID = this.ID,
                ModifiedTime = this.ModifiedTime,
                ModifiedUser = this.ModifiedUser.MapUserInfo(),
                Name = this.Name,
                CustomerName = this.Customer.Name,
                CustomerId = this.Customer.ID,
                Value = this.Value,
                Metadata = this.Metadata.Map()
            };
        }
    }
}
