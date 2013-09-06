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
            Field field = form.CreateField(null, name, required, false, true, index, FieldType.String, new StringFieldConfig(defaultValue));
            return field.Map();
        }

        public FieldInfo CreateTextField(FormType type, string name, bool required, string defaultValue, int index)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            Field field = form.CreateField(null, name, required, false, true, index, FieldType.Text, new StringFieldConfig(defaultValue));
            return field.Map();
        }

        public FieldInfo CreateDropdownField(FormType type, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            ListFieldConfig config = new ListFieldConfig(defaultValue, selectList);
            Field field = form.CreateField(null, name, required, false, true, index, FieldType.DropdownList, config);
            return field.Map();
        }

        public FieldInfo CreateRadioListField(FormType type, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            ListFieldConfig config = new ListFieldConfig(defaultValue, selectList);
            Field field = form.CreateField(null, name, required, false, true, index, FieldType.RadioList, config);
            return field.Map();
        }

        public FieldInfo CreateCheckboxListField(FormType type, string name, bool required, List<string> defaultValues, List<string> selectList, int index)
        {
            Form form = this._crmManager.FormManager.GetForm(type);
            CheckboxFieldConfig config = new CheckboxFieldConfig(defaultValues, selectList);
            Field field = form.CreateField(null, name, required, false, true, index, FieldType.CheckboxList, config);
            return field.Map();
        }

        public void ModifyStringField(int fieldId, string name, bool required, string defaultValue, int index)
        {
            Field field = this._crmManager.FormManager.GetFieldById(fieldId);
            field.Modify(new FieldModifyInfo { Name = name, Required = required, Index = index, Config = new StringFieldConfig(defaultValue) });
        }

        public void ModifyTextField(int fieldId, string name, bool required, string defaultValue, int index)
        {
            Field field = this._crmManager.FormManager.GetFieldById(fieldId);
            field.Modify(new FieldModifyInfo { Name = name, Required = required, Index = index, Config = new StringFieldConfig(defaultValue) });
        }

        public void ModifyDropdownField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            Field field = this._crmManager.FormManager.GetFieldById(fieldId);
            field.Modify(new FieldModifyInfo { Name = name, Required = required, Index = index, Config = new ListFieldConfig(defaultValue, selectList) });
        }

        public void ModifyRadioListField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            Field field = this._crmManager.FormManager.GetFieldById(fieldId);
            field.Modify(new FieldModifyInfo { Name = name, Required = required, Index = index, Config = new ListFieldConfig(defaultValue, selectList) });
        }

        public void ModifyCheckboxListField(int fieldId, string name, bool required, List<string> defaultValues, List<string> selectList, int index)
        {
            Field field = this._crmManager.FormManager.GetFieldById(fieldId);
            field.Modify(new FieldModifyInfo { Name = name, Required = required, Index = index, Config = new CheckboxFieldConfig(defaultValues, selectList) });
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
    }
}
