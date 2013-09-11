using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Core.Extend;
using Crm.Core.Organization;

namespace Crm.Core
{
    public class FormService : IFormService
    {
        CrmManager _crmManager;

        public FormService(CrmManager crmManager)
        {
            this._crmManager = crmManager;
        }

        public FormInfo GetFormByType(FormType type)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            if (form != null)
            {
                return form.Map();
            }
            return null;
        }

        public List<FieldInfo> GetFields(FormType type)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            if (form != null)
            {
                return form.GetFields().Select(x => x.Map()).ToList();
            }
            return new List<FieldInfo>();
        }

        public FieldInfo CreateStringField(FormType type, string name, bool required, string defaultValue, int index)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            Field field = form.CreateStringField(null, name, required, false, true, index, defaultValue);
            return field.Map();
        }

        public FieldInfo CreateTextField(FormType type, string name, bool required, string defaultValue, int index)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            Field field = form.CreateTextField(null, name, required, false, true, index, defaultValue);
            return field.Map();
        }

        public FieldInfo CreateDropdownField(FormType type, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            Field field = form.CreateDropdownField(null, name, required, false, true, index, defaultValue, selectList);
            return field.Map();
        }

        public FieldInfo CreateRadioListField(FormType type, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            Field field = form.CreateRadioListField(null, name, required, false, true, index, defaultValue, selectList);
            return field.Map();
        }

        public FieldInfo CreateCheckboxListField(FormType type, string name, bool required, List<string> defaultValues, List<string> selectList, int index)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            Field field = form.CreateCheckboxListField(null, name, required, false, true, index, defaultValues, selectList);
            return field.Map();
        }

        public void ModifyStringField(int fieldId, string name, bool required, string defaultValue, int index)
        {
            StringField field = this._crmManager.FormManager.GetFieldById(fieldId) as StringField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, index);
            }
        }

        public void ModifyTextField(int fieldId, string name, bool required, string defaultValue, int index)
        {
            TextField field = this._crmManager.FormManager.GetFieldById(fieldId) as TextField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, index);
            }
        }

        public void ModifyDropdownField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            DropdownField field = this._crmManager.FormManager.GetFieldById(fieldId) as DropdownField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, selectList, index);
            }
        }

        public void ModifyRadioListField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            RadioListField field = this._crmManager.FormManager.GetFieldById(fieldId) as RadioListField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, selectList, index);
            }
        }

        public void ModifyCheckboxListField(int fieldId, string name, bool required, List<string> defaultValues, List<string> selectList, int index)
        {
            CheckboxListField field = this._crmManager.FormManager.GetFieldById(fieldId) as CheckboxListField;
            if (field != null)
            {
                field.Modify(name, required, defaultValues, selectList, index);
            }
        }

        public FieldInfo GetField(int fieldId)
        {
            Field field = this._crmManager.FormManager.GetFieldById(fieldId);
            if (field != null)
            {
                return field.Map();
            }
            return null;
        }

        public void DeleteField(string opUserAccount, int fieldId)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Field field = this._crmManager.FormManager.GetFieldById(fieldId);
            if (field != null)
            {
                field.Delete(opUser);
            }
        }

        public int GetFieldMaxIndex(FormType type)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            if (form != null)
            {
                return form.GetFieldMaxIndex();
            }
            return 0;
        }

        public FieldInfo CreateDateField(FormType type, string name, bool required, bool defaultValueIsToday, int index)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            Field field = form.CreateDateField(null, name, required, false, true, index, defaultValueIsToday);
            return field.Map();
        }

        public FieldInfo CreateNumberField(FormType type, string name, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            Field field = form.CreateNumberField(null, name, required, false, true, index, defaultValue, max, min, precision);
            return field.Map();
        }

        public void ModifyDateField(int fieldId, string name, bool required, bool defaultValueIsToday, int index)
        {
            DateField field = this._crmManager.FormManager.GetFieldById(fieldId) as DateField;
            if(field != null)
            {
                field.Modify(name, required, defaultValueIsToday, index);
            }
        }

        public void ModifyNumberField(int fieldId, string name, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index)
        {
            NumberField field = this._crmManager.FormManager.GetFieldById(fieldId) as NumberField;
            if (field != null)
            {
                field.Modify(name, required, defaultValue, max, min, precision, index);
            }
        }
    }
}
