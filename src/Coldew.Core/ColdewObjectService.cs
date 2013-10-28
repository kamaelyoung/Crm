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

        public ColdewObjectInfo GetFormById(string objectId)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            if (form != null)
            {
                return form.Map();
            }
            return null;
        }

        public List<ColdewObjectInfo> GetForms()
        {
            List<ColdewObject> forms = this._coldewManager.ObjectManager.GetForms();
            return forms.Select(x => x.Map()).ToList();
        }

        public FieldInfo CreateStringField(string objectId, string name, bool required, string defaultValue, int index)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            Field field = form.CreateStringField(null, name, "", required, false, true, index, defaultValue);
            return field.Map();
        }

        public FieldInfo CreateTextField(string objectId, string name, bool required, string defaultValue, int index)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            Field field = form.CreateTextField(null, name, "", required, false, true, index, defaultValue);
            return field.Map();
        }

        public FieldInfo CreateDropdownField(string objectId, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            Field field = form.CreateDropdownField(null, name, "", required, false, true, index, defaultValue, selectList);
            return field.Map();
        }

        public FieldInfo CreateRadioListField(string objectId, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            Field field = form.CreateRadioListField(null, name, "", required, false, true, index, defaultValue, selectList);
            return field.Map();
        }

        public FieldInfo CreateCheckboxListField(string objectId, string name, bool required, List<string> defaultValues, List<string> selectList, int index)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            Field field = form.CreateCheckboxListField(null, name, "", required, false, true, index, defaultValues, selectList);
            return field.Map();
        }

        public void ModifyStringField(int fieldId, string name, bool required, string defaultValue, int index)
        {
            StringField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as StringField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, index);
            }
        }

        public void ModifyTextField(int fieldId, string name, bool required, string defaultValue, int index)
        {
            TextField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as TextField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, index);
            }
        }

        public void ModifyDropdownField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            DropdownField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as DropdownField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, selectList, index);
            }
        }

        public void ModifyRadioListField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            RadioListField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as RadioListField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, selectList, index);
            }
        }

        public void ModifyCheckboxListField(int fieldId, string name, bool required, List<string> defaultValues, List<string> selectList, int index)
        {
            CheckboxListField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as CheckboxListField;
            if (field != null)
            {
                field.Modify(name, required, defaultValues, selectList, index);
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

        public int GetFieldMaxIndex(string objectId)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            if (form != null)
            {
                return form.GetFieldMaxIndex();
            }
            return 0;
        }

        public FieldInfo CreateDateField(string objectId, string name, bool required, bool defaultValueIsToday, int index)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            Field field = form.CreateDateField(null, name, "", required, false, true, index, defaultValueIsToday);
            return field.Map();
        }

        public FieldInfo CreateNumberField(string objectId, string name, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index)
        {
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            Field field = form.CreateNumberField(null, name, "", required, false, true, index, defaultValue, max, min, precision);
            return field.Map();
        }

        public void ModifyDateField(int fieldId, string name, bool required, bool defaultValueIsToday, int index)
        {
            DateField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as DateField;
            if(field != null)
            {
                field.Modify(name, required, defaultValueIsToday, index);
            }
        }

        public void ModifyNumberField(int fieldId, string name, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index)
        {
            NumberField field = this._coldewManager.ObjectManager.GetFieldById(fieldId) as NumberField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, max, min, precision, index);
            }
        }
    }
}
