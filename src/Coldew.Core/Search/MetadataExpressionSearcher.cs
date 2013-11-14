using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Coldew.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Coldew.Core.Organization;

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

        public override bool Accord(User opUser, Metadata metadata)
        {
            if (this._keywordRegexs.Any(regex => !regex.IsMatch(metadata.Content)))
            {
                return false;
            }
            foreach (SearchExpression expression in this._expressions)
            {
                if (!expression.Compare(opUser, metadata))
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

        private const string OperationUserToken = "${operationUser}";
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
                    case FieldType.ModifiedTime:
                    case FieldType.CreatedTime:
                        string startValue = jProperty.Value["start"].ToString();
                        string endValue = jProperty.Value["end"].ToString();
                        DateTime? start = null;
                        DateTime? end = null;
                        DateTime dateOutput;
                        if (DateTime.TryParse(startValue, out dateOutput))
                        {
                            start = dateOutput;
                        }
                        if (DateTime.TryParse(endValue, out dateOutput))
                        {
                            end = dateOutput;
                        }
                        if (start.HasValue || end.HasValue)
                        {
                            expressions.Add(new DateSearchExpression(field, start, end));
                            break;
                        }

                        int? startDays = null;
                        int? endDays = null;
                        int intOutput;
                        if (int.TryParse(startValue, out intOutput))
                        {
                            startDays = intOutput;
                        }
                        if (int.TryParse(endValue, out intOutput))
                        {
                            endDays = intOutput;
                        }
                        if (startDays.HasValue || endDays.HasValue)
                        {
                            expressions.Add(new DateRecentlySearchExpression(field, startDays, endDays));
                        }
                        break;
                    case FieldType.User:
                    case FieldType.ModifiedUser:
                    case FieldType.CreatedUser:
                        if (jProperty.Value.ToString().Equals(OperationUserToken, StringComparison.InvariantCultureIgnoreCase))
                        {
                            expressions.Add(new OperationUserExpression(field));
                        }
                        else
                        {
                            expressions.Add(new KeywordSearchExpression(field, jProperty.Value.ToString()));
                        }
                        break;
                    default:
                        string keywordPropertyValue = jProperty.Value.ToString();
                        expressions.Add(new KeywordSearchExpression(field, keywordPropertyValue));
                        break;
                }
            }
            string keyword = "";
            if (jObject["keyword"] != null)
            {
                keyword = jObject["keyword"].ToString();
            }
            MetadataExpressionSearcher seracher = new MetadataExpressionSearcher(keyword, expressions, form);
            seracher.expression = expression;
            return seracher;
        }
    }
}
