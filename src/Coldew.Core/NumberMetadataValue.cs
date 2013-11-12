using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public class NumberMetadataValue : MetadataValue
    {
        public NumberMetadataValue(decimal? value, Field field)
            : base(value, field)
        {

        }

        public decimal? Number
        {
            get
            {
                return this.Value;
            }
        }

        public override JToken PersistenceValue
        {
            get 
            {
                return this.ShowValue; 
            }
        }

        public override string ShowValue
        {
            get 
            {
                if (this.Number.HasValue)
                {
                    return this.Number.ToString(); 
                }
                return "";
            }
        }

        public override dynamic OrderValue
        {
            get { return this.Number; }
        }

        public override dynamic EditValue
        {
            get { return this.Value; }
        }
    }
}
