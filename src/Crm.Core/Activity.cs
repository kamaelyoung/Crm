using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Crm.Data;
using Crm.Api;
using Crm.Api.Exceptions;
using Crm.Core.Extend;

namespace Crm.Core
{
    public class Activity
    {
        public Activity(string id, string subject, Contact contact, User creator,
            DateTime createTime, User modifiedUser, DateTime modifiedTime, Metadata metadata)
        {
            this.ID = id;
            this.Subject = subject;
            this.Contact = contact;
            this.Creator = creator;
            this.CreateTime = createTime;
            this.ModifiedUser = modifiedUser;
            this.ModifiedTime = modifiedTime;
            this.Metadata = metadata;
            this.Contact.Deleted += new TEventHanlder<Contact, User>(Contact_Deleted);
            this.BuildContent();
        }

        void Contact_Deleted(Contact sender, User args)
        {
            this._Delete(args);
        }

        public string ID { private set; get; }

        public string Subject { private set; get; }

        public Contact Contact { private set; get; }

        public User Creator { private set; get; }

        public DateTime CreateTime { private set; get; }

        public User ModifiedUser { private set; get; }

        public DateTime ModifiedTime { private set; get; }

        public Metadata Metadata { private set; get; }

        public string Content { private set; get; }

        private void BuildContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Subject.ToLower());
            sb.Append(this.Contact.Customer.Name.ToLower());
            sb.Append(this.Contact.Name);
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

        public event TEventHanlder<Activity, ActivityModifyInfo> Modifying;
        public event TEventHanlder<Activity, ActivityModifyInfo> Modified;

        public void Modify(ActivityModifyInfo info)
        {
            if (this.Modifying != null)
            {
                this.Modifying(this, info);
            }

            this.Metadata.SetPropertys(info.PropertyInfos);

            ActivityModel model = NHibernateHelper.CurrentSession.Get<ActivityModel>(this.ID);
            model.ModifiedTime = DateTime.Now;
            model.ModifiedUserId = info.OpUser.ID;
            model.Subject = info.Subject;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.ModifiedTime = DateTime.Now;
            this.ModifiedUser = info.OpUser;
            this.Subject = info.Subject;

            this.BuildContent();

            if (this.Modified != null)
            {
                this.Modified(this, info);
            }
        }

        public event TEventHanlder<Activity> Deleting;
        public event TEventHanlder<Activity> Deleted;

        public void Delete(User opUser)
        {
            if (!this.CanDelete(opUser))
            {
                throw new CrmException("没有权限删除该记录");
            }

            this._Delete(opUser);
        }

        private void _Delete(User opUser)
        {
            if (this.Deleting != null)
            {
                this.Deleting(this);
            }

            ActivityModel model = NHibernateHelper.CurrentSession.Get<ActivityModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Metadata.Delete();

            if (this.Deleted != null)
            {
                this.Deleted(this);
            }
        }

        public bool CanPreview(User user)
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

        public bool CanDelete(User user)
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

        public ActivityInfo Map()
        {
            return new ActivityInfo
            {
                CreateTime = this.CreateTime,
                Creator = this.Creator.MapUserInfo(),
                ID = this.ID,
                ModifiedTime = this.ModifiedTime,
                ModifiedUser = this.ModifiedUser.MapUserInfo(),
                Subject = this.Subject,
                CustomerName = this.Contact.Customer.Name,
                CustomerId = this.Contact.Customer.ID,
                ContactName = this.Contact.Name,
                ContactId = this.Contact.ID,
                Metadata = this.Metadata.MapMetadataInfo()
            };
        }
    }
}
