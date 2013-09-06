using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    public interface ICustomerService
    {
        List<CustomerInfo> GetCustomers(string account);

        List<CustomerInfo> GetCustomers(string account, int skipCount, int takeCount, out int totalCount);

        CustomerInfo Create(string opUserAccount, string name, int areaId, List<string> salesAccounts, List<PropertyOperationInfo> propertys);

        void Modify(string opUserAccount, string customerId, string name, int areaId, List<string> salesAccounts, List<PropertyOperationInfo> propertys);

        void Delete(string opUserAccount, List<string> customerIds);

        void Favorite(string opUserAccount, List<string> customerIds);

        void CancelFavorite(string opUserAccount, List<string> customerIds);

        List<CustomerInfo> GetFavorites(string account, int skipCount, int takeCount, out int totalCount);

        List<CustomerInfo> GetFavorites(string account);

        CustomerInfo GetCustomerById(string id);

        List<CustomerInfo> Search(string account, List<string> keywords, int skipCount, int takeCount, out int totalCount);

        List<CustomerInfo> Search(string account, List<string> keywords);
    }
}
