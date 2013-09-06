using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class DepartmentHasLogoffUserDeleteException: OrganizationException
    {
        public DepartmentHasLogoffUserDeleteException()
        {
            this.ExceptionMessage = "部门中包含有已注销用户无法删除!";
        }

        public DepartmentHasLogoffUserDeleteException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_DepartmentHasLogoffUserDelete");
                }
                return base.Message;
            }
        }

    }
}
