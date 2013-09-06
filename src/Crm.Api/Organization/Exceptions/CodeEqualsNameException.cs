using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class CodeEqualsNameException : OrganizationException
    {
        string _name;

        public CodeEqualsNameException(string name)
        {
            this.ExceptionMessage = string.Format("编号与 {0} 名称重复!", name);
            this._name = name;
        }

        public CodeEqualsNameException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._name = info.GetString("name");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("name", this._name);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_codeEqualsName"), this._name);
                }
                return base.Message;
            }
        }
    }
}
