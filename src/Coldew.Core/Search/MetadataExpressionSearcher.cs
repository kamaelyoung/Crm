using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Coldew.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coldew.Core.Search
{
    public class MetadataExpressionSearcher : MetadataSearcher
    {
        List<SearchExpression> _expressions;
        List<Regex> _keywordRegexs;

        private string expression;

        public MetadataExpressionSearcher(string keyword, List<SearchExpression> expressions, ColdewObject coldewObject)
        {
            this._keywordRegexs = RegexHelper.GetRegexes(keyword);
            this._expressions = expressions;
            expression = "";

            coldewObject.FieldDeleted += new TEventHandler<Core.ColdewObject, Field>(ColdewObject_FieldDeleted);
        }

        void ColdewObject_FieldDeleted(ColdewObject sender, Field field)
        {
            this.RemoveFieldSearchExpression(field);
        }

        public void RemoveFieldSearchExpression(Field field)
        {
            SearchExpression expression = this._expressions.Find(x => x.Field == field);
            if (expression != null)
            {
                List<SearchExpression> expressions = this._expressions.ToList();
                expressions.Remove(expression);
                this._expressions = expressions.ToList();
            }
        }

        public override bool Accord(Metadata metadata)
        {
            if (this._keywordRegexs.Any(regex => !regex.IsMatch(metadata.Content)))
            {
                return false;
            }
            foreach (SearchExpression expression in this._expressions)
            {
                if (!expression.Compare(metadata))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return expression;
        }

        public static MetadataExpressionSearcher Parse(string expression, ColdewObject form)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            List<SearchExpression> expressions = new List<SearchExpression>();
            JObject jObject = JsonConvert.DeserializeObject<JObject>(expression);
            foreach (JProperty jProperty in jObject.Properties())
            {
                Field field = form.GetFieldByCode(jProperty.Name);
                if (field == null)
                {
                    continue;
                }
                switch (field.Type)
                {
                    case FieldType.Number:
                        decimal? max = null;
                        decimal? min = null;
                        decimal decimalOutput;
                        if (decimal.TryParse(jProperty.Value["max"].ToString(), out decimalOutput))
                        {
                            max = decimalOutput;
                        }
                        if (decimal.TryParse(jProperty.Value["min"].ToString(), out decimalOutput))
                        {
                            min = decimalOutput;
                        }
                        expressions.Add(new NumberSearchExpression(field, min, max));
                        break;
                    case FieldType.Date:
                        DateTime? start = null;
                        DateTime? end = null;
                        DateTime dateOutput;
                        if (DateTime.TryParse(jProperty.Value["start"].ToString(), out dateOutput))
                        {
                            start = dateOutput;
                        }
                        if (DateTime.TryParse(jProperty.Value["end"].ToString(), out dateOutput))
                        {
                            end = dateOutput;
                        }
                        expressions.Add(new DateSearchExpression(field, start, end));
                        break;
                    default:
                        string keywordPropertyValue = jProperty.Value.ToString();
                        expressions.Add(new KeywordSearchExpression(field, keywordPropertyValue));
                        break;
                }
            }
            MetadataExpressionSearcher seracher = new MetadataExpressionSearcher(jObject["keyword"].ToString(), expressions, form);
            seracher.expression = expression;
            return seracher;
        }
    }
}
