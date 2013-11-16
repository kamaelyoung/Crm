using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Core.Workflow
{
    public class RenwuNotify
    {
        public RenwuNotify(string id, string renwuId, string userAccount, DateTime notifyTime, string subject, string body)
        {
            this.ID = id;
            this.RenwuId = renwuId;
            this.UserAccount = userAccount;
            this.NotifyTime = notifyTime;
            this.Subject = subject;
            this.Body = body;
        }

        public virtual string ID { set; get; }

        public virtual string RenwuId { get; set; }

        public virtual string UserAccount { get; set; }

        public virtual DateTime NotifyTime { get; set; }

        public virtual string Subject { get; set; }

        public virtual string Body { get; set; }
    }
}
