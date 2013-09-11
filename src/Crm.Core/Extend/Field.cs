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
    public abstract class Field
    {
        public Field(FieldNewInfo info)
        {
            this.ID = info.ID;
            this.Name = info.Name;
            this.Required = info.Required;
            this.CanModify = info.CanModify;
            this._code = info.Code;
            this.CanImport = info.CanImport;
            this.Index = info.Index;
            this.Type = info.Type;
            this.Form = info.Form;
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

        public abstract PropertyValueType ValueType { get; }

        public Form Form { private set; get; }

        public event TEventHanlder<Field, FieldModifyArgs> Modifying;
        public event TEventHanlder<Field, FieldModifyArgs> Modified;

        protected void OnModifying(FieldModifyArgs modifyInfo)
        {
            if (this.Modifying != null)
            {
                this.Modifying(this, modifyInfo);
            }
        }

        protected void OnModifyed(FieldModifyArgs modifyInfo)
        {
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

        public abstract FieldInfo Map();

        public void Fill(FieldInfo info)
        {
            info.Code = this.Code;
            info.FormType = this.Form.Type;
            info.ID = this.ID;
            info.CanModify = this.CanModify;
            info.Name = this.Name;
            info.Required = this.Required;
            info.Type = this.Type;
            info.CanImport = this.CanImport;
            info.Index = this.Index;
            info.ValueType = this.ValueType;
        }

        public static Field CreateField(FieldNewInfo newInfo, string config)
        {
            switch (newInfo.Type)
            {
                case FieldType.String:
                case FieldType.Text:
                case FieldType.System:
                    return new StringField(newInfo, config);
                case FieldType.DropdownList:
                case FieldType.RadioList:
                    ListFieldConfigModel listFieldConfig = JsonConvert.DeserializeObject<ListFieldConfigModel>(config);
                    return new ListField(newInfo, listFieldConfig.DefaultValue, listFieldConfig.SelectList);
                case FieldType.CheckboxList:
                    CheckboxFieldConfigModel checkboxFieldConfig = JsonConvert.DeserializeObject<CheckboxFieldConfigModel>(config);
                    return new CheckboxListField(newInfo, checkboxFieldConfig.DefaultValues, checkboxFieldConfig.SelectList);
                case FieldType.Number:
                    NumberFieldConfigModel numberFieldConfigModel = JsonConvert.DeserializeObject<NumberFieldConfigModel>(config);
                    return new NumberField(newInfo, numberFieldConfigModel.DefaultValue, numberFieldConfigModel.Max, numberFieldConfigModel.Min, numberFieldConfigModel.Precision);
                case FieldType.Date:
                    DateFieldConfigModel dateFieldConfigModel = JsonConvert.DeserializeObject<DateFieldConfigModel>(config);
                    return new DateField(newInfo, dateFieldConfigModel.DefaultValueIsToday);
            }
            throw new ArgumentException("type");
        }
    }
}
