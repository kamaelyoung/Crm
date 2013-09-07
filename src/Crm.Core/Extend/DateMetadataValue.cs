using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;

namespace Crm.Core.Extend
{
    public class DateMetadataValue : MetadataValue
    {
        public DateMetadataValue(DateTime? value)
            : base(value)
        {

        }

        public DateTime? Date
        {
            get
            {
                return this.Value;
            }
        }

        public override PropertyValueType Type
        {
            get { return PropertyValueType.Date; }
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

        public override bool InCondition(IPropetySearchCondition condition)
        {
            DateRange dateCondition = condition as DateRange;
            if (dateCondition == null)
            {
                throw new ArgumentException("condition");
            }

            return dateCondition.InRange(this.Date);
        }
    }
}
