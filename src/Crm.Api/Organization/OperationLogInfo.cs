using System;
using System.Collections.Generic;

using System.Text;

namespace Crm.Api.Organization
{
    [Serializable]
    public class OperationLogInfo
    {
        public virtual long ID { get; set; }

        public virtual OperationType OperationType { get; set; }

        public virtual string OperationContent { get; set; }

        public virtual DateTime OperationTime { get; set; }

        public virtual string OperatorId { get; set; }

        public virtual string OperatorName { get; set; }
    }
}
