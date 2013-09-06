using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Core.Organization;
using Crm.Data;
using Crm.Api.Exceptions;
using Crm.Core.Extend;

namespace Crm.Core
{
    public class Contact
    {
        public Contact(string id, string name, Customer customer, User creator,
            DateTime createTime, User modifiedUser, DateTime modifiedTime, Metadata metadata)
        {
            this.ID = id;
            this.Name = name;
            this.Customer = customer;
            this.Creator = creator;
            this.CreateTime = createTime;
            this.ModifiedUser = modifiedUser;
            this.ModifiedTime = modifiedTime;
            this.Metadata = metadata;
            this.Customer.Deleted += new TEventHanlder<Customer, User>(Customer_Deleted);
            this.BuildContent();
        }

        void Customer_Deleted(Customer args, User opUser)
        {
            this._Delete(opUser);
        }

        public string ID { private set; get; }

        public string Name { private set; get; }

        public Customer Customer { private set; get; }

        public User Creator { private set; get; }

        public DateTime CreateTime { private set; get; }

        public User ModifiedUser { private set; get; }

        public DateTime ModifiedTime { private set; get; }

        public Metadata Metadata { private set; get; }

        public string Content { private set; get; }

        private void BuildContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Name.ToLower());
            sb.Append(this.Customer.Name.ToLower());
            sb.Append(this.Creator.Name.ToLower());
            sb.Append(this.Creator.Account.ToLower());

            foreach (MetadataProperty property in this.Metadata.GetPropertys())
            {
                if (!string.IsNullOrEmpty(property.Value.ShowValue))
                {
                    sb.Append(property.Value.ShowValue.ToLower());
                }
            }
            this.Content = sb.ToString();
        }

        public event TEventHanlder<Contact, ContactModifyInfo> Modifying;
        public event TEventHanlder<Contact, ContactModifyInfo> Modified;

        public void Modify(ContactModifyInfo info)
        {
            if (this.Modifying != null)
            {
                this.Modifying(this, info);
            }

            this.Metadata.SetPropertys(info.PropertyInfos);

            ContactModel model = NHibernateHelper.CurrentSession.Get<ContactModel>(this.ID);
            model.ModifiedTime = DateTime.Now;
            model.ModifiedUserId = info.OpUser.ID;
            model.Name = info.Name;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.ModifiedTime = DateTime.Now;
            this.ModifiedUser = info.OpUser;
            this.Name = info.Name;

            this.BuildContent();

            if (this.Modified != null)
            {
                this.Modified(this, info);
            }
        }

        public event TEventHanlder<Contact, User> Deleting;
        public event TEventHanlder<Contact, User> Deleted;

        public void Delete(User opUser)
        {
            if (!this.CanDelete(opUser))
            {
                throw new CrmException("没有权限删除该联系人");
            }

            this._Delete(opUser);
        }

        private void _Delete(User opUser)
        {
            if (this.Deleting != null)
            {
                this.Deleting(this, opUser);
            }

            ContactModel model = NHibernateHelper.CurrentSession.Get<ContactModel>(this.ID);

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

            if (this.Customer.CanDelete(user))
            {
                return true;
            }

            return false;
        }

        public ContactInfo Map()
        {
            return new ContactInfo
            {
                CreateTime = this.CreateTime,
                Creator = this.Creator.MapUserInfo(),
                ID = this.ID,
                ModifiedTime = this.ModifiedTime,
                ModifiedUser = this.ModifiedUser.MapUserInfo(),
                Name = this.Name,
                CustomerName = this.Customer.Name,
                CustomerId = this.Customer.ID,
                Metadata = this.Metadata.MapMetadataInfo()
            };
        }
    }
}
