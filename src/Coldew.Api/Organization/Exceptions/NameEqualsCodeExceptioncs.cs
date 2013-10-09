using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class NameEqualsCodeExceptioncs : OrganizationException
    {
        string _name;
        public NameEqualsCodeExceptioncs(string name)
        {
            this.ExceptionMessage = string.Format("名称与 {0} 编号重复!", name);
            this._name = name;
        }

        public NameEqualsCodeExceptioncs(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._name = info.GetString("_name");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_name", this._name);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_NameEqualsCode"), this._name);
                }
                return base.Message;
            }
        }
    }
}
