using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Crm.Api;
using Crm.Data;
using Crm.Api.Exceptions;

namespace Crm.Core.Extend
{
    public class Metadata
    {
        public Metadata(int id, List<MetadataProperty> propertys, Form form)
        {
            this.ID = id;
            this._propertys = propertys.ToDictionary(x => x.Field.Code);
            this.Form = form;
        }

        public int ID { private set; get; }

        public Form Form { private set; get; }

        private Dictionary<string, MetadataProperty> _propertys;

        public void SetPropertys(List<PropertyOperationInfo> propertyInfos)
        {
            List<MetadataProperty> propertys = new List<MetadataProperty>();
            foreach (PropertyOperationInfo propertyInfo in propertyInfos)
            {
                Field field = this.Form.GetFieldByCode(propertyInfo.Code);
                MetadataValue metadataValue = MetadataValue.Create(field.ValueType, propertyInfo.Value);
                if (metadataValue != null)
                {
                    propertys.Add(new MetadataProperty(field, metadataValue));
                }
            }

            MetadataModel model = NHibernateHelper.CurrentSession.Get<MetadataModel>(this.ID);
            List<MetadataPropertyModel> propertyModels = propertys.Select(x => new MetadataPropertyModel { FieldId = x.Field.ID, Value = x.Value.PersistenceValue }).ToList();
            model.PropertysJson = JsonConvert.SerializeObject(propertyModels);

            NHibernateHelper.CurrentSession.Update(model);

            this._propertys = propertys.ToDictionary(x => x.Field.Code);
        }

        public List<MetadataProperty> GetPropertys()
        {
            return this._propertys.Values.ToList();
        }

        internal MetadataProperty GetProperty(string propertyCode)
        {
            if (this._propertys.ContainsKey(propertyCode))
            {
                return this._propertys[propertyCode];
            }
            return null;
        }

        internal void Delete()
        {
            MetadataModel model = NHibernateHelper.CurrentSession.Get<MetadataModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        public virtual MetadataInfo MapMetadataInfo()
        {
            return new MetadataInfo()
            {
                ID = this.ID,
                Propertys = this._propertys.Values.Select(x => x.Map()).ToList()
            };
        }

        public static Metadata LoadMetadata(int metadataId, Form form)
        {
            MetadataModel model = NHibernateHelper.CurrentSession
                .Get<MetadataModel>(metadataId);
            List<MetadataPropertyModel> propertyModels = JsonConvert.DeserializeObject<List<MetadataPropertyModel>>(model.PropertysJson);
            List<MetadataProperty> propertys = new List<MetadataProperty>();
            foreach (MetadataPropertyModel propertyModel in propertyModels)
            {
                Field field = form.GetFieldById(propertyModel.FieldId);
                if (field != null)
                {
                    MetadataValue metadataValue = MetadataValue.Create(field.ValueType, propertyModel.Value);
                    propertys.Add(new MetadataProperty(field, metadataValue));
                }
            }
            Metadata metadata = new Metadata(model.ID, propertys, form);

            return metadata;
        }

        public static Metadata CreateMetadata(List<PropertyOperationInfo> propertyInfos, Form form)
        {
            MetadataModel model = new MetadataModel();

            model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();

            Metadata metadata = new Metadata(model.ID, new List<MetadataProperty>(), form);

            metadata.SetPropertys(propertyInfos);
            return metadata;
        }
    }
}
