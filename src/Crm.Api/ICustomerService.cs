using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    public interface ICustomerService
    {
        List<MetadataInfo> GetCustomers(string account);

        List<MetadataInfo> GetCustomers(string account, int skipCount, int takeCount, out int totalCount);

        MetadataInfo Create(string opUserAccount, PropertySettingDictionary propertys);

        void Modify(string opUserAccount, string customerId, PropertySettingDictionary propertys);

        void Delete(string opUserAccount, List<string> customerIds);

        void Favorite(string opUserAccount, List<string> customerIds);

        void CancelFavorite(string opUserAccount, List<string> customerIds);

        List<MetadataInfo> GetFavorites(string account, int skipCount, int takeCount, out int totalCount);

        List<MetadataInfo> GetFavorites(string account);

        MetadataInfo GetCustomerById(string id);

        List<MetadataInfo> Search(string account, MetadataSearchInfo serachInfo, int skipCount, int takeCount, out int totalCount);

        List<MetadataInfo> Search(string account, MetadataSearchInfo serachInfo);
    }
}
