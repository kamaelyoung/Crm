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
            return this.CreateField(code, name, required, true, canImport, index, FieldType.System, FieldConfig.CreateFieldConfig(FieldType.System, ""));
        }

        public Field CreateField(string code, string name, bool required, bool isSystem, bool canImport, int index, FieldType type, FieldConfig config)
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
                    Config = config.PersistenceValue
                };
                model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();

                Field field = new Field(model.ID, model.Code, model.Name, model.Required, model.CanModify, type, model.CanImport, model.Index, config, this);
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
            field.Modifying += new TEventHanlder<Field, FieldModifyInfo>(Field_Modifying);
            field.Deleted += new TEventHanlder<Field, User>(Field_Deleted);
        }

        void Field_Modifying(Field sender, FieldModifyInfo args)
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
                FieldConfig config = FieldConfig.CreateFieldConfig(type, model.Config);
                Field field = new Field(model.ID, model.Code, model.Name, model.Required, model.CanModify, type, model.CanImport, model.Index, config, this);
                this.BindEvent(field);
                this._fields.Add(field);
            }
            this._fields = this._fields.OrderBy(x => x.Index).ToList();
        }
    }
}
