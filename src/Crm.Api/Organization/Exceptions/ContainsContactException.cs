using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class ContainsContactException : OrganizationException
    {
        string _contactorName;

        public ContainsContactException(string contactorName)
        {
            this.ExceptionMessage = string.Format("已经包含联系人{0}", contactorName);
            this._contactorName = contactorName;
        }

        public ContainsContactException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._contactorName = info.GetString("_contactorName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_contactorName", this._contactorName);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_containsContactor"), this._contactorName);
                }
                return base.Message;
            }
        }
    }
}
