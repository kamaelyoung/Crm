using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class StringPropertySearchCondition : PropetySearchCondition
    {
        public string Keyword { set; get; }
    }
}
