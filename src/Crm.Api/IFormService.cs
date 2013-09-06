using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    public interface IFormService
    {
        FormInfo GetFormByType(FormType type);

        FieldInfo GetField(int fieldId);

        List<FieldInfo> GetFields(FormType type);

        int GetFieldMaxIndex(FormType type);

        void DeleteField(string opUserAccount, int fieldId);

        FieldInfo CreateStringField(FormType type, string name, bool required, string defaultValue, int index);

        FieldInfo CreateTextField(FormType type, string name, bool required, string defaultValue, int index);

        FieldInfo CreateDropdownField(FormType type, string name, bool required, string defaultValue, List<string> selectList, int index);

        FieldInfo CreateRadioListField(FormType type, string name, bool required, string defaultValue, List<string> selectList, int index);

        FieldInfo CreateCheckboxListField(FormType type, string name, bool required, List<string> defaultValues, List<string> selectList, int index);

        void ModifyStringField(int fieldId, string name, bool required, string defaultValue, int index);

        void ModifyTextField(int fieldId, string name, bool required, string defaultValue, int index);

        void ModifyDropdownField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index);

        void ModifyRadioListField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index);

        void ModifyCheckboxListField(int fieldId, string name, bool required, List<string> defaultValues, List<string> selectList, int index);
    }
}
