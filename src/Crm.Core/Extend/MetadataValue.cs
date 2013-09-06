using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;

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

        public abstract bool InCondition(PropetySearchCondition condition);

        public abstract PropertyValueType Type { get; }
    }
}
