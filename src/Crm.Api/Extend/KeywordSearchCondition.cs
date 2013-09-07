using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class KeywordSearchCondition : IPropetySearchCondition
    {
        public string Keyword { set; get; }
    }
}
