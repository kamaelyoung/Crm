using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;
using Newtonsoft.Json.Linq;
using Crm.Website.Models;

namespace Crm.Website
{
    public class ExtendHelper
    {
        public static List<PropertyOperationInfo> MapPropertyOperationInfos(JObject extends)
        {
            List<PropertyOperationInfo> propertys = new List<PropertyOperationInfo>();

            foreach (JProperty property in extends.Properties())
            {
                if (property.Value is JArray)
                {
                    JArray array = property.Value as JArray;
                    string value = string.Join(",", array.Select(x => (string)x));
                    propertys.Add(new PropertyOperationInfo { Code = property.Name, Value = value });
                }
                else
                {
                    propertys.Add(new PropertyOperationInfo { Code = property.Name, Value = property.Value.ToString() });
                }
            }

            return propertys;
        }

        public static List<PropertySearchInfo> MapPropertySearchInfos(List<JObject> extendSearchs)
        {
            List<PropertySearchInfo> propertys = new List<PropertySearchInfo>();

            foreach (JObject serachModel in extendSearchs)
            {
                PropertySearchInfo serachInfo = new PropertySearchInfo();
                serachInfo.FieldCode = serachModel["fieldCode"].ToString();
                PropertyValueType valueType = (PropertyValueType)Enum.Parse(typeof(PropertyValueType), serachModel["valueType"].ToString());
                switch (valueType)
                {
                    case PropertyValueType.String:
                    case PropertyValueType.StringList:
                        serachInfo.Condition = PropertySearchConditionHelper.CreateKeywordCondition(serachModel["value"].ToString());
                        break;
                    case PropertyValueType.Number:
                        decimal? max = null;
                        decimal? min = null;
                        decimal decimalOutput;
                        if(decimal.TryParse(serachModel["max"].ToString(), out decimalOutput))
                        {
                            max = decimalOutput;
                        }
                        if(decimal.TryParse(serachModel["min"].ToString(), out decimalOutput))
                        {
                            min = decimalOutput;
                        }
                        serachInfo.Condition = PropertySearchConditionHelper.CreateNumberCondition(min, max);
                        break;
                    case PropertyValueType.Date:
                        DateTime? start = null;
                        DateTime? end = null;
                        DateTime dateOutput;
                        if (DateTime.TryParse(serachModel["start"].ToString(), out dateOutput))
                        {
                            start = dateOutput;
                        }
                        if (DateTime.TryParse(serachModel["end"].ToString(), out dateOutput))
                        {
                            end = dateOutput;
                        }
                        serachInfo.Condition = PropertySearchConditionHelper.CreateDateCondition(start, end);
                        break;
                }
                propertys.Add(serachInfo);
            }

            return propertys;
        }
    }
}