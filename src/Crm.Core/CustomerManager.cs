using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Crm.Data;
using Crm.Api;
using Crm.Core.Extend;
using System.Text.RegularExpressions;
using log4net.Util;
using Crm.Api.Exceptions;

namespace Crm.Core
{
    public class CustomerManager
    {
        private Dictionary<string, Customer> _customerDicById;
        private Dictionary<string, Customer> _customerDicByName;
        private List<Customer> _customerList;
        CustomerAreaManager _areaManager;
        OrganizationManagement _orgManger;
        Form _form;
        ReaderWriterLock _lock;

        public CustomerManager(CustomerAreaManager areaManager, OrganizationManagement orgManger, FormManager formManager)
        {
            this._customerDicById = new Dictionary<string, Customer>();
            this._customerDicByName = new Dictionary<string, Customer>();
            this._customerList = new List<Customer>();
            this._areaManager = areaManager;
            this._orgManger = orgManger; 
            this._form = formManager.GetForm(FormType.Customer);
            this._lock = new ReaderWriterLock();
            areaManager.CustomerAreaDeleting += new TEventHanlder<CustomerArea>(CustomerArea_Deleting);
            this.Load();
        }

        void CustomerArea_Deleting(CustomerArea args)
        {
            List<Customer> customers = this.GetCustomerByArea(args);
            if (customers.Count > 0)
            {
                throw new CustomerAreaDeleteException(string.Format("无法删除该区域，该区域下有{0}个客户", customers.Count));
            }
        }

