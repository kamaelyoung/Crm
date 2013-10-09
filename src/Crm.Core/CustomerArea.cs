using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Data;
using Crm.Api.Exceptions;
using Coldew.Core.Organization;
using Coldew.Core;

namespace Crm.Core
{
    public class CustomerArea
    {
        public CustomerArea(int id, string name, List<User> managerUsers)
        {
            this.ID = id;
            this.Name = name;
            this.ManagerUsers = managerUsers;
        }

        public int ID { private set; get; }

        public string Name { private set; get; }

        public List<User> ManagerUsers { private set; get; }

        public event TEventHandler<CustomerArea, CustomerAreaModifyInfo> Modifying;
        public event TEventHandler<CustomerArea, CustomerAreaModifyInfo> Modified;

        public void Modify(CustomerAreaModifyInfo info)
        {
            if (Modifying != null)
            {
                this.Modifying(this, info);
            }

            CustomerAreaModel model = NHibernateHelper.CurrentSession.Get<CustomerAreaModel>(this.ID);
            model.Name = info.Name;
            model.ManagerAccounts = string.Join(",", info.ManagerUsers.Select(x => x.Account));
            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Name = model.Name;
            this.ManagerUsers = info.ManagerUsers;

            if (Modified != null)
            {
                this.Modified(this, info);
            }
        }

        public event TEventHandler<CustomerArea> Deleting;
        public event TEventHandler<CustomerArea> Deleted;

        public void Delete()
        {
            if (this.Deleting != null)
            {
                this.Deleting(this);
            }
            CustomerAreaModel model = NHibernateHelper.CurrentSession.Get<CustomerAreaModel>(this.ID);
            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();

            if (this.Deleted != null)
            {
                this.Deleted(this);
            }
        }

        public CustomerAreaInfo Map()
        {
            return new CustomerAreaInfo
            {
                ID = this.ID,
                Name = this.Name,
                Managers = this.ManagerUsers.Select(x => x.MapUserInfo()).ToList()
            };
        }
    }
}
