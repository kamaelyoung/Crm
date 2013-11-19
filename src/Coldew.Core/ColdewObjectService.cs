using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class ColdewObjectService : IColdewObjectService
    {
        ColdewManager _coldewManager;

        public ColdewObjectService(ColdewManager crmManager)
        {
            this._coldewManager = crmManager;
        }

        public ColdewObjectInfo GetFormById(string userAccount, string objectId)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(userAccount);
            ColdewObject form = this._coldewManager.ObjectManager.GetObjectById(objectId);
            if (form != null)
            {
                return form.Map(user);
            }
            return null;
        }

        public ColdewObjectInfo GetFormByCode(string userAccount, string objectCode)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(userAccount);
            ColdewObject form = this._coldewManager.ObjectManager.GetObjectByCode(objectCode);
            if (form != null)
            {
                return form.Map(user);
            }
            return null;
        }

        public List<ColdewObjectInfo> GetForms(string userAccount)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(userAccount);
            List<ColdewObject> objects = this._coldewManager.ObjectManager.GetObjects();
            return objects.Where(x => {
                return x.ObjectPermission.HasValue(user, ObjectPermissionValue.View);
            }).Select(x => x.Map(user)).ToList();
        }

        public FieldInfo CreateStringField(string objectId, string name, string code, bool required, string defaultValue)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetObjectById(objectId);
            
            Field field = form.CreateStringField(new FieldCreateBaseInfo(code, name, "", required, false),  defaultValue);
            return field.Map();
        }

        public FieldInfo CreateTextField(string objectId, string name, string code, bool required, string defaultValue)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetObjectById(objectId);
            Field field = form.CreateTextField(new FieldCreateBaseInfo(code, name, "", required, false),  defaultValue);
            return field.Map();
        }

        public FieldInfo CreateDropdownField(string objectId, string name, string code, bool required, string defaultValue, List<string> selectList)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetObjectById(objectId);
            Field field = form.CreateDropdownField(new FieldCreateBaseInfo(code, name, "", required, false),  defaultValue, selectList);
            return field.Map();
        }

        public FieldInfo CreateRadioListField(string objectId, string name, string code, bool required, string defaultValue, List<string> selectList)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetObjectById(objectId);
            Field field = form.CreateRadioListField(new FieldCreateBaseInfo(code, name, "", required, false),  defaultValue, selectList);
            return field.Map();
        }

        public FieldInfo CreateCheckboxListField(string objectId, string name, string code, bool required, List<string> defaultValues, List<string> selectList)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetObjectById(objectId);
            Field field = form.CreateCheckboxListField(new FieldCreateBaseInfo(code, name, "", required, false),  defaultValues, selectList);
            return field.Map();
        }

        public void ModifyStringField(int fieldId, string name, bool required, string defaultValue)
        {
            StringField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as StringField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue);
            }
        }

        public void ModifyTextField(int fieldId, string name, bool required, string defaultValue)
        {
            TextField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as TextField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue);
            }
        }

        public void ModifyDropdownField(int fieldId, string name, bool required, string defaultValue, List<string> selectList)
        {
            DropdownField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as DropdownField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, selectList);
            }
        }

        public void ModifyRadioListField(int fieldId, string name, bool required, string defaultValue, List<string> selectList)
        {
            RadioListField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as RadioListField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, selectList);
            }
        }

        public void ModifyCheckboxListField(int fieldId, string name, bool required, List<string> defaultValues, List<string> selectList)
        {
            CheckboxListField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as CheckboxListField;
            if (field != null)
            {
                field.Modify(name, required, defaultValues, selectList);
            }
        }

        public FieldInfo GetField(int fieldId)
        {
            Field field = this._coldewManager.ObjectManager.GetFieldById(fieldId);
            if (field != null)
            {
                return field.Map();
            }
            return null;
        }

        public void DeleteField(string opUserAccount, int fieldId)
        {
            User opUser = this._coldewManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Field field = this._coldewManager.ObjectManager.GetFieldById(fieldId);
            if (field != null)
            {
                field.Delete(opUser);
            }
        }

        public FieldInfo CreateDateField(string objectId, string name, string code, bool required, bool defaultValueIsToday)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetObjectById(objectId);
            Field field = form.CreateDateField(new FieldCreateBaseInfo(code, name, "", required, false),  defaultValueIsToday);
            return field.Map();
        }

        public FieldInfo CreateNumberField(string objectId, string name, string code, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetObjectById(objectId);
            Field field = form.CreateNumberField(new FieldCreateBaseInfo(code, name, "", required, false),  defaultValue, max, min, precision);
            return field.Map();
        }

        public void ModifyDateField(int fieldId, string name, bool required, bool defaultValueIsToday)
        {
            DateField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as DateField;
            if(field != null)
            {
                field.Modify(name, required, defaultValueIsToday);
            }
        }

        public void ModifyNumberField(int fieldId, string name, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision)
        {
            NumberField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as NumberField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, max, min, precision);
            }
        }
    }
}
