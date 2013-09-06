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
using Crm.Core.Extend;

namespace Crm.Core
{
    public class Metadata
    {
        Form _form;

        public Metadata(int id, List<MetadataProperty> propertys, Form form)
        {
            this.ID = id;
            this._propertys = propertys.ToDictionary(x => x.Code);
            this._form = form;
        }

        public int ID { private set; get; }

        private Dictionary<string, MetadataProperty> _propertys;

        public void SetPropertys(List<PropertyOperationInfo> propertyInfos)
        {
            List<MetadataProperty> propertys = new List<MetadataProperty>();
            foreach (PropertyOperationInfo propertyInfo in propertyInfos)
            {
                MetadataValue metadataValue = Metadata.CreateMetadataValue(propertyInfo.Code, propertyInfo.Value, this._form);
                if (metadataValue != null)
                {
                    propertys.Add(new MetadataProperty(propertyInfo.Code, metadataValue));
                }
            }

            MetadataModel model = NHibernateHelper.CurrentSession.Get<MetadataModel>(this.ID);
            List<MetadataPropertyModel> propertyModels = propertyInfos.Select(x => new MetadataPropertyModel { Code = x.Code, Value = x.Value }).ToList();
            model.PropertysJson = JsonConvert.SerializeObject(propertyModels);

            NHibernateHelper.CurrentSession.Update(model);

            this._propertys = propertys.ToDictionary(x => x.Code);
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

        public static MetadataValue CreateMetadataValue(string fieldCode, string value, Form form)
        {
            Field field = form.GetFieldByCode(fieldCode);
            if (field == null)
            {
                return null;
            }
            switch (field.ValueType)
            {
                case PropertyValueType.Number:
                    if (string.IsNullOrEmpty(value))
                    {
                        return new NumberMetadataValue(null);
                    }
                    decimal number;
                    if (decimal.TryParse(value, out number))
                    {
                        return new NumberMetadataValue(number);
                    }
                    else
                    {
                        throw new CrmException("创建NumberMetadataValue出错,value:" + value);
                    }
                case PropertyValueType.String:
                    return new StringMetadataValue(value);
                case PropertyValueType.StringList:
                    List<string> stringList = new List<string>();
                    if (!string.IsNullOrEmpty(value))
                    {
                        stringList = value.Split(',').ToList();
                    }
                    return new StringListMetadataValue(stringList);
            }

            throw new CrmException("创建NumberMetadataValue出错,找不到符合的valuetype:" + field.ValueType);
        }

        public static Metadata LoadMetadata(int metadataId, Form form)
        {
            MetadataModel model = NHibernateHelper.CurrentSession
                .Get<MetadataModel>(metadataId);
            List<MetadataPropertyModel> propertyModels = JsonConvert.DeserializeObject<List<MetadataPropertyModel>>(model.PropertysJson);
            List<MetadataProperty> propertys = new List<MetadataProperty>();
            foreach (MetadataPropertyModel propertyModel in propertyModels)
            {
                MetadataValue metadataValue = Metadata.CreateMetadataValue(propertyModel.Code, propertyModel.Value, form);
                if (metadataValue != null)
                {
                    propertys.Add(new MetadataProperty(propertyModel.Code, metadataValue));
                }
            }
            Metadata metadata = new Metadata(model.ID, propertys, form);

            return metadata;
        }

        public static Metadata CreateMetadata(List<PropertyOperationInfo> propertyInfos, Form form)
        {
            MetadataModel model = new MetadataModel();
            List<MetadataPropertyModel> propertyModels = propertyInfos.Select(x => new MetadataPropertyModel { Code = x.Code, Value = x.Value }).ToList();
            model.PropertysJson = JsonConvert.SerializeObject(propertyModels);

            model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();

            List<MetadataProperty> propertys = new List<MetadataProperty>();
            foreach (PropertyOperationInfo propertyInfo in propertyInfos)
            {
                MetadataValue metadataValue = Metadata.CreateMetadataValue(propertyInfo.Code, propertyInfo.Value, form);
                if (metadataValue != null)
                {
                    propertys.Add(new MetadataProperty(propertyInfo.Code, metadataValue));
                }
            }
            Metadata metadata = new Metadata(model.ID, propertys, form);

            return metadata;
        }
    }
}
