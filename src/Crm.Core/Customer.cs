using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Crm.Data;
using Crm.Api;
using Crm.Api.Exceptions;

namespace Crm.Core
{
    public class Customer
    {
        public Customer(string id, string name, CustomerArea area, List<User> salesUsers, User creator,
            DateTime createTime, User modifiedUser, DateTime modifiedTime, Metadata metadata)
        {
            this.ID = id;
            this.Name = name;
            this.Area = area;
            this.SalesUsers = salesUsers;
            this.Creator = creator;
            this.CreateTime = createTime;
            this.ModifiedUser = modifiedUser;
            this.ModifiedTime = modifiedTime;
            this.Metadata = metadata;

            this.BuildContent();
        }

        public string ID { private set; get; }

        public string Name { private set; get; }

        public CustomerArea Area { private set; get; }

        public List<User> SalesUsers { private set; get; }

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
            sb.Append(this.Area.Name.ToLower());
            sb.Append(string.Join(" ", this.SalesUsers.Select(x => x.Name.ToLower())));
            sb.Append(string.Join(" ", this.SalesUsers.Select(x => x.Account.ToLower())));
            sb.Append(this.Creator.Name.ToLower());
            sb.Append(this.Creator.Account.ToLower());
            sb.Append(this.ModifiedUser.Account.ToLower());
            sb.Append(this.ModifiedUser.Name.ToLower());
            foreach (MetadataProperty property in this.Metadata.GetPropertys())
            {
                if (!string.IsNullOrEmpty(property.Value.ShowValue))
                {
                    sb.Append(property.Value.ShowValue.ToLower());
                }
            }
            this.Content = sb.ToString();
        }

        public event TEventHanlder<Customer, CustomerModifyInfo> Modifying;
        public event TEventHanlder<Customer, CustomerModifyInfo> Modified;

        public void Modify(CustomerModifyInfo info)
        {
            if (info.SalesUsers == null || info.SalesUsers.Count == 0)
            {
                throw new CustomerSalesUserNullException();
            }
            if (this.Modifying != null)
            {
                this.Modifying(this, info);
            }

            this.Metadata.SetPropertys(info.PropertyInfos);

            CustomerModel model = NHibernateHelper.CurrentSession.Get<CustomerModel>(this.ID);
            model.AreaId = info.Area.ID;
            model.ModifiedTime = DateTime.Now;
            model.ModifiedUserId = info.OpUser.ID;
            model.Name = info.Name;
            model.SalesUserIds = string.Join(",", info.SalesUsers.Select(x => x.ID).ToArray());

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Area = info.Area;
            this.ModifiedTime = DateTime.Now;
            this.ModifiedUser = info.OpUser;
            this.Name = info.Name;
            this.SalesUsers = info.SalesUsers;

            this.BuildContent();

            if (this.Modified != null)
            {
                this.Modified(this, info);
            }
        }

        public event TEventHanlder<Customer, User> Deleting;
        public event TEventHanlder<Customer, User> Deleted;

        public void Delete(User opUser)
        {
            if (!this.CanDelete(opUser))
            {
                throw new CrmException("没有权限删除该客户!");
            }

            if (this.Deleting != null)
            {
                this.Deleting(this, opUser);
            }

            CustomerModel model = NHibernateHelper.CurrentSession.Get<CustomerModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Metadata.Delete();

            if (this.Deleted != null)
            {
                this.Deleted(this, opUser);
            }
        }

        public void Favorite(User opUser)
        {
            if (this.Deleting != null)
            {
                this.Deleting(this, opUser);
            }

            CustomerModel model = NHibernateHelper.CurrentSession.Get<CustomerModel>(this.ID);

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
            if (user.Role == Api.Organization.UserRole.Administrator)
            {
                return true;
            }

            if (user == this.Creator)
            {
                return true;
            }

            if (this.SalesUsers.Contains(user))
            {
                return true;
            }

            if (this.Area.ManagerUsers.Contains(user))
            {
                return true;
            }

            if (this.Creator.IsMySuperior(user, true))
            {
                return true;
            }

            if (this.SalesUsers.Any(x => x.IsMySuperior(user, true)))
            {
                return true;
            }

            if (this.Area.ManagerUsers.Any(x => x.IsMySuperior(user, true)))
            {
                return true;
            }

            return false;
        }

        public bool CanDelete(User user)
        {
            if (user.Role == Api.Organization.UserRole.Administrator)
            {
                return true;
            }

            if (user == this.Creator)
            {
                return true;
            }

            if (this.Area.ManagerUsers.Contains(user))
            {
                return true;
            }

            if (this.Creator.IsMySuperior(user, true))
            {
                return true;
            }

            if (this.Area.ManagerUsers.Any(x => x.IsMySuperior(user, true)))
            {
                return true;
            }

            return false;
        }

        public CustomerInfo Map()
        {
            return new CustomerInfo
            {
                Area = this.Area.Map(),
                CreateTime = this.CreateTime,
                Creator = this.Creator.MapUserInfo(),
                ID = this.ID,
                ModifiedTime = this.ModifiedTime,
                ModifiedUser = this.ModifiedUser.MapUserInfo(),
                Name = this.Name,
                SalesUsers = this.SalesUsers.Select(x => x.MapUserInfo()).ToList(),
                Metadata = this.Metadata.MapMetadataInfo()
            };
        }
    }
}
