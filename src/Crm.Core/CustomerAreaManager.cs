using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data;
using log4net.Util;
using Crm.Api.Exceptions;
using Coldew.Core.Organization;
using Coldew.Core;
using Crm.Api;

namespace Crm.Core
{
    public class CustomerAreaManager
    {
        FormManager _formManager;
        OrganizationManagement _orgManager;
        ReaderWriterLock _lock;
        public CustomerAreaManager(OrganizationManagement orgManager, FormManager formManager)
        {
            this._orgManager = orgManager;
            this._formManager = formManager;
            this.Areas = new List<CustomerArea>();
            this._lock = new ReaderWriterLock();
        }

        public event TEventHandler<CustomerArea> CustomerAreaDeleting;

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
            area.Deleting += new TEventHandler<CustomerArea>(Area_Deleting);
            area.Deleted += new TEventHandler<CustomerArea>(Area_Deleted);
            area.Modifying += new TEventHandler<CustomerArea, CustomerAreaModifyInfo>(Area_Modifying);
            this.Areas.Add(area);
            this.Areas = this.Areas.OrderBy(x => x.Name).ToList();
            return area;
        }

        void Area_Deleting(CustomerArea args)
        {

            Form customerForm = this._formManager.GetFormByCode(CrmFormConstCode.FORM_CUSTOMER);
            CustomerManager customerManager = customerForm.MetadataManager as CustomerManager;

            int count = customerManager.GetAreaCustomerCount(args);

            if (count > 0)
            {
                throw new CustomerAreaDeleteException(string.Format("无法删除该区域，该区域下有{0}个客户", count));
            }

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

        internal void Load()
        {
            IList<CustomerAreaModel> models = NHibernateHelper.CurrentSession.QueryOver<CustomerAreaModel>().List();
            foreach (CustomerAreaModel model in models)
            {
                this.Create(model);
            }
        }
    }
}
