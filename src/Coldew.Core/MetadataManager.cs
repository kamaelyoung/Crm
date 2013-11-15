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
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public class MetadataManager
    {
        protected Dictionary<string, Metadata> _metadataDicById;
        protected List<Metadata> _metadataList;
        OrganizationManagement _orgManger;
        protected ReaderWriterLock _lock;

        public MetadataManager(ColdewObject cobject, OrganizationManagement orgManger)
        {
            this._metadataDicById = new Dictionary<string, Metadata>();
            this._metadataList = new List<Metadata>();
            this._orgManger = orgManger;
            this.ColdewObject = cobject;
            this._lock = new ReaderWriterLock();
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

        protected virtual void OnCreating(User creator, JObject jobject)
        {

        }

        public Metadata Create(User creator, JObject jobject)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                this.OnCreating(creator, jobject);
                
                jobject.Add(this.ColdewObject.CreatedUserField.Code , creator.Account);
                jobject.Add(this.ColdewObject.CreatedTimeField.Code, DateTime.Now);
                jobject.Add(this.ColdewObject.ModifiedUserField.Code, creator.Account);
                jobject.Add(this.ColdewObject.ModifiedTimeField.Code, DateTime.Now);

                List<Field> requiredFields = this.ColdewObject.GetRequiredFields();
                foreach (Field field in requiredFields)
                {
                    if (jobject[field.Code] == null || string.IsNullOrEmpty(jobject[field.Code].ToString()))
                    {
                        throw new ColdewException(string.Format("必要字段{0}不能空", field.Name));
                    }
                }

                List<MetadataProperty> propertys = MetadataPropertyListHelper.MapPropertys(jobject, this.ColdewObject);
                Metadata metadata = this.ColdewObject.DataService.Create(propertys);

                this._metadataDicById.Add(metadata.ID, metadata);
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
            metadata.PropertyChanging += new TEventHandler<Metadata, JObject>(Metadata_Changing);
            metadata.Deleted += new TEventHandler<Metadata, User>(Metadata_Deleted);
        }

        void Metadata_Changing(Metadata metadata, JObject propertys)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                
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

        public List<Metadata> Search(User user, List<MetadataSearcher> serachers, int skipCount, int takeCount, string orderBy, out int totalCount)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                List<Metadata> metadatas = null;
                if (serachers == null || serachers.Count == 0)
                {
                    metadatas = this._metadataList.Where(x => x.CanPreview(user)).OrderBy(orderBy).ToList();
                }
                else
                {
                    metadatas = this._metadataList.Where(x => x.CanPreview(user) && serachers.All(s =>s.Accord(user, x))).OrderBy(orderBy).ToList();
                }
                totalCount = metadatas.Count;
                return metadatas.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Metadata> Search(User user, List<MetadataSearcher> serachers, string orderBy)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                List<Metadata> metadatas = null;
                if (serachers == null || serachers.Count == 0)
                {
                    metadatas = this._metadataList.Where(x => x.CanPreview(user)).OrderBy(orderBy).ToList();
                }
                else
                {
                    metadatas = this._metadataList.Where(x => x.CanPreview(user) && serachers.All(s =>s.Accord(user, x))).OrderBy(orderBy).ToList();
                }
                return metadatas;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        internal virtual void Load()
        {
            List<Metadata> metadatas = this.ColdewObject.DataService.LoadFromDB();
            foreach (Metadata metadata in metadatas)
            {
                this._metadataDicById.Add(metadata.ID, metadata);
                this._metadataList.Add(metadata);

                this.BindEvent(metadata);
            }
        }
    }
}
