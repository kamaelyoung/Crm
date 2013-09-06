using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Crm.Data;
using log4net.Util;

namespace Crm.Core
{
    public class CustomerFavoriteManager
    {
        CustomerManager _customerManger;
        OrganizationManagement _orgManger;
        Dictionary<User, List<Customer>> _userFavoriteDic;
        Dictionary<Customer, Customer> _bindedEventCustomerDic;
        ReaderWriterLock _lock;

        public CustomerFavoriteManager(CustomerManager customerManger, OrganizationManagement orgManger)
        {
            this._customerManger = customerManger;
            this._orgManger = orgManger;
            this._userFavoriteDic = new Dictionary<User, List<Customer>>();
            this._bindedEventCustomerDic = new Dictionary<Customer, Customer>();
            this._lock = new ReaderWriterLock();

            this.Load();
        }

        public void Favorite(User user, Customer customer)
        {
            this._lock.AcquireWriterLock();
            try
            {
                if (!this._userFavoriteDic.ContainsKey(user))
                {
                    this._userFavoriteDic.Add(user, new List<Customer>());
                }
                if (this._userFavoriteDic[user].Contains(customer))
                {
                    return;
                }
                CustomerFavoriteModel model = new CustomerFavoriteModel { CustomerID = customer.ID, UserID = user.ID };
                NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();

                this._userFavoriteDic[user].Add(customer);
                this._userFavoriteDic[user] = this._userFavoriteDic[user].OrderByDescending(x => x.CreateTime).ToList();
                this.BindCustomerEvent(customer);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private void BindCustomerEvent(Customer customer)
        {
            if (!this._bindedEventCustomerDic.ContainsKey(customer))
            {
                this._bindedEventCustomerDic.Add(customer, customer);
                customer.Deleted += new TEventHanlder<Customer, User>(Customer_Deleted);
            }
        }

        void Customer_Deleted(Customer customer, User opUser)
        {
            this._lock.AcquireWriterLock();
            try
            {

                IList<CustomerFavoriteModel> models = NHibernateHelper.CurrentSession.QueryOver<CustomerFavoriteModel>()
                    .Where(x => x.CustomerID == customer.ID).List();
                foreach (CustomerFavoriteModel model in models)
                {
                    NHibernateHelper.CurrentSession.Delete(model);
                    NHibernateHelper.CurrentSession.Flush();
                }

                foreach (KeyValuePair<User, List<Customer>> pair in _userFavoriteDic)
                {
                    pair.Value.Remove(customer);
                }
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public void CancelFavorite(User user, Customer customer)
        {
            this._lock.AcquireWriterLock();
            try
            {
                if (!this._userFavoriteDic.ContainsKey(user))
                {
                    return;
                }
                if (!this._userFavoriteDic[user].Contains(customer))
                {
                    return;
                }
                CustomerFavoriteModel model = NHibernateHelper.CurrentSession.QueryOver<CustomerFavoriteModel>()
                    .Where(x => x.CustomerID == customer.ID && x.UserID == user.ID)
                    .SingleOrDefault();
                NHibernateHelper.CurrentSession.Delete(model);
                NHibernateHelper.CurrentSession.Flush();

                this._userFavoriteDic[user].Remove(customer);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public List<Customer> GetFavorites(User user, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock();
            try
            {
                totalCount = 0;
                if (!this._userFavoriteDic.ContainsKey(user))
                {
                    return new List<Customer>();
                }
                totalCount = this._userFavoriteDic[user].Count;
                return this._userFavoriteDic[user].Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Customer> GetFavorites(User user)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (!this._userFavoriteDic.ContainsKey(user))
                {
                    return new List<Customer>();
                }
                return this._userFavoriteDic[user].ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private void Load()
        {
            IList<CustomerFavoriteModel> models = NHibernateHelper.CurrentSession.QueryOver<CustomerFavoriteModel>().List();
            foreach (CustomerFavoriteModel model in models)
            {
                Customer customer = this._customerManger.GetCustomerById(model.CustomerID);
                User user = this._orgManger.UserManager.GetUserById(model.UserID);

                if (!this._userFavoriteDic.ContainsKey(user))
                {
                    this._userFavoriteDic.Add(user, new List<Customer>());
                }

                this._userFavoriteDic[user].Add(customer);
                this.BindCustomerEvent(customer);
            }
            Dictionary<User, List<Customer>> userFavoriteDic = new Dictionary<User,List<Customer>>();
            foreach (KeyValuePair<User, List<Customer>> pair in this._userFavoriteDic)
            {
                userFavoriteDic.Add(pair.Key, pair.Value.OrderByDescending(x => x.CreateTime).ToList());
            }
            this._userFavoriteDic = userFavoriteDic;
        }
    }
}
