using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;

namespace Coldew.Core
{
    public class DateMetadataValue : MetadataValue
    {
        public DateMetadataValue(DateTime? value, DateField field)
            : base(value, field)
        {

        }

        public DateTime? Date
        {
            get
            {
                return this.Value;
            }
        }

        public override string PersistenceValue
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
                if (this.Date.HasValue)
                {
                    return this.Date.Value.ToString("yyyy-MM-dd"); 
                }
                return "";
            }
        }

        public override dynamic OrderValue
        {
            get { return this.Date; }
        }

        public override dynamic EditValue
        {
            get { return this.ShowValue; }
        }
    }
}
