using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Api.Exceptions;

namespace Crm.Core.Extend
{
    public abstract class MetadataValue 
    {
        public MetadataValue(dynamic value)
        {
            this.Value = value;
        }

        public virtual dynamic Value { protected set; get; }

        public abstract dynamic OrderValue { get; }

        public abstract string PersistenceValue { get; }

        public abstract string ShowValue { get; }

        public abstract dynamic EditValue { get; }

        public abstract bool InCondition(IPropetySearchCondition condition);

        public abstract PropertyValueType Type { get; }

        public static MetadataValue Create(PropertyValueType valueType, string value)
        {
            switch (valueType)
            {
                case PropertyValueType.Number:
                    if (string.IsNullOrEmpty(value))
                    {
                        return new NumberMetadataValue(null);
                    }
                    decimal number;
                    if (decimal.TryParse(value, out number))
                    {
                        return new NumberMetadataValue(number);
                    }
                    else
                    {
                        throw new CrmException("创建NumberMetadataValue出错,value:" + value);
                    }
                case PropertyValueType.Date:
                    if (string.IsNullOrEmpty(value))
                    {
                        return new DateMetadataValue(null);
                    }
                    DateTime date;
                    if (DateTime.TryParse(value, out date))
                    {
                        return new DateMetadataValue(date);
                    }
                    else
                    {
                        throw new CrmException("创建DateMetadataValue出错,value:" + value);
                    }
                case PropertyValueType.String:
                    return new StringMetadataValue(value);
                case PropertyValueType.StringList:
                    List<string> stringList = new List<string>();
                    if (!string.IsNullOrEmpty(value))
                    {
                        stringList = value.Split(',').ToList();
                    }
                    return new StringListMetadataValue(stringList);
            }

            throw new CrmException("创建NumberMetadataValue出错,找不到符合的valuetype:" + valueType);
        }
    }
}
