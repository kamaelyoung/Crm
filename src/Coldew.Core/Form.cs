﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Coldew.Api;
using Newtonsoft.Json;
using log4net.Util;
using Coldew.Core.Organization;
using Coldew.Api.Exceptions;

namespace Coldew.Core
{
    public class Form
    {
        ReaderWriterLock _lock;
        private List<Field> _fields;
        protected ColdewManager _coldewManager;

        public Form(string id, string code, string name, ColdewManager coldewManager)
        {
            this.ID = id;
            this.Name = name;
            this.Code = code;
            this._fields = new List<Field>();
            this._lock = new ReaderWriterLock();
            this._coldewManager = coldewManager;
            this.MetadataManager = this.CreateMetadataManager(coldewManager);
        }

        protected virtual MetadataManager CreateMetadataManager(ColdewManager coldewManager)
        {
            return new MetadataManager(this, coldewManager.OrgManager);
        }

        public string ID { set; get; }

        public string Code { set; get; }

        public string Name { set; get; }

        public MetadataManager MetadataManager { private set; get; }

        public StringField NameField
        {
            get
            {
                return this.GetFieldByCode(FormConstCode.FIELD_NAME_NAME) as StringField;
            }
        }

        public UserField CreatorField
        {
            get
            {
                return this.GetFieldByCode(FormConstCode.FIELD_NAME_CREATOR) as UserField;
            }
        }

        public DateField CreateTimeField
        {
            get
            {
                return this.GetFieldByCode(FormConstCode.FIELD_NAME_CREATE_TIME) as DateField;
            }
        }

        public Field CreateStringField(string code, string name, bool required, bool canModify, bool canInput, int index, string defaultValue)
        {
            return this.CreateField(code, name, required, canModify, canInput, index, FieldType.String, defaultValue);
        }

        public Field CreateTextField(string code, string name, bool required, bool canModify, bool canInput, int index, string defaultValue)
        {
            return this.CreateField(code, name, required, canModify, canInput, index, FieldType.Text, defaultValue);
        }

