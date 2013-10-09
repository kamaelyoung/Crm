using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    public interface IFormService
    {
        FormInfo GetFormById(string formId);

        List<FormInfo> GetForms();

        FieldInfo GetField(int fieldId);

        List<FieldInfo> GetFields(string formId);

        int GetFieldMaxIndex(string formId);

        void DeleteField(string opUserAccount, int fieldId);

        FieldInfo CreateStringField(string formId, string name, bool required, string defaultValue, int index);

        FieldInfo CreateDateField(string formId, string name, bool required, bool defaultValueIsToday, int index);

        FieldInfo CreateNumberField(string formId, string name, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index);

        FieldInfo CreateTextField(string formId, string name, bool required, string defaultValue, int index);

        FieldInfo CreateDropdownField(string formId, string name, bool required, string defaultValue, List<string> selectList, int index);

        FieldInfo CreateRadioListField(string formId, string name, bool required, string defaultValue, List<string> selectList, int index);

        FieldInfo CreateCheckboxListField(string formId, string name, bool required, List<string> defaultValues, List<string> selectList, int index);

        void ModifyStringField(int fieldId, string name, bool required, string defaultValue, int index);

        void ModifyDateField(int fieldId, string name, bool required, bool defaultValueIsToday, int index);

        void ModifyNumberField(int fieldId, string name, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index);

        void ModifyTextField(int fieldId, string name, bool required, string defaultValue, int index);

        void ModifyDropdownField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index);

        void ModifyRadioListField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index);

        void ModifyCheckboxListField(int fieldId, string name, bool required, List<string> defaultValues, List<string> selectList, int index);
    }
}
