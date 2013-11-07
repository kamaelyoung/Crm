using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Newtonsoft.Json;
using Coldew.Core.Organization;
using Coldew.Api;
using System.Threading;
using Coldew.Api.Exceptions;
using Coldew.Core.DataServices;

namespace Coldew.Core
{
    public class MetadataManager
    {
        protected Dictionary<string, Metadata> _metadataDicById;
        protected Dictionary<string, Metadata> _metadataDicByName;
        protected List<Metadata> _metadataList;
        OrganizationManagement _orgManger;
        protected ReaderWriterLock _lock;
        protected MetadataDataService _dataService;

        public MetadataManager(ColdewObject cobject, MetadataDataService dataService, OrganizationManagement orgManger)
        {
            this._metadataDicById = new Dictionary<string, Metadata>();
            this._metadataDicByName = new Dictionary<string, Metadata>();
            this._metadataList = new List<Metadata>();
            this._orgManger = orgManger;
            this.ColdewObject = cobject;
            this._dataService = dataService;
            this._lock = new ReaderWriterLock();
            this.FavoriteManager = new MetadataFavoriteManager(this, orgManger);
            this.ColdewObject.FieldDeleted += new TEventHandler<Core.ColdewObject, Field>(ColdewObject_FieldDeleted);
        }

        void ColdewObject_FieldDeleted(ColdewObject sender, Field field)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                foreach (Metadata metadata in this._metadataList)
                {
                    metadata.RemoveFieldProperty(field);
                }
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public ColdewObject ColdewObject { private set; get; }

        public MetadataFavoriteManager FavoriteManager { private set; get; }

        protected virtual void OnCreating(User creator, PropertySettingDictionary dictionary)
        {

        }

        public Metadata Create(User creator, PropertySettingDictionary dictionary)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                this.OnCreating(creator, dictionary);
                dictionary.Add(ColdewObjectCode.FIELD_NAME_CREATOR, creator.Account);
                dictionary.Add(ColdewObjectCode.FIELD_NAME_CREATE_TIME, DateTime.Now.ToString("yyyy-MM-dd"));
                dictionary.Add(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, creator.Account);
                dictionary.Add(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, DateTime.Now.ToString("yyyy-MM-dd"));

                List<Field> requiredFields = this.ColdewObject.GetRequiredFields();
                foreach (Field field in requiredFields)
                {
                    if (!dictionary.ContainsKey(field.Code) || string.IsNullOrEmpty(dictionary[field.Code]))
                    {
                        throw new ColdewException(string.Format("必要字段{0}不能空", field.Name));
                    }
                }
                if (this._metadataDicByName.ContainsKey(dictionary[ColdewObjectCode.FIELD_NAME_NAME]))
                {
                    throw new FieldNameRepeatException();
                }

                List<MetadataProperty> propertys = MetadataPropertyListHelper.MapPropertys(dictionary, this.ColdewObject);
                Metadata metadata = this._dataService.Create(propertys);

                this._metadataDicById.Add(metadata.ID, metadata);
                this._metadataDicByName.Add(metadata.Name, metadata);
                this._metadataList.Insert(0, metadata);

                this.BindEvent(metadata);
                return metadata;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        protected virtual void BindEvent(Metadata metadata)
        {
            metadata.PropertyChanging += new TEventHandler<Metadata, PropertySettingDictionary>(Metadata_Changing);
            metadata.Deleted += new TEventHandler<Metadata, User>(Metadata_Deleted);
        }

        void Metadata_Changing(Metadata metadata, PropertySettingDictionary propertys)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                if (metadata.Name != metadata.Name && this._metadataDicByName.ContainsKey(metadata.Name))
                {
                    throw new FieldNameRepeatException();
                }
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        void Metadata_Deleted(Metadata customer, User opUser)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                this._metadataDicById.Remove(customer.ID);
                this._metadataDicByName.Remove(customer.Name);
                this._metadataList.Remove(customer );
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public List<Metadata> GetList(User user, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var metadatasEnumer = this._metadataList.Where(x => x.CanPreview(user));
                var metadatas = metadatasEnumer.OrderBy(orderBy).ToList();
                totalCount = metadatas.Count;
                return metadatas.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Metadata> GetList(User user, string orderBy)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var metadatasEnumer = this._metadataList.Where(x => x.CanPreview(user));
                return metadatasEnumer.OrderBy(orderBy).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Metadata> GetRelatedList(ColdewObject cObject, string metadataId, string orderBy)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var metadatasEnumer = this._metadataList.Where(x => {
                    MetadataProperty property = x.GetPropertyByObject(cObject);
                    if (property != null)
                    {
                        MetadataRelatedValue value = property.Value as MetadataRelatedValue;
                        return value.Metadata.ID == metadataId;
                    }
                    return false;
                });

                return metadatasEnumer.OrderBy(orderBy).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public Metadata GetById(string id)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                if (this._metadataDicById.ContainsKey(id))
                {
                    return this._metadataDicById[id];
                }
                return null;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Metadata> Search(User user, MetadataSearcher seracher, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var metadatasEnumer = this._metadataList.Where(x => x.CanPreview(user) && seracher.Accord(x));
                var metadatas = metadatasEnumer.OrderBy(orderBy).ToList();
                totalCount = metadatas.Count;
                return metadatas.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Metadata> Search(User user, MetadataSearcher seracher, string orderBy)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var metadatasEnumer = this._metadataList.Where(x => x.CanPreview(user) && seracher.Accord(x));
                return metadatasEnumer.OrderBy(orderBy).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        internal virtual void Load()
        {
            List<Metadata> metadatas = this._dataService.LoadFromDB();
            foreach (Metadata metadata in metadatas)
            {
                this._metadataDicById.Add(metadata.ID, metadata);
                this._metadataDicByName.Add(metadata.Name, metadata);
                this._metadataList.Add(metadata);

                this.BindEvent(metadata);
            }
        }
    }
}
