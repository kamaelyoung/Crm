using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Crm.Core;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public class CustomerAreaMetadataValue : MetadataValue
    {
        public CustomerAreaMetadataValue(CustomerArea value, Field field)
            : base(value, field)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
        }

        public CustomerArea Area
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
                return this.Area.ID.ToString(); 
            }
        }

        public override string ShowValue
        {
            get 
            {
                if (this.Area != null)
                {
                    return this.Area.Name; 
                }
                return "";
            }
        }

        public override dynamic OrderValue
        {
            get { return this.ShowValue; }
        }

        public override dynamic EditValue
        {
            get { return this.Area.ID; }
        }
    }
}
