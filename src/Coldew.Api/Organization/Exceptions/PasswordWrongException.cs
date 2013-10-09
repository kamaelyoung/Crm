using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class PasswordWrongException : OrganizationException
    {

        int _allowTryTimes; 
        int _wrongCount;
        public PasswordWrongException(int allowTryTimes, int wrongCount)
        {
            this._allowTryTimes = allowTryTimes;
            this._wrongCount = wrongCount;
            string message = "";
            if (allowTryTimes > 0)
            {
                message = string.Format("密码错误，还有{0}次机会！", allowTryTimes - wrongCount);
            }
            else
            {
                message = "密码错误!";
            }
            this.ExceptionMessage = message;
        }

        public PasswordWrongException()
            :this(0, 0)
        {

        }

        public PasswordWrongException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._allowTryTimes = info.GetInt32("_allowTryTimes");
            this._wrongCount = info.GetInt32("_wrongCount");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_allowTryTimes", this._allowTryTimes);
            info.AddValue("_wrongCount", this._wrongCount);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    if (this._allowTryTimes > 0)
                    {
                        return string.Format(this.StringResouceProvider.GetString("ex_TryLoginPasswordWrong"), this._allowTryTimes - this._wrongCount);
                    }
                    else
                    {
                        return this.StringResouceProvider.GetString("ex_PasswordWrong");
                    }
                }
                return base.Message;
            }
        }
    }
}
