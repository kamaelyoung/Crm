using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Core
{
    public static class MetadataEnumerable
    {
        public static IEnumerable<Metadata> OrderBy(this IEnumerable<Metadata> metadatas, string orderBy)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return metadatas;
            }
            orderBy = orderBy.Trim();
            if (orderBy.EndsWith(" desc"))
            {
                orderBy = orderBy.Replace(" desc", "");
                return metadatas.OrderByDescending(x =>
                {
                    MetadataProperty property = x.GetProperty(orderBy);
                    if (property != null)
                    {
                        return property.Value.OrderValue;
                    }
                    return null;
                });
            }

            orderBy = orderBy.Replace(" asc", "");
            return metadatas.OrderBy(x =>
            {
                MetadataProperty property = x.GetProperty(orderBy);
                if (property != null)
                {
                    return property.Value.OrderValue;
                }
                return null;
            });
        }
    }
}
