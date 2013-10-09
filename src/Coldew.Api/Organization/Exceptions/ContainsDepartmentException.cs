using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class ContainsDepartmentException : OrganizationException
    {
        string _departmentName;
        public ContainsDepartmentException(string departmentName)
        {
            this.ExceptionMessage = string.Format("已经包含部门 {0} ", departmentName);
            this._departmentName = departmentName;
        }

        public ContainsDepartmentException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._departmentName = info.GetString("_departmentName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_departmentName", this._departmentName);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_containsDepartment"), this._departmentName);
                }
                return base.Message;
            }
        }
    }
}
