using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Core.Organization;

namespace Crm.Core
{
    public class CustomerService : ICustomerService
    {
        CrmManager _crmManager;
        public CustomerService(CrmManager crmManager)
        {
            this._crmManager = crmManager;
        }

        public List<CustomerInfo> GetCustomers(string account)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            return this._crmManager.CustomerManager
                .GetCustomers(user)
                .Select(x => x.Map())
                .ToList();
        }

        public List<CustomerInfo> GetCustomers(string account, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            return this._crmManager.CustomerManager
                .GetCustomers(user, skipCount, takeCount, out totalCount)
                .Select(x => x.Map())
                .ToList();
        }

        public CustomerInfo GetCustomerById(string id)
        {
            Customer customer = this._crmManager.CustomerManager.GetCustomerById(id);
            if (customer != null)
            {
                return customer.Map();
            }
            return null;
        }

        public CustomerInfo Create(string opUserAccount, string name, int areaId, List<string> salesAccounts, List<PropertyOperationInfo> propertys)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            List<User> salesUsers = new List<User>();
            if (salesAccounts != null)
            {
                foreach (string account in salesAccounts)
                {
                    User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
                    salesUsers.Add(user);
                }
            }
            CustomerArea area = this._crmManager.AreaManager.GetAreaById(areaId);

            Customer customer = this._crmManager.CustomerManager.Create(new CustomerCreateInfo { OpUser = opUser, Name = name, Area = area, SalesUsers = salesUsers, PropertyInfos = propertys });
            return customer.Map();
        }

        public void Modify(string opUserAccount, string customerId, string name, int areaId, List<string> salesAccounts, List<PropertyOperationInfo> propertys)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            List<User> salesUsers = new List<User>();
            if (salesAccounts != null)
            {
                foreach (string account in salesAccounts)
                {
                    User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
                    salesUsers.Add(user);
                }
            }
            CustomerArea area = this._crmManager.AreaManager.GetAreaById(areaId);
            Customer customer = this._crmManager.CustomerManager.GetCustomerById(customerId);
            customer.Modify(new CustomerModifyInfo { OpUser = opUser, Name = name, Area = area, SalesUsers = salesUsers, PropertyInfos = propertys });
        }

        public void Delete(string opUserAccount, List<string> customerIds)
        {
            if (customerIds == null)
            {
                return;
            }
            foreach (string customerId in customerIds)
            {
                User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
                Customer customer = this._crmManager.CustomerManager.GetCustomerById(customerId);
                customer.Delete(opUser);
            }
        }

        public List<CustomerInfo> Search(string account, CustomerSearchInfo serachInfo, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            List<Customer> customers = this._crmManager.CustomerManager.Search(user, new CustomerSearcher(serachInfo), skipCount, takeCount, out totalCount);
            return customers.Select(x => x.Map()).ToList();
        }

        public List<CustomerInfo> Search(string account, CustomerSearchInfo serachInfo)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            List<Customer> customers = this._crmManager.CustomerManager.Search(user, new CustomerSearcher(serachInfo));
            return customers.Select(x => x.Map()).ToList();
        }

        public void Favorite(string opUserAccount, List<string> customerIds)
        {
            if (customerIds == null)
            {
                return;
            }
            foreach (string customerId in customerIds)
            {
                User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
                Customer customer = this._crmManager.CustomerManager.GetCustomerById(customerId);

                this._crmManager.FavoriteManager.Favorite(user, customer);
            }
        }

        public void CancelFavorite(string opUserAccount, List<string> customerIds)
        {
            if (customerIds == null)
            {
                return;
            }
            foreach (string customerId in customerIds)
            {
                User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
                Customer customer = this._crmManager.CustomerManager.GetCustomerById(customerId);

                this._crmManager.FavoriteManager.CancelFavorite(user, customer);
            }
        }

        public List<CustomerInfo> GetFavorites(string account, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            return this._crmManager.FavoriteManager.GetFavorites(user, skipCount, takeCount, out totalCount).Select(x => x.Map()).ToList();
        }

        public List<CustomerInfo> GetFavorites(string account)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            return this._crmManager.FavoriteManager.GetFavorites(user).Select(x => x.Map()).ToList();
        }
    }
}
