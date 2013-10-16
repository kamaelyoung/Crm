using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    public interface IColdewObjectService
    {
        ColdewObjectInfo GetFormById(string objectId);

        List<ColdewObjectInfo> GetForms();

        FieldInfo GetField(int fieldId);

        int GetFieldMaxIndex(string objectId);

        void DeleteField(string opUserAccount, int fieldId);

        FieldInfo CreateStringField(string objectId, string name, bool required, string defaultValue, int index);

        FieldInfo CreateDateField(string objectId, string name, bool required, bool defaultValueIsToday, int index);

        FieldInfo CreateNumberField(string objectId, string name, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index);

        FieldInfo CreateTextField(string objectId, string name, bool required, string defaultValue, int index);

        FieldInfo CreateDropdownField(string objectId, string name, bool required, string defaultValue, List<string> selectList, int index);

        FieldInfo CreateRadioListField(string objectId, string name, bool required, string defaultValue, List<string> selectList, int index);

        FieldInfo CreateCheckboxListField(string objectId, string name, bool required, List<string> defaultValues, List<string> selectList, int index);

        void ModifyStringField(int fieldId, string name, bool required, string defaultValue, int index);

        void ModifyDateField(int fieldId, string name, bool required, bool defaultValueIsToday, int index);

        void ModifyNumberField(int fieldId, string name, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision, int index);

        void ModifyTextField(int fieldId, string name, bool required, string defaultValue, int index);

        void ModifyDropdownField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index);

        void ModifyRadioListField(int fieldId, string name, bool required, string defaultValue, List<string> selectList, int index);

        void ModifyCheckboxListField(int fieldId, string name, bool required, List<string> defaultValues, List<string> selectList, int index);
    }
}
