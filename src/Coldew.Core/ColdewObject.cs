using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Coldew.Api;
using Newtonsoft.Json;
using log4net.Util;
using Coldew.Core.Organization;
using Coldew.Api.Exceptions;
using Coldew.Core.UI;
using Coldew.Core.DataServices;
using Coldew.Core.Permission;

namespace Coldew.Core
{
    public class ColdewObject
    {
        public const string FIELD_NAME_NAME = "name";
        public const string FIELD_NAME_CREATOR = "creator";
        public const string FIELD_NAME_CREATE_TIME = "createTime";
        public const string FIELD_NAME_MODIFIED_USER = "modifiedUser";
        public const string FIELD_NAME_MODIFIED_TIME = "modifiedTime";

        ReaderWriterLock _lock;
        private List<Field> _fields;

        public ColdewObject(string id, string code, string name, ColdewObjectType type, bool isSystem, int index, ColdewManager coldewManager)
        {
            this.ID = id;
            this.Name = name;
            this.Code = code;
            this.Type = type;
            this.IsSystem = isSystem;
            this.Index = index;
            this._fields = new List<Field>();
            this._lock = new ReaderWriterLock();
            this.ColdewManager = coldewManager;
            this.MetadataManager = this.CreateMetadataManager(coldewManager);
            this.FavoriteManager = new MetadataFavoriteManager(this);
            this.GridViewManager = this.CreateGridViewManager(coldewManager);
            this.FormManager = this.CreateFormManager(coldewManager);
            this.DataService = this.CreateDataService();
            this.ObjectPermission = new ObjectPermissionManager(this);
            this.MetadataPermission = new MetadataPermissionManager(this);
            this.FieldPermission = new FieldPermissionManager(this);
        }

        public ColdewManager ColdewManager { private set; get; }

        protected virtual MetadataManager CreateMetadataManager(ColdewManager coldewManager)
        {
            return new MetadataManager(this, coldewManager.OrgManager);
        }

        protected virtual MetadataDataService CreateDataService()
        {
            return new MetadataDataService(this);
        }

        protected virtual FormManager CreateFormManager(ColdewManager coldewManager)
        {
            return new FormManager(coldewManager.ObjectManager, this);
        }

        protected virtual GridViewManager CreateGridViewManager(ColdewManager coldewManager)
        {
            return new GridViewManager(coldewManager.OrgManager, this);
        }

        public virtual ObjectPermissionManager ObjectPermission { private set; get; }

        public virtual MetadataPermissionManager MetadataPermission { private set; get; }

        public virtual FieldPermissionManager FieldPermission { private set; get; }

        public string ID { set; get; }

        public string Code { set; get; }

        public string Name { set; get; }

        public bool IsSystem { set; get; }

        public int Index { set; get; }

        public ColdewObjectType Type { set; get; }

        public MetadataManager MetadataManager { private set; get; }

        public MetadataFavoriteManager FavoriteManager { private set; get; }

        public GridViewManager GridViewManager { private set; get; }

        public FormManager FormManager { private set; get; }

        internal MetadataDataService DataService { private set; get; }

        public NameField NameField { private set; get; }

        public CreatedUserField CreatedUserField { private set; get; }

        public CreatedTimeField CreatedTimeField { private set; get; }

        public ModifiedUserField ModifiedUserField { private set; get; }

        public ModifiedTimeField ModifiedTimeField { private set; get; }

        internal void CreateSystemFields(string nameFieldName)
        {
            this.NameField = this.CreateField(new FieldCreateBaseInfo(FIELD_NAME_NAME, nameFieldName, "", true, true), FieldType.Name, "") as NameField;
            this.CreatedUserField = this.CreateField(new FieldCreateBaseInfo(FIELD_NAME_CREATOR, "创建人", "", true, true), FieldType.CreatedUser, "") as CreatedUserField;
            this.CreatedTimeField = this.CreateField(new FieldCreateBaseInfo(FIELD_NAME_CREATE_TIME, "创建时间", "", true, true), FieldType.CreatedTime, "") as CreatedTimeField;
            this.ModifiedUserField = this.CreateField(new FieldCreateBaseInfo(FIELD_NAME_MODIFIED_USER, "修改人", "", true, true), FieldType.ModifiedUser, "") as ModifiedUserField;
            this.ModifiedTimeField = this.CreateField(new FieldCreateBaseInfo(FIELD_NAME_MODIFIED_TIME, "修改时间", "", true, true), FieldType.ModifiedTime, "") as ModifiedTimeField;
        }

