using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class KeywordSearchExpression : SearchExpression
    {
        List<Regex> _keywordRegexs;
        public KeywordSearchExpression(Field field, string keyword)
            :base(field)
        {
            this._keywordRegexs = RegexHelper.GetRegexes(keyword);
        }

        public override bool Compare(User opUser, Metadata metadata)
        {
            MetadataProperty property = metadata.GetProperty(this.Field.Code);
            if (property != null)
            {
                if (this._keywordRegexs.Any(regex => !regex.IsMatch(property.Value.ShowValue)))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
