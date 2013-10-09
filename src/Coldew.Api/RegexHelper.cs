using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Coldew.Api
{
    public class RegexHelper
    {
        public static List<string> GetKeywords(string keyword)
        {
            List<string> keywords = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                Regex regex = new Regex("\\s+");
                keyword = regex.Replace(keyword, " ");
                keywords = keyword.Split(' ').ToList();
            }
            return keywords;
        }

        public static List<Regex> GetRegexes(string keyword)
        {
            List<string> keywords = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                Regex regex = new Regex("\\s+");
                keyword = regex.Replace(keyword, " ");
                keywords = keyword.Split(' ').ToList();
            }
            return GetRegexes(keywords);
        }

        public static List<Regex> GetRegexes(List<string> keywords)
        {
            if (keywords == null || keywords.Count == 0)
            {
                return new List<Regex>();
            }
            return keywords.Select(x => new Regex(x.ToLower())).ToList();
        }

        public static Regex GetRegex(string keyword)
        {
            return new Regex(keyword.ToLower());
        }
    }
}
