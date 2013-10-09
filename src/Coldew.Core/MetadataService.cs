using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class MetadataService : IMetadataService
    {
        ColdewManager _coldewManager;
        public MetadataService(ColdewManager coldewManager)
        {
            this._coldewManager = coldewManager;
        }

        public List<MetadataInfo> GetMetadatas(string formId, string account)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            return form.MetadataManager.GetList(user)
                .Select(x => x.Map())
                .ToList();
        }

        public List<MetadataInfo> GetMetadatas(string formId, string account, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            return form.MetadataManager
                .GetList(user, skipCount, takeCount, out totalCount)
                .Select(x => x.Map())
                .ToList();
        }

        public List<MetadataInfo> GetMetadatas(string formId, string gridViewId, string account)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            GridView view = this._coldewManager.GridViewManager.GetGridView(gridViewId);
            return form.MetadataManager.Search(user, view.Searcher)
                .Select(x => x.Map())
                .ToList();
        }

        public List<MetadataInfo> GetMetadatas(string formId, string gridViewId, string account, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            GridView view = this._coldewManager.GridViewManager.GetGridView(gridViewId);
            return form.MetadataManager
                .Search(user, view.Searcher, skipCount, takeCount, out totalCount)
                .Select(x => x.Map())
                .ToList();
        }

        public MetadataInfo Create(string formId, string opUserAccount, PropertySettingDictionary propertys)
        {
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            User opUser = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);

            Metadata customer = form.MetadataManager.Create(opUser, propertys);
            return customer.Map();
        }

        public void Modify(string formId, string opUserAccount, string customerId, PropertySettingDictionary propertys)
        {
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            User opUser = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Metadata customer = form.MetadataManager.GetById(customerId);
            customer.SetPropertys(propertys);
        }

        public void Delete(string formId, string opUserAccount, List<string> customerIds)
        {
            if (customerIds == null)
            {
                return;
            }
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            foreach (string customerId in customerIds)
            {
                User opUser = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
                Metadata customer = form.MetadataManager.GetById(customerId);
                customer.Delete(opUser);
            }
        }

        public void Favorite(string formId, string opUserAccount, List<string> customerIds)
        {
            if (customerIds == null)
            {
                return;
            }
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            foreach (string customerId in customerIds)
            {
                User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
                Metadata customer = form.MetadataManager.GetById(customerId);

                form.MetadataManager.FavoriteManager.Favorite(user, customer);
            }
        }

        public void CancelFavorite(string formId, string opUserAccount, List<string> customerIds)
        {
            if (customerIds == null)
            {
                return;
            }
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            foreach (string customerId in customerIds)
            {
                User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
                Metadata customer = form.MetadataManager.GetById(customerId);

                form.MetadataManager.FavoriteManager.CancelFavorite(user, customer);
            }
        }

        public List<MetadataInfo> GetFavorites(string formId, string account, int skipCount, int takeCount, out int totalCount)
        {
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            return form.MetadataManager.FavoriteManager.GetFavorites(user, skipCount, takeCount, out totalCount).Select(x => x.Map()).ToList();
        }

        public List<MetadataInfo> GetFavorites(string formId, string account)
        {
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            return form.MetadataManager.FavoriteManager.GetFavorites(user).Select(x => x.Map()).ToList();
        }

        public MetadataInfo GetMetadataById(string formId, string id)
        {
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            Metadata customer = form.MetadataManager.GetById(id);
            if (customer != null)
            {
                return customer.Map();
            }
            return null;
        }

        public List<MetadataInfo> Search(string formId, string account, string serachExpressionJson, int skipCount, int takeCount, out int totalCount)
        {
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            List<Metadata> customers = form.MetadataManager.Search(user, MetadataSearcher.Parse(serachExpressionJson, form), skipCount, takeCount, out totalCount);
            return customers.Select(x => x.Map()).ToList();
        }

        public List<MetadataInfo> Search(string formId, string account, string serachExpressionJson)
        {
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(account);
            List<Metadata> customers = form.MetadataManager.Search(user, MetadataSearcher.Parse(serachExpressionJson, form));
            return customers.Select(x => x.Map()).ToList();
        }
    }
}
