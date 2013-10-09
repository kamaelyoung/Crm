using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Website;

namespace Crm.Website
{
    public class CrmColdewInputFactory : ColdewInputFactory
    {
        public override ColdewInput CreateInput(bool setDefaultValue)
        {
            return new CrmColdewInput(setDefaultValue);
        }

        public override ColdewSearchInput CreateSearchInput()
        {
            return new CrmColdewSearchInput();
        }
    }
}