        public Field CreateStringField(FieldCreateBaseInfo baseInfo, string defaultValue)
        {
            return this.CreateField(baseInfo, FieldType.String, defaultValue);
        }

        public Field CreateJsonField(FieldCreateBaseInfo baseInfo)
        {
            return this.CreateField(baseInfo,FieldType.Json, "");
        }

        public Field CreateTextField(FieldCreateBaseInfo baseInfo,  string defaultValue)
        {
            return this.CreateField(baseInfo, FieldType.Text, defaultValue);
        }

        public Field CreateDateField(FieldCreateBaseInfo baseInfo,  bool defaultValueIsToday)
        {
            DateFieldConfigModel configModel = new DateFieldConfigModel { DefaultValueIsToday = defaultValueIsToday };
            return this.CreateField(baseInfo, FieldType.Date, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateNumberField(FieldCreateBaseInfo baseInfo,  decimal? defaultValue, decimal? max, decimal? min, int precision)
        {
            NumberFieldConfigModel configModel = new NumberFieldConfigModel { DefaultValue = defaultValue, Max = max, Min = min, Precision = precision };
            return this.CreateField(baseInfo, FieldType.Number, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateCheckboxListField(FieldCreateBaseInfo baseInfo,  List<string> defaultValues, List<string> selectList)
        {
            CheckboxFieldConfigModel configModel = new CheckboxFieldConfigModel { DefaultValues = defaultValues, SelectList = selectList };
            return this.CreateField(baseInfo, FieldType.CheckboxList, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateRadioListField(FieldCreateBaseInfo baseInfo,  string defaultValue, List<string> selectList)
        {
            ListFieldConfigModel configModel = new ListFieldConfigModel { DefaultValue = defaultValue, SelectList = selectList };
            return this.CreateField(baseInfo, FieldType.RadioList, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateDropdownField(FieldCreateBaseInfo baseInfo,  string defaultValue, List<string> selectList)
        {
            ListFieldConfigModel configModel = new ListFieldConfigModel { DefaultValue = defaultValue, SelectList = selectList };
            return this.CreateField(baseInfo, FieldType.DropdownList, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateUserField(FieldCreateBaseInfo baseInfo,  bool defaultValueIsCurrent)
        {
            UserFieldConfigModel configModel = new UserFieldConfigModel { defaultValueIsCurrent = defaultValueIsCurrent};
            return this.CreateField(baseInfo, FieldType.User, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateUserListField(FieldCreateBaseInfo baseInfo,  bool defaultValueIsCurrent)
        {
            UserFieldConfigModel configModel = new UserFieldConfigModel { defaultValueIsCurrent = defaultValueIsCurrent};
            return this.CreateField(baseInfo,FieldType.UserList, JsonConvert.SerializeObject(configModel));
        }

        public Field CreateMetadataField(FieldCreateBaseInfo baseInfo,  string formCode)
        {
            MetadataFieldConfigModel configModel = new MetadataFieldConfigModel { FormCode = formCode};
            return this.CreateField(baseInfo,FieldType.Metadata, JsonConvert.SerializeObject(configModel));
        }

        public event TEventHandler<ColdewObject, Field> FieldCreated;

        protected Field CreateField(FieldCreateBaseInfo baseInfo, string type, string config)
        {
            this._lock.AcquireWriterLock();
            try
            {
                if (string.IsNullOrEmpty(baseInfo.Code))
                {
                    throw new ArgumentNullException("baseInfo.Code");
                }
                if (string.IsNullOrEmpty(baseInfo.Name))
                {
                    throw new ArgumentNullException("baseInfo.Name");
                }

                if (this._fields.Any(x => x.Name == baseInfo.Name))
                {
                    throw new FieldNameRepeatException();
                }

                if (!string.IsNullOrEmpty(baseInfo.Code) && this._fields.Any(x => x.Code == baseInfo.Code))
                {
                    throw new FieldCodeRepeatException();
                }

                FieldModel model = new FieldModel
                {
                    ObjectId = this.ID,
                    Code = baseInfo.Code,
                    Name = baseInfo.Name,
                    Tip = baseInfo.Tip,
                    Required = baseInfo.Required,
                    Type = type,
                    Config = config
                };
                model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();

                Field field = this.CreateField(model);
                this.BindEvent(field);
                this._fields.Add(field);
                if (this.FieldCreated != null)
                {
                    this.FieldCreated(this, field);
                }
                return field;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public virtual Field CreateField(FieldModel model)
        {
            FieldNewInfo newInfo = new FieldNewInfo(model.ID, model.Code, model.Name, model.Tip, model.Required, model.Type, model.IsSystem, this);
            switch (newInfo.Type)
            {
                case FieldType.Name:
                    return new NameField(newInfo);
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
                case FieldType.CreatedTime:
                    return new CreatedTimeField(newInfo);
                case FieldType.ModifiedTime:
                    return new ModifiedTimeField(newInfo);
                case FieldType.User:
                    UserFieldConfigModel userFieldConfigModel = JsonConvert.DeserializeObject<UserFieldConfigModel>(model.Config);
                    return new UserField(newInfo, userFieldConfigModel.defaultValueIsCurrent, this.ColdewManager.OrgManager.UserManager);
                case FieldType.CreatedUser:
                    return new CreatedUserField(newInfo, this.ColdewManager.OrgManager.UserManager);
                case FieldType.ModifiedUser:
                    return new ModifiedUserField(newInfo, this.ColdewManager.OrgManager.UserManager);
                case FieldType.UserList:
                    UserFieldConfigModel userListFieldConfigModel = JsonConvert.DeserializeObject<UserFieldConfigModel>(model.Config);
                    return new UserListField(newInfo, userListFieldConfigModel.defaultValueIsCurrent, this.ColdewManager.OrgManager.UserManager);
                case FieldType.Metadata:
                    MetadataFieldConfigModel metadataFieldConfigModel = JsonConvert.DeserializeObject<MetadataFieldConfigModel>(model.Config);
                    return new MetadataField(newInfo, this.ColdewManager.ObjectManager.GetObjectByCode(metadataFieldConfigModel.FormCode));
                case FieldType.Json:
                    return new JsonField(newInfo);
            }
            throw new ArgumentException("type");
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

        public event TEventHandler<ColdewObject, Field> FieldDeleted;

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
            if (this.FieldDeleted != null)
            {
                this.FieldDeleted(this, field);
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

        public ColdewObjectInfo Map(User user)
        {
            return new ColdewObjectInfo
            {
                ID = this.ID,
                Name = this.Name,
                Code = this.Code,
                Type = this.Type,
                PermissionValue = this.ObjectPermission.GetPermission(user),
                Fields = this._fields.Select(x => x.Map()).ToList()
            };
        }

        public ColdewObjectInfo Map()
        {
            return new ColdewObjectInfo
            {
                ID = this.ID,
                Name = this.Name,
                Code = this.Code,
                Type = this.Type,
                Fields = this._fields.Select(x => x.Map()).ToList()
            };
        }

        internal void Load()
        {
            IList<FieldModel> models = NHibernateHelper.CurrentSession.QueryOver<FieldModel>().Where(x => x.ObjectId == this.ID).List();
            foreach (FieldModel model in models)
            {
                Field field = this.CreateField(model);
                if (field.Type == FieldType.CreatedUser)
                {
                    this.CreatedUserField = field as CreatedUserField;
                }
                else if (field.Type == FieldType.CreatedTime)
                {
                    this.CreatedTimeField = field as CreatedTimeField;
                }
                else if (field.Type == FieldType.ModifiedUser)
                {
                    this.ModifiedUserField = field as ModifiedUserField;
                }
                else if (field.Type == FieldType.ModifiedTime)
                {
                    this.ModifiedTimeField = field as ModifiedTimeField;
                }
                else if (field.Type == FieldType.Name)
                {
                    this.NameField = field as NameField;
                }
                this.BindEvent(field);
                this._fields.Add(field);
            }
            this.MetadataManager.Load();
            this.FavoriteManager.Load();
            this.GridViewManager.Load();
            this.FormManager.Load();
            this.MetadataPermission.EntityManager.Load();
            this.MetadataPermission.StrategyManager.Load();
            this.ObjectPermission.Load();
            this.FieldPermission.Load();
        }

    }
}
