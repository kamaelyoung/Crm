using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Core.Organization
{
    public abstract class OrganizationChildInitializer
    {
        public abstract void Init(OrganizationManagement orgManager);
    }
}
