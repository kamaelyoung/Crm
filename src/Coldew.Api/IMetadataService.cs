using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    public interface IMetadataService
    {
        List<MetadataInfo> GetMetadatas(string objectId, string account, int skipCount, int takeCount, string orderBy, out int totalCount);

        List<MetadataInfo> GetMetadatas(string objectId, string gridViewId, string account, string orderBy);

        List<MetadataInfo> GetMetadatas(string objectId, string gridViewId, string account, int skipCount, int takeCount, string orderBy, out int totalCount);

        MetadataInfo Create(string objectId, string opUserAccount, string propertyJson);

        void Modify(string objectId, string opUserAccount, string customerId, string propertyJson);

        void Delete(string objectId, string opUserAccount, List<string> metadataIds);

        void ToggleFavorite(string objectId, string opUserAccount, List<string> metadataIds);

        MetadataInfo GetMetadataById(string userAccount, string objectId, string id);

        List<MetadataInfo> Search(string objectId, string account, string serachExpressionJson, int skipCount, int takeCount, string orderBy, out int totalCount);

        List<MetadataInfo> Search(string objectId, string gridViewId, string account, string serachExpressionJson, int skipCount, int takeCount, string orderBy, out int totalCount);

        List<MetadataInfo> Search(string objectId, string gridViewId, string account, string serachExpressionJson, string orderBy);

        List<MetadataInfo> GetRelatedMetadatas(string userAccount, string relatedObjectId, string objectId, string metadataId, string orderBy);
    }
}
