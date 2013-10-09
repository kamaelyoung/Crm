using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Api.Exceptions;

namespace Coldew.Core
{
    public class DynamicMetadataMetadataValue: MetadataMetadataValue 
    {
        Func<Metadata> _func;
        public DynamicMetadataMetadataValue(Func<Metadata> func, MetadataField field)
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
