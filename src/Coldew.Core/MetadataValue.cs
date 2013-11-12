using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Api.Exceptions;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public abstract class MetadataValue 
    {
        public MetadataValue(dynamic value, Field field)
        {
            this.Value = value;
            this.Field = field;
        }

        protected virtual dynamic Value { set; get; }

        public virtual Field Field { protected set; get; }

        public abstract dynamic OrderValue { get; }

        public abstract JToken PersistenceValue { get; }

        public abstract string ShowValue { get; }

        public abstract dynamic EditValue { get; }
    }
}
