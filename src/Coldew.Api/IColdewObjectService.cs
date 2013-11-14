using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    public interface IColdewObjectService
    {
        ColdewObjectInfo GetFormById(string objectId);

        ColdewObjectInfo GetFormByCode(string objectCode);

        List<ColdewObjectInfo> GetForms(string userAccount);

        ObjectPermissionValue GetobjectPermissionValue(string objectId, string account);

        FieldInfo GetField(int fieldId);

        void DeleteField(string opUserAccount, int fieldId);

        FieldInfo CreateStringField(string objectId, string name, string code, bool required, string defaultValue);

        FieldInfo CreateDateField(string objectId, string name, string code, bool required, bool defaultValueIsToday);

        FieldInfo CreateNumberField(string objectId, string name, string code, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision);

        FieldInfo CreateTextField(string objectId, string name, string code, bool required, string defaultValue);

        FieldInfo CreateDropdownField(string objectId, string name, string code, bool required, string defaultValue, List<string> selectList);

        FieldInfo CreateRadioListField(string objectId, string name, string code, bool required, string defaultValue, List<string> selectList);

        FieldInfo CreateCheckboxListField(string objectId, string name, string code, bool required, List<string> defaultValues, List<string> selectList);

        void ModifyStringField(int fieldId, string name, bool required, string defaultValue);

        void ModifyDateField(int fieldId, string name, bool required, bool defaultValueIsToday);

        void ModifyNumberField(int fieldId, string name, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision);

        void ModifyTextField(int fieldId, string name, bool required, string defaultValue);

        void ModifyDropdownField(int fieldId, string name, bool required, string defaultValue, List<string> selectList);

        void ModifyRadioListField(int fieldId, string name, bool required, string defaultValue, List<string> selectList);

        void ModifyCheckboxListField(int fieldId, string name, bool required, List<string> defaultValues, List<string> selectList);
    }
}
