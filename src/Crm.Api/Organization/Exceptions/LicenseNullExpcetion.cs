using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class LicenseNullExpcetion : LicenseException
    {
        public LicenseNullExpcetion()
        {
            this.ExceptionMessage = "License没得到许可!";
        }

        public LicenseNullExpcetion(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_licenseNull");
                }
                return base.Message;
            }
        }
    }
}
