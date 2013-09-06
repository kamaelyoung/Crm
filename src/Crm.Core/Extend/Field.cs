using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Newtonsoft.Json;
using Crm.Core.Organization;
using Crm.Data;

namespace Crm.Core.Extend
{
    public class Field
    {
        public Field(int id, string code, string name, bool required, bool canModify, 
            FieldType type, bool canImport, int index, FieldConfig config, Form form)
        {
            this.ID = id;
            this.Name = name;
            this.Required = required;
            this.CanModify = canModify;
            this._code = code;
            this.CanImport = canImport;
            this.Index = index;
            this.Type = type;
            this.Config = config;
            this.Form = form;
            this.Config.Field = this;
        }

        public int ID { set; get; }

        private string _code;
        public string Code
        {
            private set
            {
                this._code = value;
            }
            get 
            {
                if (string.IsNullOrEmpty(this._code))
                {
                    return "field" + this.ID;
                }
                return this._code;
            } 
        }

        public string Name { set; get; }

        public bool Required { set; get; }

        public bool CanModify { set; get; }

        public bool CanImport { set; get; }

        public int Index { set; get; }

        public FieldType Type { set; get; }

        public FieldConfig Config { set; get; }

        public Form Form { private set; get; }

        public PropertyValueType ValueType
        {
            get
            {
                return this.Config.ValueType;
            }
        }

        public event TEventHanlder<Field, FieldModifyInfo> Modifying;
        public event TEventHanlder<Field, FieldModifyInfo> Modified;

        public void Modify(FieldModifyInfo modifyInfo)
        {
            if (this.Modifying != null)
            {
                this.Modifying(this, modifyInfo);
            }

            FieldModel model = NHibernateHelper.CurrentSession.Get<FieldModel>(this.ID);
            model.Name = modifyInfo.Name;
            model.Required = modifyInfo.Required;
            model.Index = modifyInfo.Index;
            model.Config = modifyInfo.Config.PersistenceValue;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Name = modifyInfo.Name;
            this.Index = modifyInfo.Index;
            this.Required = modifyInfo.Required;
            this.Config = modifyInfo.Config;

            if (this.Modified != null)
            {
                this.Modified(this, modifyInfo);
            }
        }

        public event TEventHanlder<Field, User> Deleting;
        public event TEventHanlder<Field, User> Deleted;

        public void Delete(User opUser)
        {
            if (this.Deleting != null)
            {
                this.Deleting(this, opUser);
            }

            FieldModel model = NHibernateHelper.CurrentSession.Get<FieldModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();

            if (this.Deleted != null)
            {
                this.Deleted(this, opUser);
            }
        }

        public FieldInfo Map()
        {
            FieldInfo info = new FieldInfo();
            info.Code = this.Code;
            info.FormType = this.Form.Type;
            info.ID = this.ID;
            info.CanModify = this.CanModify;
            info.Name = this.Name;
            info.Required = this.Required;
            info.Type = this.Type;
            info.CanImport = this.CanImport;
            info.Index = this.Index;
            info.ValueType = this.Config.ValueType;
            info.ConfigInfo = this.Config.Map();
            return info;
        }
    }
}
