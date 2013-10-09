using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Core
{
    public abstract class SearchExpression
    {
        protected Field _field;
        public SearchExpression(Field field)
        {
            this._field = field;
        }

        public abstract bool Compare(Metadata metadata);
    }
}
