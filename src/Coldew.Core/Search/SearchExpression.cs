using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public abstract class SearchExpression
    {
        public SearchExpression(Field field)
        {
            this.Field = field;
        }

        public Field Field { set; get; }

        public abstract bool Compare(User opUser, Metadata metadata);
    }
}
