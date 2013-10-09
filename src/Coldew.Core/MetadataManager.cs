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

namespace Coldew.Core
{
    public class MetadataManager
    {
        protected Dictionary<string, Metadata> _metadataDicById;
        protected Dictionary<string, Metadata> _metadataDicByName;
        protected List<Metadata> _metadataList;
        OrganizationManagement _orgManger;
        protected ReaderWriterLock _lock;

        public MetadataManager(Form form, OrganizationManagement orgManger)
        {
            this._metadataDicById = new Dictionary<string, Metadata>();
            this._metadataDicByName = new Dictionary<string, Metadata>();
            this._metadataList = new List<Metadata>();
            this._orgManger = orgManger;
            this.Form = form;
            this._lock = new ReaderWriterLock();
            this.FavoriteManager = new MetadataFavoriteManager(this, orgManger);
        }

        public Form Form { private set; get; }

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
                dictionary.Add(FormConstCode.FIELD_NAME_CREATOR, creator.Account);
                dictionary.Add(FormConstCode.FIELD_NAME_CREATE_TIME, DateTime.Now.ToString("yyyy-MM-dd"));
                dictionary.Add(FormConstCode.FIELD_NAME_MODIFIED_USER, creator.Account);
                dictionary.Add(FormConstCode.FIELD_NAME_MODIFIED_TIME, DateTime.Now.ToString("yyyy-MM-dd"));

                List<Field> requiredFields = this.Form.GetRequiredFields();
                foreach (Field field in requiredFields)
                {
                    if (!dictionary.ContainsKey(field.Code) || string.IsNullOrEmpty(dictionary[field.Code]))
                    {
                        throw new ColdewException(string.Format("必要字段{0}不能空", field.Name));
                    }
                }
                if (this._metadataDicByName.ContainsKey(dictionary[FormConstCode.FIELD_NAME_NAME]))
                {
                    throw new FieldNameRepeatException();
                }

                MetadataPropertyList propertys = MetadataPropertyList.MapPropertys(dictionary, this.Form);
                Metadata metadata = this.CreateAndSaveDB(propertys);

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

        protected virtual Metadata CreateAndSaveDB(MetadataPropertyList propertys)
        {
            MetadataModel model = new MetadataModel();
            model.PropertysJson = propertys.ToJson();
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            Metadata metadata = new Metadata(model.ID, propertys, this.Form);
            return metadata;
        }

        protected virtual void BindEvent(Metadata metadata)
        {
            metadata.PropertyChanged += new TEventHandler<Metadata, PropertySettingDictionary>(Metadata_Modified);
            metadata.Deleted += new TEventHandler<Metadata, User>(Metadata_Deleted);
        }

        void Metadata_Modified(Metadata customer, PropertySettingDictionary propertys)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                if (customer.Name != customer.Name && this._metadataDicByName.ContainsKey(customer.Name))
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

        public List<Metadata> GetList(User user, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var customers = this._metadataList.Where(x => x.CanPreview(user)).ToList().ToList();
                totalCount = customers.Count;
                return customers.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Metadata> GetList(User user)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                return this._metadataList.Where(x => x.CanPreview(user)).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private List<Metadata> OrderBy(string code, IEnumerable<Metadata> customers)
        {
            if (string.IsNullOrEmpty(code))
            {
                code = "name";
            }
            List<Metadata> orderByCodeCustomers = new List<Metadata>();
            IEnumerable<Metadata> canOrderByCodeCusomters = customers.Where(x => x.GetProperty(code) != null);
            orderByCodeCustomers.AddRange(canOrderByCodeCusomters.OrderBy(x =>
            {
                MetadataProperty property = x.GetProperty(code);
                return property.Value.OrderValue;

            }).ToList());
            orderByCodeCustomers.AddRange(customers.Where(x => x.GetProperty(code) == null));
            return orderByCodeCustomers;
        }

        private List<Metadata> OrderByDescending(string code, IEnumerable<Metadata> customers)
        {
            if (string.IsNullOrEmpty(code))
            {
                code = "name";
            }
            List<Metadata> orderByCodeCustomers = new List<Metadata>();
            IEnumerable<Metadata> canOrderByCodeCusomters = customers.Where(x => x.GetProperty(code) != null);
            orderByCodeCustomers.AddRange(canOrderByCodeCusomters.OrderByDescending(x =>
            {
                MetadataProperty property = x.GetProperty(code);
                return property.Value.OrderValue;

            }).ToList());
            orderByCodeCustomers.AddRange(customers.Where(x => x.GetProperty(code) == null));
            return orderByCodeCustomers;
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

        public List<Metadata> Search(User user, MetadataSearcher seracher, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var searchCustomers = this._metadataList.Where(x => x.CanPreview(user) && seracher.Accord(x)).ToList();
                totalCount = searchCustomers.Count;
                return searchCustomers.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Metadata> Search(User user, MetadataSearcher seracher)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                return this._metadataList.Where(x => x.CanPreview(user) && seracher.Accord(x)).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        protected virtual List<Metadata> LoadFromDB()
        {
            List<Metadata> metadatas = new List<Metadata>();

            IList<MetadataModel> models = NHibernateHelper.CurrentSession.QueryOver<MetadataModel>().List();
            foreach (MetadataModel model in models)
            {
                Metadata metadata = new Metadata(model.ID, MetadataPropertyList.GetPropertys(model.PropertysJson, this.Form), this.Form);

                metadatas.Add(metadata);
            }
            return metadatas;
        }

        internal virtual void Load()
        {
            List<Metadata> metadatas = this.LoadFromDB();
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
