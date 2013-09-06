using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;

namespace Crm.Core.Extend
{
    public class NumberMetadataValue : MetadataValue
    {
        public NumberMetadataValue(decimal? value)
            : base(value)
        {

        }

        public decimal? Number
        {
            get
            {
                return this.Value;
            }
        }

        public override PropertyValueType Type
        {
            get { return PropertyValueType.Number; }
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

        public override bool InCondition(PropetySearchCondition condition)
        {
            NumberPropertySearchCondition numberCondition = condition as NumberPropertySearchCondition;
            if (numberCondition == null)
            {
                throw new ArgumentException("condition");
            }

            return numberCondition.InRange(this.Number);
        }
    }
}
