using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core.Organization;
using Coldew.Core.Search;

namespace Coldew.Core
{
    public class MetadataService : IMetadataService
    {
        ColdewManager _coldewManager;
        public MetadataService(ColdewManager coldewManager)
        {
            this._coldewManager = coldewManager;
        }

        public List<MetadataInfo> GetMetadatas(string objectId, string account, string orderBy)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            return form.MetadataManager.GetList(user, orderBy)
                .Select(x => x.Map())
                .ToList();
        }

        public List<MetadataInfo> GetMetadatas(string objectId, string account, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            return form.MetadataManager
                .GetList(user, skipCount, takeCount, orderBy, out totalCount)
                .Select(x => x.Map())
                .ToList();
        }

        public List<MetadataInfo> GetMetadatas(string objectId, string gridViewId, string account, string orderBy)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            GridView view = form.GridViewManager.GetGridView(gridViewId);
            return form.MetadataManager.Search(user, view.Searcher, orderBy)
                .Select(x => x.Map())
                .ToList();
        }

        public List<MetadataInfo> GetMetadatas(string objectId, string gridViewId, string account, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            GridView view = form.GridViewManager.GetGridView(gridViewId);
            return form.MetadataManager
                .Search(user, view.Searcher, skipCount, takeCount, orderBy, out totalCount)
                .Select(x => x.Map())
                .ToList();
        }

        public MetadataInfo Create(string objectId, string opUserAccount, PropertySettingDictionary propertys)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            User opUser = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);

            Metadata customer = form.MetadataManager.Create(opUser, propertys);
            return customer.Map();
        }

        public void Modify(string objectId, string opUserAccount, string customerId, PropertySettingDictionary propertys)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            User opUser = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Metadata customer = form.MetadataManager.GetById(customerId);
            customer.SetPropertys(propertys);
        }

        public void Delete(string objectId, string opUserAccount, List<string> customerIds)
        {
            if (customerIds == null)
            {
                return;
            }
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            foreach (string customerId in customerIds)
            {
                User opUser = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
                Metadata customer = form.MetadataManager.GetById(customerId);
                customer.Delete(opUser);
            }
        }

        public void Favorite(string objectId, string opUserAccount, List<string> customerIds)
        {
            if (customerIds == null)
            {
                return;
            }
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            foreach (string customerId in customerIds)
            {
                User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
                Metadata customer = form.MetadataManager.GetById(customerId);

                form.MetadataManager.FavoriteManager.Favorite(user, customer);
            }
        }

        public void CancelFavorite(string objectId, string opUserAccount, List<string> customerIds)
        {
            if (customerIds == null)
            {
                return;
            }
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            foreach (string customerId in customerIds)
            {
                User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
                Metadata customer = form.MetadataManager.GetById(customerId);

                form.MetadataManager.FavoriteManager.CancelFavorite(user, customer);
            }
        }

        public List<MetadataInfo> GetFavorites(string objectId, string account, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            return form.MetadataManager.FavoriteManager.GetFavorites(user, skipCount, takeCount, orderBy, out totalCount).Select(x => x.Map()).ToList();
        }

        public List<MetadataInfo> GetFavorites(string objectId, string account, string orderBy)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            return form.MetadataManager.FavoriteManager.GetFavorites(user, orderBy).Select(x => x.Map()).ToList();
        }

        public MetadataInfo GetMetadataById(string objectId, string id)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            Metadata customer = form.MetadataManager.GetById(id);
            if (customer != null)
            {
                return customer.Map();
            }
            return null;
        }

        public List<MetadataInfo> GetRelatedMetadatas(string relatedObjectId, string objectId, string metadataId, string orderBy)
        {
            ColdewObject relatedObject = this._coldewManager.ObjectManager.GetFormById(relatedObjectId);
            ColdewObject cObject = this._coldewManager.ObjectManager.GetFormById(objectId);
            return relatedObject.MetadataManager.GetRelatedList(cObject, metadataId, orderBy).Select(x => x.Map()).ToList();
        }

        public List<MetadataInfo> Search(string objectId, string account, string serachExpressionJson, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            List<Metadata> customers = form.MetadataManager.Search(user, MetadataExpressionSearcher.Parse(serachExpressionJson, form), skipCount, takeCount, orderBy, out totalCount);
            return customers.Select(x => x.Map()).ToList();
        }

        public List<MetadataInfo> Search(string objectId, string account, string serachExpressionJson, string orderBy)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            List<Metadata> customers = form.MetadataManager.Search(user, MetadataExpressionSearcher.Parse(serachExpressionJson, form), orderBy);
            return customers.Select(x => x.Map()).ToList();
        }
    }
}
