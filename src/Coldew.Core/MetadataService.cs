using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core.Organization;
using Coldew.Core.Search;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Coldew.Core
{
    public class MetadataService : IMetadataService
    {
        ColdewManager _coldewManager;
        public MetadataService(ColdewManager coldewManager)
        {
            this._coldewManager = coldewManager;
        }

        public List<UserMetadataInfo> GetMetadatas(string objectId, string account, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            ColdewObject cobject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            return cobject.MetadataManager
                .GetList(user, skipCount, takeCount, orderBy, out totalCount)
                .Select(x => x.Map(user))
                .ToList();
        }

        public List<MetadataInfo> GetMetadatas(string objectId, string gridViewId, string account, string orderBy)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            ColdewObject cobject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            GridView view = cobject.GridViewManager.GetGridView(gridViewId);
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = view.OrderFieldCode;
            }

            List<MetadataSearcher> searchers = new List<MetadataSearcher>();
            if (view.Searcher != null)
            {
                searchers.Add(view.Searcher);
            }
            return cobject.MetadataManager.Search(user, searchers, orderBy)
                .Select(x => x.Map())
                .ToList();
        }

        public List<UserMetadataInfo> GetMetadatas(string objectId, string gridViewId, string account, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            ColdewObject cobject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            GridView view = cobject.GridViewManager.GetGridView(gridViewId);
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = view.OrderFieldCode;
            }

            List<MetadataSearcher> searchers = new List<MetadataSearcher>();
            if (view.Searcher != null)
            {
                searchers.Add(view.Searcher);
            }
            return cobject.MetadataManager
                .Search(user, searchers, skipCount, takeCount, orderBy, out totalCount)
                .Select(x => x.Map(user))
                .ToList();
        }

        public MetadataInfo Create(string objectId, string opUserAccount, string propertyJson)
        {
            ColdewObject cobject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            User opUser = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);

            Metadata metadata = cobject.MetadataManager.Create(opUser, JsonConvert.DeserializeObject<JObject>(propertyJson));
            return metadata.Map();
        }

        public void Modify(string objectId, string opUserAccount, string metadataId, string propertyJson)
        {
            ColdewObject cobject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            User opUser = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Metadata metadata = cobject.MetadataManager.GetById(metadataId);
            metadata.SetPropertys(opUser, JsonConvert.DeserializeObject<JObject>(propertyJson));
        }

        public void Delete(string objectId, string opUserAccount, List<string> metadataIds)
        {
            if (metadataIds == null)
            {
                return;
            }
            ColdewObject cobject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            foreach (string metadataId in metadataIds)
            {
                User opUser = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
                Metadata metadata = cobject.MetadataManager.GetById(metadataId);
                metadata.Delete(opUser);
            }
        }

        public void ToggleFavorite(string objectId, string opUserAccount, List<string> metadataIds)
        {
            if (metadataIds == null)
            {
                return;
            }
            ColdewObject cobject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            foreach (string metadataId in metadataIds)
            {
                User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
                Metadata metadata = cobject.MetadataManager.GetById(metadataId);

                if (cobject.FavoriteManager.IsFavorite(user, metadata))
                {
                    cobject.FavoriteManager.CancelFavorite(user, metadata);
                }
                else
                {
                    cobject.FavoriteManager.Favorite(user, metadata);
                }
            }
        }

        public MetadataInfo GetMetadataById(string objectId, string id)
        {
            ColdewObject cobject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            Metadata metadata = cobject.MetadataManager.GetById(id);
            if (metadata != null)
            {
                return metadata.Map();
            }
            return null;
        }

        public List<MetadataInfo> GetRelatedMetadatas(string relatedObjectId, string objectId, string metadataId, string orderBy)
        {
            ColdewObject relatedObject = this._coldewManager.ObjectManager.GetObjectById(relatedObjectId);
            ColdewObject cObject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            return relatedObject.MetadataManager.GetRelatedList(cObject, metadataId, orderBy).Select(x => x.Map()).ToList();
        }

        public List<UserMetadataInfo> Search(string objectId, string account, string serachExpressionJson, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            ColdewObject cobject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            MetadataExpressionSearcher searcher = MetadataExpressionSearcher.Parse(serachExpressionJson, cobject);

            List<MetadataSearcher> searchers = new List<MetadataSearcher>();
            searchers.Add(searcher);
            List<Metadata> customers = cobject.MetadataManager.Search(user, searchers, skipCount, takeCount, orderBy, out totalCount);
            return customers.Select(x => x.Map(user)).ToList();
        }

        public List<UserMetadataInfo> Search(string objectId, string gridViewId, string account, string serachExpressionJson, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            ColdewObject cobject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            MetadataExpressionSearcher searcher = MetadataExpressionSearcher.Parse(serachExpressionJson, cobject);
            GridView view = cobject.GridViewManager.GetGridView(gridViewId);
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = view.OrderFieldCode;
            }
            
            List<MetadataSearcher> searchers = new List<MetadataSearcher>();
            searchers.Add(searcher);
            if(view.Searcher != null)
            {
                searchers.Add(view.Searcher);
            }
            List<Metadata> customers = cobject.MetadataManager.Search(user, searchers, skipCount, takeCount, orderBy, out totalCount);
            return customers.Select(x => x.Map(user)).ToList();
        }

        public List<MetadataInfo> Search(string objectId, string gridViewId, string account, string serachExpressionJson, string orderBy)
        {
            ColdewObject cobject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            MetadataExpressionSearcher searcher = MetadataExpressionSearcher.Parse(serachExpressionJson, cobject);
            GridView view = cobject.GridViewManager.GetGridView(gridViewId);
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = view.OrderFieldCode;
            }

            List<MetadataSearcher> searchers = new List<MetadataSearcher>();
            searchers.Add(searcher);
            if (view.Searcher != null)
            {
                searchers.Add(view.Searcher);
            }
            List<Metadata> customers = cobject.MetadataManager.Search(user, searchers, orderBy);
            return customers.Select(x => x.Map()).ToList();
        }
    }
}
