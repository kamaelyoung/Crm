using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class CodeRepeatException : OrganizationException
    {
        string _name;
        public CodeRepeatException(string name)
        {
            this.ExceptionMessage = string.Format("编号与 {0} 重复!", name);
            this._name = name;
        }

        public CodeRepeatException(SerializationInfo info, StreamingContext context)
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
                    return string.Format(this.StringResouceProvider.GetString("ex_codeRepeat"), this._name);
                }
                return base.Message;
            }
        }
    }
}
