using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    public interface IMetadataService
    {
        List<MetadataInfo> GetMetadatas(string formId, string account);

        List<MetadataInfo> GetMetadatas(string formId, string account, int skipCount, int takeCount, out int totalCount);

        List<MetadataInfo> GetMetadatas(string formId, string gridViewId, string account);

        List<MetadataInfo> GetMetadatas(string formId, string gridViewId, string account, int skipCount, int takeCount, out int totalCount);

        MetadataInfo Create(string formId, string opUserAccount, PropertySettingDictionary propertys);

        void Modify(string formId, string opUserAccount, string customerId, PropertySettingDictionary propertys);

        void Delete(string formId, string opUserAccount, List<string> customerIds);

        void Favorite(string formId, string opUserAccount, List<string> customerIds);

        void CancelFavorite(string formId, string opUserAccount, List<string> customerIds);

        List<MetadataInfo> GetFavorites(string formId, string account, int skipCount, int takeCount, out int totalCount);

        List<MetadataInfo> GetFavorites(string formId, string account);

        MetadataInfo GetMetadataById(string formId, string id);

        List<MetadataInfo> Search(string formId, string account, string serachExpressionJson, int skipCount, int takeCount, out int totalCount);

        List<MetadataInfo> Search(string formId, string account, string serachExpressionJson);
    }
}
