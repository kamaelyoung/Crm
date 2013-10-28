using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Coldew.Api;
using Coldew.Data;
using Coldew.Api.Exceptions;

namespace Coldew.Core
{
    public class Metadata
    {
        public Metadata(string id, List<MetadataProperty> propertys, ColdewObject form)
        {
            this.ID = id;
            this._propertys = propertys.ToDictionary(x => x.Field.Code);
            this.ColdewObject = form;
            this.InitPropertys();
        }

        private void InitPropertys()
        {
            List<MetadataProperty> virtualPropertys = this.GetVirtualPropertys();
            if (virtualPropertys != null)
            {
                foreach (MetadataProperty property in virtualPropertys)
                {
                    this._propertys.Add(property.Field.Code, property);
                }
            }
        }

        public void RemoveFieldProperty(Field field)
        {
            List<MetadataProperty> propertys = this._propertys.Values.ToList();
            MetadataProperty property = propertys.Find(x => x.Field == field);
            if (property != null)
            {
                propertys.Remove(property);

                this.UpdateDB(propertys);

                this._propertys = propertys.ToDictionary(x => x.Field.Code);
                this.BuildContent();
            }
        }

        protected virtual List<MetadataProperty> GetVirtualPropertys()
        {
            return null;
        }

        public string ID { private set; get; }

        public string Name
        {
            get
            {
                StringMetadataValue value = this.GetProperty(this.ColdewObject.NameField.Code).Value as StringMetadataValue;
                return value.String;
            }
        }

        public User Creator
        {
            get
            {
                UserMetadataValue value = this.GetProperty(this.ColdewObject.CreatorField.Code).Value as UserMetadataValue;
                return value.User;
            }
        }

        public DateTime CreateTime
        {
            get
            {
                DateMetadataValue value = this.GetProperty(this.ColdewObject.CreateTimeField.Code).Value as DateMetadataValue;
                return value.Date.Value;
            }
        }

        public ColdewObject ColdewObject { private set; get; }

        string _content;
        public string Content
        {
            private set
            {
                this._content = value;
            }
            get
            {
                if (string.IsNullOrEmpty(this._content))
                {
                    this.BuildContent();
                }
                return this._content;
            }
        }

        protected virtual void BuildContent()
        {
            StringBuilder sb = new StringBuilder();
            foreach (MetadataProperty property in this.GetPropertys())
            {
                if (!string.IsNullOrEmpty(property.Value.ShowValue))
                {
                    sb.Append(property.Value.ShowValue.ToLower());
                }
            }
            this.Content = sb.ToString();
        }

        protected Dictionary<string, MetadataProperty> _propertys;

        public event TEventHandler<Metadata, PropertySettingDictionary> PropertyChanging;
        public event TEventHandler<Metadata, PropertySettingDictionary> PropertyChanged;

        protected virtual void OnPropertyChanging(PropertySettingDictionary dictionary)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, dictionary);
            }
        }

        protected virtual void OnPropertyChanged(PropertySettingDictionary dictionary)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, dictionary);
            }
        }

        public virtual void SetPropertys(PropertySettingDictionary dictionary)
        {
            this.OnPropertyChanging(dictionary);

            List<MetadataProperty> modifyPropertys = MetadataPropertyListHelper.MapPropertys(dictionary, this.ColdewObject);

            foreach (MetadataProperty modifyproperty in modifyPropertys)
            {
                if (this._propertys.ContainsKey(modifyproperty.Field.Code))
                {
                    this._propertys[modifyproperty.Field.Code] = modifyproperty;
                }
                else
                {
                    this._propertys.Add(modifyproperty.Field.Code, modifyproperty);
                }
            }

            this.UpdateDB(this._propertys.Values.ToList());

            this.InitPropertys();
            this.BuildContent();

            this.OnPropertyChanged(dictionary);
        }

        protected virtual void UpdateDB(List<MetadataProperty> propertys)
        {
            MetadataModel model = NHibernateHelper.CurrentSession.Get<MetadataModel>(this.ID);
            model.PropertysJson = MetadataPropertyListHelper.ToPropertyModelJson(propertys);

            NHibernateHelper.CurrentSession.Update(model);
        }

        public virtual List<MetadataProperty> GetPropertys()
        {
            return this._propertys.Values.ToList();
        }

        public MetadataProperty GetProperty(string propertyCode)
        {
            if (this._propertys.ContainsKey(propertyCode))
            {
                return this._propertys[propertyCode];
            }
            return null;
        }

        public MetadataProperty GetPropertyByObject(ColdewObject cObject)
        {
            return this._propertys.Values.Where(x => {
                if (x.Field is MetadataField)
                {
                    MetadataField field = x.Field as MetadataField;
                    return field.ValueForm == cObject;
                }
                return false;
            }).FirstOrDefault();
        }

        public event TEventHandler<Metadata, User> Deleting;
        public event TEventHandler<Metadata, User> Deleted;

        public virtual void Delete(User opUser)
        {
            if (!this.CanDelete(opUser))
            {
                throw new ColdewException("没有权限删除该客户!");
            }

            if (this.Deleting != null)
            {
                this.Deleting(this, opUser);
            }

            this.DeleteDB();

            if (this.Deleted != null)
            {
                this.Deleted(this, opUser);
            }
        }

        protected virtual void DeleteDB()
        {
            MetadataModel model = NHibernateHelper.CurrentSession.Get<MetadataModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        public virtual bool CanPreview(User user)
        {
            if (user.Role == Api.Organization.UserRole.Administrator)
            {
                return true;
            }

            if (user == this.Creator)
            {
                return true;
            }

            if (this.Creator.IsMySuperior(user, true))
            {
                return true;
            }

            return false;
        }

        public virtual bool CanDelete(User user)
        {
            if (user.Role == Api.Organization.UserRole.Administrator)
            {
                return true;
            }

            if (user == this.Creator)
            {
                return true;
            }

            if (this.Creator.IsMySuperior(user, true))
            {
                return true;
            }

            return false;
        }

        internal void Delete()
        {
            MetadataModel model = NHibernateHelper.CurrentSession.Get<MetadataModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        public virtual MetadataInfo Map()
        {
            return new MetadataInfo()
            {
                ID = this.ID,
                Name = this.Name,
                Propertys = this._propertys.Values.Select(x => x.Map()).ToList()
            };
        }
    }
}
