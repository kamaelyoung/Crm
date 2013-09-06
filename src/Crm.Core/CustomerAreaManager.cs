using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data;
using Crm.Core.Organization;
using log4net.Util;
using Crm.Api.Exceptions;

namespace Crm.Core
{
    public class CustomerAreaManager
    {
        OrganizationManagement _orgManager;
        ReaderWriterLock _lock;
        public CustomerAreaManager(OrganizationManagement orgManager)
        {
            this._orgManager = orgManager;
            this.Areas = new List<CustomerArea>();
            this._lock = new ReaderWriterLock();

            this.Load();
        }

        public event TEventHanlder<CustomerArea> CustomerAreaDeleting;

        private List<CustomerArea> Areas { set; get; }

        public CustomerArea Create(string name, List<User> managers)
        {
            this._lock.AcquireWriterLock();
            try
            {
                name = name.Trim();
                if (this.Areas.Any(x => x.Name == name))
                {
                    throw new CrmException("区域名称重复");
                }

                if (managers == null)
                {
                    managers = new List<User>();
                }

                CustomerAreaModel model = new CustomerAreaModel();
                model.ManagerAccounts = string.Join(",", managers.Select(x => x.Account));
                model.Name = name;
                model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();

                return this.Create(model);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private CustomerArea Create(CustomerAreaModel model)
        {
            List<User> managers = new List<User>();
            if (!string.IsNullOrEmpty(model.ManagerAccounts))
            {
                managers = model.ManagerAccounts.Split(',').Select(x => this._orgManager.UserManager.GetUserByAccount(x)).ToList();
            }
            CustomerArea area = new CustomerArea(model.ID, model.Name, managers);
            area.Deleting += new TEventHanlder<CustomerArea>(Area_Deleting);
            area.Deleted += new TEventHanlder<CustomerArea>(Area_Deleted);
            area.Modifying += new TEventHanlder<CustomerArea, CustomerAreaModifyInfo>(Area_Modifying);
            this.Areas.Add(area);
            this.Areas = this.Areas.OrderBy(x => x.Name).ToList();
            return area;
        }

        void Area_Deleting(CustomerArea args)
        {
            if (this.CustomerAreaDeleting != null)
            {
                this.CustomerAreaDeleting(args);
            }
        }

        void Area_Modifying(CustomerArea sender, CustomerAreaModifyInfo args)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (args.Name != sender.Name && this.Areas.Any(x => x.Name == args.Name))
                {
                    throw new CrmException("区域名称重复");
                }
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        void Area_Deleted(CustomerArea args)
        {
            this._lock.AcquireWriterLock();
            try
            {
                this.Areas.Remove(args);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public CustomerArea GetAreaById(int areaId)
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this.Areas.Find(x => x.ID == areaId);
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<CustomerArea> GetAllArea()
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this.Areas.ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private void Load()
        {
            IList<CustomerAreaModel> models = NHibernateHelper.CurrentSession.QueryOver<CustomerAreaModel>().List();
            foreach (CustomerAreaModel model in models)
            {
                this.Create(model);
            }
        }
    }
}
