using System;
using System.Collections.Generic;

using System.Web;

namespace Crm.Api.Organization
{
    [Flags]
    public enum OrganizationType
    {
        Company = 1,
        Department = 2,
        Position = 4,
        VirtualPosition = 8,
        ManagerPosition = 16,
        GeneralManagerPosition = 32,
        AssistantPosition = 64,
        Group = 128,
        User = 256
    }
}