using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data;
using Crm.Api;
using Newtonsoft.Json;
using log4net.Util;
using Crm.Core.Organization;
using Crm.Api.Exceptions;

namespace Crm.Core.Extend
{
    public class Form
    {
        ReaderWriterLock _lock;
        private List<Field> _fields;

        public Form(int id, FormType type, string name)
        {
            this.ID = id;
            this.Type = type;
            this.Name = name;
            this._fields = new List<Field>();
            this._lock = new ReaderWriterLock();
        }

        public int ID { set; get; }

        public FormType Type { set; get; }

        public string Name { set; get; }

        public Field CreateSystemField(string code, string name, bool required, bool canImport, int index)
        {
            return this.CreateField(code, name, required, true, canImport, index, FieldType.System, "");
        }

        public Field CreateStringField(string code, string name, bool required, bool isSystem, bool canImport, int index, string defaultValue)
        {
            return this.CreateField(code, name, required, isSystem, canImport, index, FieldType.String, defaultValue);
        }

        public Field CreateTextField(string code, string name, bool required, bool isSystem, bool canImport, int index, string defaultValue)
        {
            return this.CreateField(code, name, required, isSystem, canImport, index, FieldType.Text, defaultValue);
        }

        public Field CreateDateField(string code, string name, bool required, bool isSystem, bool canImport, int index, bool defaultValueIsToday)
        {
            DateFieldConfigModel configModel = new DateFieldConfigModel { DefaultValueIsToday = defaultValueIsToday };
            return this.CreateField(code, name, required, isSystem, canImport, index, FieldType.Date, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateNumberField(string code, string name, bool required, bool isSystem, bool canImport, int index, decimal? defaultValue, decimal? max, decimal? min, int precision)
        {
            NumberFieldConfigModel configModel = new NumberFieldConfigModel { DefaultValue = defaultValue, Max = max, Min = min, Precision = precision };
            return this.CreateField(code, name, required, isSystem, canImport, index, FieldType.Number, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateCheckboxListField(string code, string name, bool required, bool isSystem, bool canImport, int index, List<string> defaultValues, List<string> selectList)
        {
            CheckboxFieldConfigModel configModel = new CheckboxFieldConfigModel { DefaultValues = defaultValues, SelectList = selectList };
            return this.CreateField(code, name, required, isSystem, canImport, index, FieldType.CheckboxList, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateRadioListField(string code, string name, bool required, bool isSystem, bool canImport, int index, string defaultValue, List<string> selectList)
        {
            ListFieldConfigModel configModel = new ListFieldConfigModel { DefaultValue = defaultValue, SelectList = selectList };
            return this.CreateField(code, name, required, isSystem, canImport, index, FieldType.RadioList, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateDropdownField(string code, string name, bool required, bool isSystem, bool canImport, int index, string defaultValue, List<string> selectList)
        {
            ListFieldConfigModel configModel = new ListFieldConfigModel { DefaultValue = defaultValue, SelectList = selectList };
            return this.CreateField(code, name, required, isSystem, canImport, index, FieldType.DropdownList, JsonConvert.SerializeObject(configModel));
        }

        private Field CreateField(string code, string name, bool required, bool isSystem, bool canImport, int index, FieldType type, string config)
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
                    Type = (int)type,
                    CanModify = isSystem,
                    CanImport = canImport,
                    Index = index,
                    Config = config
                };
                model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();

                Field field = Field.CreateField(new FieldNewInfo(model.ID, model.Code, model.Name, model.Required, model.CanModify, type, model.CanImport, model.Index, this), model.Config);
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
            field.Modifying += new TEventHanlder<Field, FieldModifyArgs>(Field_Modifying);
            field.Deleted += new TEventHanlder<Field, User>(Field_Deleted);
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
                Type = this.Type,
                Name = this.Name
            };
        }

        internal void Load()
        {
            IList<FieldModel> models = NHibernateHelper.CurrentSession.QueryOver<FieldModel>().Where(x => x.FormId == this.ID).List();
            foreach (FieldModel model in models)
            {
                FieldType type = (FieldType)model.Type;
                Field field = Field.CreateField(new FieldNewInfo(model.ID, model.Code, model.Name, model.Required, model.CanModify, type, model.CanImport, model.Index, this), model.Config);
                this.BindEvent(field);
                this._fields.Add(field);
            }
            this._fields = this._fields.OrderBy(x => x.Index).ToList();
        }
    }
}
