using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Crm.Core
{
    internal class RegexHelper
    {
        public static List<Regex> GetRegexes(List<string> keywords)
        {
            if (keywords == null || keywords.Count == 0)
            {
                return new List<Regex>();
            }
            return keywords.Select(x => new Regex(x.ToLower())).ToList();
        }
    }
}