        public Field CreateDateField(string code, string name, bool required, bool canModify, bool canInput, int index, bool defaultValueIsToday)
        {
            DateFieldConfigModel configModel = new DateFieldConfigModel { DefaultValueIsToday = defaultValueIsToday };
            return this.CreateField(code, name, required, canModify, canInput, index, FieldType.Date, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateNumberField(string code, string name, bool required, bool canModify, bool canInput, int index, decimal? defaultValue, decimal? max, decimal? min, int precision)
        {
            NumberFieldConfigModel configModel = new NumberFieldConfigModel { DefaultValue = defaultValue, Max = max, Min = min, Precision = precision };
            return this.CreateField(code, name, required, canModify, canInput, index, FieldType.Number, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateCheckboxListField(string code, string name, bool required, bool canModify, bool canInput, int index, List<string> defaultValues, List<string> selectList)
        {
            CheckboxFieldConfigModel configModel = new CheckboxFieldConfigModel { DefaultValues = defaultValues, SelectList = selectList };
            return this.CreateField(code, name, required, canModify, canInput, index, FieldType.CheckboxList, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateRadioListField(string code, string name, bool required, bool canModify, bool canInput, int index, string defaultValue, List<string> selectList)
        {
            ListFieldConfigModel configModel = new ListFieldConfigModel { DefaultValue = defaultValue, SelectList = selectList };
            return this.CreateField(code, name, required, canModify, canInput, index, FieldType.RadioList, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateDropdownField(string code, string name, bool required, bool canModify, bool canInput, int index, string defaultValue, List<string> selectList)
        {
            ListFieldConfigModel configModel = new ListFieldConfigModel { DefaultValue = defaultValue, SelectList = selectList };
            return this.CreateField(code, name, required, canModify, canInput, index, FieldType.DropdownList, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateUserField(string code, string name, bool required, bool canModify, bool canInput, int index, bool defaultValueIsCurrent)
        {
            UserFieldConfigModel configModel = new UserFieldConfigModel { defaultValueIsCurrent = defaultValueIsCurrent};
            return this.CreateField(code, name, required, canModify, canInput, index, FieldType.User, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateUserListField(string code, string name, bool required, bool canModify, bool canInput, int index, bool defaultValueIsCurrent)
        {
            UserFieldConfigModel configModel = new UserFieldConfigModel { defaultValueIsCurrent = defaultValueIsCurrent};
            return this.CreateField(code, name, required, canModify, canInput, index, FieldType.UserList, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateMetadataField(string code, string name, bool required, bool canModify, bool canInput, int index, string formCode)
        {
            MetadataFieldConfigModel configModel = new MetadataFieldConfigModel { FormCode = formCode};
            return this.CreateField(code, name, required, canModify, canInput, index, FieldType.Metadata, JsonConvert.SerializeObject(configModel));
        }

        protected Field CreateField(string code, string name, bool required, bool canModify, bool canInput, int index, string type, string config)
        {
            this._lock.AcquireWriterLock();
            try
            {
                if (this._fields.Any(x => x.Name == name))
                {
                    throw new FieldNameRepeatException();
                } 
                
                if (!string.IsNullOrEmpty(code) && this._fields.Any(x => x.Code == code))
                {
                    throw new FieldCodeRepeatException();
                }

                FieldModel model = new FieldModel
                {
                    FormId = this.ID,
                    Code = code,
                    Name = name,
                    Required = required,
                    Type = type,
                    CanModify = canModify,
                    CanInput = canInput,
                    Index = index,
                    Config = config
                };
                model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();

                Field field = this.CreateField(model);
                this.BindEvent(field);
                this._fields.Add(field);
                this._fields = this._fields.OrderBy(x => x.Index).ToList();
                return field;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public virtual Field CreateField(FieldModel model)
        {
            FieldNewInfo newInfo = new FieldNewInfo(model.ID, model.Code, model.Name, model.Required, model.CanModify, model.Type, model.CanInput, model.Index, this);
            switch (newInfo.Type)
            {
                case FieldType.String:
                    return new StringField(newInfo, model.Config);
                case FieldType.Text:
                    return new TextField(newInfo, model.Config);
                case FieldType.DropdownList:
                    ListFieldConfigModel dropdownFieldConfig = JsonConvert.DeserializeObject<ListFieldConfigModel>(model.Config);
                    return new DropdownField(newInfo, dropdownFieldConfig.DefaultValue, dropdownFieldConfig.SelectList);
                case FieldType.RadioList:
                    ListFieldConfigModel listFieldConfig = JsonConvert.DeserializeObject<ListFieldConfigModel>(model.Config);
                    return new RadioListField(newInfo, listFieldConfig.DefaultValue, listFieldConfig.SelectList);
                case FieldType.CheckboxList:
                    CheckboxFieldConfigModel checkboxFieldConfig = JsonConvert.DeserializeObject<CheckboxFieldConfigModel>(model.Config);
                    return new CheckboxListField(newInfo, checkboxFieldConfig.DefaultValues, checkboxFieldConfig.SelectList);
                case FieldType.Number:
                    NumberFieldConfigModel numberFieldConfigModel = JsonConvert.DeserializeObject<NumberFieldConfigModel>(model.Config);
                    return new NumberField(newInfo, numberFieldConfigModel.DefaultValue, numberFieldConfigModel.Max, numberFieldConfigModel.Min, numberFieldConfigModel.Precision);
                case FieldType.Date:
                    DateFieldConfigModel dateFieldConfigModel = JsonConvert.DeserializeObject<DateFieldConfigModel>(model.Config);
                    return new DateField(newInfo, dateFieldConfigModel.DefaultValueIsToday);
                case FieldType.User:
                    UserFieldConfigModel userFieldConfigModel = JsonConvert.DeserializeObject<UserFieldConfigModel>(model.Config);
                    return new UserField(newInfo, userFieldConfigModel.defaultValueIsCurrent, this._coldewManager.OrgManager.UserManager);
                case FieldType.UserList:
                    UserFieldConfigModel userListFieldConfigModel = JsonConvert.DeserializeObject<UserFieldConfigModel>(model.Config);
                    return new UserListField(newInfo, userListFieldConfigModel.defaultValueIsCurrent, this._coldewManager.OrgManager.UserManager);
                case FieldType.Metadata:
                    MetadataFieldConfigModel metadataFieldConfigModel = JsonConvert.DeserializeObject<MetadataFieldConfigModel>(model.Config);
                    return new MetadataField(newInfo, this._coldewManager.FormManager.GetFormByCode(metadataFieldConfigModel.FormCode));
            }
            throw new ArgumentException("type");
        }

        public int GetFieldMaxIndex()
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._fields.Max(x => x.Index);
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private void BindEvent(Field field)
        {
            field.Modifying += new TEventHandler<Field, FieldModifyArgs>(Field_Modifying);
            field.Deleted += new TEventHandler<Field, User>(Field_Deleted);
        }

        void Field_Modifying(Field sender, FieldModifyArgs args)
        {
            if (sender.Name != args.Name && this._fields.Any(x => x.Name == args.Name))
            {
                throw new FieldNameRepeatException();
            }
        }

        void Field_Deleted(Field field, User args)
        {
            this._lock.AcquireWriterLock();
            try
            {
                this._fields.Remove(field);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public List<Field> GetFields()
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._fields.ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Field> GetRequiredFields()
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._fields.Where(x => x.Required).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public Field GetFieldById(int fieldId)
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._fields.Find(x => x.ID == fieldId);
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public Field GetFieldByCode(string fieldCode)
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._fields.Find(x => x.Code == fieldCode);
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public FormInfo Map()
        {
            return new FormInfo
            {
                ID = this.ID,
                Name = this.Name
            };
        }

        internal void Load()
        {
            IList<FieldModel> models = NHibernateHelper.CurrentSession.QueryOver<FieldModel>().Where(x => x.FormId == this.ID).List();
            foreach (FieldModel model in models)
            {
                Field field = this.CreateField(model);
                this.BindEvent(field);
                this._fields.Add(field);
            }
            this._fields = this._fields.OrderBy(x => x.Index).ToList();
            this.MetadataManager.Load();
        }
    }
}