        public Customer Create(CustomerCreateInfo info)
        {
            this._lock.AcquireWriterLock();
            try
            {
                if (this._customerDicByName.ContainsKey(info.Name))
                {
                    throw new CustomerNameRepeatException();
                }
                if (info.SalesUsers == null || info.SalesUsers.Count == 0)
                {
                    throw new CustomerSalesUserNullException();
                }
                Metadata metadata = Metadata.CreateMetadata(info.PropertyInfos, this._form);

                CustomerModel model = new CustomerModel();
                model.AreaId = info.Area.ID;
                model.CreateTime = DateTime.Now;
                model.CreatorId = info.OpUser.ID;
                model.ModifiedTime = DateTime.Now;
                model.ModifiedUserId = info.OpUser.ID;
                model.Name = info.Name;
                model.SalesUserIds = string.Join(",", info.SalesUsers.Select(x => x.ID).ToArray());
                model.MetadataId = metadata.ID;

                model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
                NHibernateHelper.CurrentSession.Flush();
                Customer customer = new Customer(model.ID, model.Name, info.Area, info.SalesUsers, info.OpUser, model.CreateTime, info.OpUser, model.CreateTime, metadata);

                this._customerDicById.Add(customer.ID, customer);
                this._customerDicByName.Add(customer.Name, customer);
                this._customerList.Insert(0, customer);

                this.BindEvent(customer);
                return customer;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private void BindEvent(Customer customer)
        {
            customer.Modified += new TEventHanlder<Customer, CustomerModifyInfo>(Customer_Modified);
            customer.Deleted += new TEventHanlder<Customer, User>(Customer_Deleted);
        }

        void Customer_Modified(Customer customer, CustomerModifyInfo info)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (info.Name != customer.Name && this._customerDicByName.ContainsKey(info.Name))
                {
                    throw new CustomerNameRepeatException();
                }
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        void Customer_Deleted(Customer customer, User opUser)
        {
            this._lock.AcquireWriterLock();
            try
            {
                this._customerDicById.Remove(customer.ID);
                this._customerDicByName.Remove(customer.Name);
                this._customerList.Remove(customer);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public List<Customer> GetCustomers(User user, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock();
            try
            {
                var customers = this._customerList.Where(x => x.CanPreview(user)).ToList().ToList();
                totalCount = customers.Count;
                return customers.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Customer> GetCustomers(User user)
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._customerList.Where(x => x.CanPreview(user)).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private List<Customer> OrderBy(string code, IEnumerable<Customer> customers)
        {
            if (string.IsNullOrEmpty(code))
            {
                code = "name";
            }

            switch (code)
            {
                case "name":
                    return customers.OrderBy(x => x.Name).ToList();
                case "area":
                    return customers.OrderBy(x => x.Area.Name).ToList();
                case "salesUsers":
                    return customers.OrderBy(x => string.Join(",", x.SalesUsers.Select(u => u.Name))).ToList();
                case "creator":
                    return customers.OrderBy(x => x.Creator.Name).ToList();
                case "createTime":
                    return customers.OrderBy(x => x.CreateTime).ToList();
            }
            List<Customer> orderByCodeCustomers = new List<Customer>();
            IEnumerable<Customer> canOrderByCodeCusomters = customers.Where(x => x.Metadata.GetProperty(code) != null);
            orderByCodeCustomers.AddRange(canOrderByCodeCusomters.OrderBy(x =>
            {
                MetadataProperty property = x.Metadata.GetProperty(code);
                return property.Value.OrderValue;
            
            }).ToList());
            orderByCodeCustomers.AddRange(customers.Where(x => x.Metadata.GetProperty(code) == null));
            return orderByCodeCustomers;
        }

        private List<Customer> OrderByDescending(string code, IEnumerable<Customer> customers)
        {
            if (string.IsNullOrEmpty(code))
            {
                code = "name";
            }
            switch (code)
            {
                case "name":
                    return customers.OrderByDescending(x => x.Name).ToList();
                case "area":
                    return customers.OrderByDescending(x => x.Area.Name).ToList();
                case "salesUsers":
                    return customers.OrderByDescending(x => string.Join(",", x.SalesUsers.Select(u => u.Name))).ToList();
                case "creator":
                    return customers.OrderByDescending(x => x.Creator.Name).ToList();
                case "createTime":
                    return customers.OrderByDescending(x => x.CreateTime).ToList();
            }
            List<Customer> orderByCodeCustomers = new List<Customer>();
            IEnumerable<Customer> canOrderByCodeCusomters = customers.Where(x => x.Metadata.GetProperty(code) != null);
            orderByCodeCustomers.AddRange(canOrderByCodeCusomters.OrderByDescending(x =>
            {
                MetadataProperty property = x.Metadata.GetProperty(code);
                return property.Value.OrderValue;

            }).ToList());
            orderByCodeCustomers.AddRange(customers.Where(x => x.Metadata.GetProperty(code) == null));
            return orderByCodeCustomers;
        }

        public Customer GetCustomerById(string id)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (this._customerDicById.ContainsKey(id))
                {
                    return this._customerDicById[id];
                }
                return null;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Customer> GetCustomerByArea(CustomerArea area)
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._customerList.Where(x => x.Area == area).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Customer> Search(User user, List<string> keywords, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (keywords == null || keywords.Count == 0)
                {
                    return this.GetCustomers(user, skipCount, takeCount, out totalCount);
                }

                List<Regex> regexs = keywords.Select(x => new Regex(x.ToLower())).ToList();
                var searchCustomers = this._customerList.Where(x => regexs.All(regex => regex.IsMatch(x.Content))).Where(x => x.CanPreview(user)).ToList();
                totalCount = searchCustomers.Count;
                return searchCustomers.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Customer> Search(User user, List<string> keywords)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (keywords == null || keywords.Count == 0)
                {
                    return this.GetCustomers(user);
                }

                List<Regex> regexs = keywords.Select(x => new Regex(x.ToLower())).ToList();
                return this._customerList.Where(x => regexs.All(regex => regex.IsMatch(x.Content))).Where(x => x.CanPreview(user)).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private void Load()
        {
            IList<CustomerModel> models = NHibernateHelper.CurrentSession.QueryOver<CustomerModel>().OrderBy(x => x.CreateTime).Desc.List();
            foreach (CustomerModel model in models)
            {
                CustomerArea area = this._areaManager.GetAreaById(model.AreaId);
                List<User> salesUsers = new List<User>();
                string[] ids = model.SalesUserIds.Split(',');
                foreach (string userId in ids)
                {
                    salesUsers.Add(this._orgManger.UserManager.GetUserById(userId));
                }
                User creator = this._orgManger.UserManager.GetUserById(model.CreatorId);
                User modifiedUser = this._orgManger.UserManager.GetUserById(model.ModifiedUserId);
                Metadata metadata = Metadata.LoadMetadata(model.MetadataId, this._form);
                Customer customer = new Customer(model.ID, model.Name, area, salesUsers, creator, model.CreateTime, modifiedUser, model.CreateTime, metadata);

                this._customerDicById.Add(customer.ID, customer);
                this._customerDicByName.Add(customer.Name, customer);
                this._customerList.Add(customer);

                this.BindEvent(customer);
            }
        }
    }
}
