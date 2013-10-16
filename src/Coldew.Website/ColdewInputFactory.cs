using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coldew.Website
{
    public class ColdewInputFactory
    {
        public virtual ColdewInput CreateInput(bool setDefaultValue)
        {
            return new ColdewInput(setDefaultValue);
        }

        public virtual ColdewSearchInput CreateSearchInput()
        {
            return new ColdewSearchInput();
        }

        public virtual ColdewDetailsInput CreateDetailsInput()
        {
            return new ColdewDetailsInput();
        }
    }
}