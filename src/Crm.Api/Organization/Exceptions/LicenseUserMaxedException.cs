using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class LicenseUserMaxedException : LicenseException
    {
        public LicenseUserMaxedException()
        {
            this.ExceptionMessage = "超过License用户数!";
        }

        public LicenseUserMaxedException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
        
        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_licenseUserMaxed");
                }
                return base.Message;
            }
        }
    }
}
