using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Api.Exceptions;

namespace Coldew.Core
{
    public class FunctionMetadataRelatedValue: MetadataRelatedValue 
    {
        Func<Metadata> _func;
        public FunctionMetadataRelatedValue(Func<Metadata> func, MetadataField field)
            : base(null, field)
        {
            this._func = func;
        }

        public override Metadata Metadata
        {
            get
            {
                return this._func();
            }
        }
    }
}
