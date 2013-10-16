using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Coldew.Data;
using Coldew.Core;
using System.Threading;

namespace Coldew.Core
{
    public class MetadataFavoriteManager
    {
        MetadataManager _metadataManager;
        OrganizationManagement _orgManger;
        Dictionary<User, List<Metadata>> _userFavoriteDic;
        Dictionary<Metadata, Metadata> _bindedEventDic;
        ReaderWriterLock _lock;

        public MetadataFavoriteManager(MetadataManager metadataManager, OrganizationManagement orgManger)
        {
            this._metadataManager = metadataManager;
            this._orgManger = orgManger;
            this._userFavoriteDic = new Dictionary<User, List<Metadata>>();
            this._bindedEventDic = new Dictionary<Metadata, Metadata>();
            this._lock = new ReaderWriterLock();

            this.Load();
        }

        public void Favorite(User user, Metadata metadata)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                if (!this._userFavoriteDic.ContainsKey(user))
                {
                    this._userFavoriteDic.Add(user, new List<Metadata>());
                }
                if (this._userFavoriteDic[user].Contains(metadata))
                {
                    return;
                }
                MetadataFavoriteModel model = new MetadataFavoriteModel { MetadataId = metadata.ID, UserId = user.ID, FormId = this._metadataManager.ColdewObject.ID };
                NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();

                this._userFavoriteDic[user].Add(metadata);
                this._userFavoriteDic[user] = this._userFavoriteDic[user].OrderByDescending(x => x.CreateTime).ToList();
                this.BindCustomerEvent(metadata);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private void BindCustomerEvent(Metadata metadata)
        {
            if (!this._bindedEventDic.ContainsKey(metadata))
            {
                this._bindedEventDic.Add(metadata, metadata);
                metadata.Deleted += new TEventHandler<Metadata, User>(Metadata_Deleted);
            }
        }

        void Metadata_Deleted(Metadata metadata, User opUser)
        {
            this._lock.AcquireWriterLock(0);
            try
            {

                IList<MetadataFavoriteModel> models = NHibernateHelper.CurrentSession.QueryOver<MetadataFavoriteModel>()
                    .Where(x => x.MetadataId == metadata.ID).List();
                foreach (MetadataFavoriteModel model in models)
                {
                    NHibernateHelper.CurrentSession.Delete(model);
                    NHibernateHelper.CurrentSession.Flush();
                }

                foreach (KeyValuePair<User, List<Metadata>> pair in _userFavoriteDic)
                {
                    pair.Value.Remove(metadata);
                }
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public void CancelFavorite(User user, Metadata metadata)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                if (!this._userFavoriteDic.ContainsKey(user))
                {
                    return;
                }
                if (!this._userFavoriteDic[user].Contains(metadata))
                {
                    return;
                }
                MetadataFavoriteModel model = NHibernateHelper.CurrentSession.QueryOver<MetadataFavoriteModel>()
                    .Where(x => x.MetadataId == metadata.ID && x.UserId == user.ID)
                    .SingleOrDefault();
                NHibernateHelper.CurrentSession.Delete(model);
                NHibernateHelper.CurrentSession.Flush();

                this._userFavoriteDic[user].Remove(metadata);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public List<Metadata> GetFavorites(User user, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                totalCount = 0;
                if (!this._userFavoriteDic.ContainsKey(user))
                {
                    return new List<Metadata>();
                }
                totalCount = this._userFavoriteDic[user].Count;
                return this._userFavoriteDic[user].OrderBy(orderBy).Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Metadata> GetFavorites(User user, string orderBy)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                if (!this._userFavoriteDic.ContainsKey(user))
                {
                    return new List<Metadata>();
                }
                return this._userFavoriteDic[user].OrderBy(orderBy).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private void Load()
        {
            IList<MetadataFavoriteModel> models = NHibernateHelper.CurrentSession.QueryOver<MetadataFavoriteModel>().Where(x => x.FormId == this._metadataManager.ColdewObject.ID).List();
            foreach (MetadataFavoriteModel model in models)
            {
                Metadata metadata = this._metadataManager.GetById(model.MetadataId);
                User user = this._orgManger.UserManager.GetUserById(model.UserId);

                if (!this._userFavoriteDic.ContainsKey(user))
                {
                    this._userFavoriteDic.Add(user, new List<Metadata>());
                }

                this._userFavoriteDic[user].Add(metadata);
                this.BindCustomerEvent(metadata);
            }
            Dictionary<User, List<Metadata>> userFavoriteDic = new Dictionary<User, List<Metadata>>();
            foreach (KeyValuePair<User, List<Metadata>> pair in this._userFavoriteDic)
            {
                userFavoriteDic.Add(pair.Key, pair.Value.OrderByDescending(x => x.CreateTime).ToList());
            }
            this._userFavoriteDic = userFavoriteDic;
        }
    }
}
