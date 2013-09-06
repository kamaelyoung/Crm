using System;
using System.Collections.Generic;

using System.Text;

namespace Crm.Api.Organization
{
    [Serializable]
    public class OperationLogSearchInfo
    {
        public int StartIndex { set; get; }
        public int EndIndex { set; get; }
        public string OperatorName { set; get; }
        public List<OperationType> OperationType { set; get; }
        public DateTime? OperationStartDate { set; get; }
        public DateTime? OperationEndDate { set; get; }
        public OperationLogOrder? Order { set; get; }
        public bool OrderByDescending { set; get; }
    }

    public enum OperationLogOrder
    {
        OperationType,
        OperationContent,
        OperationTime,
        OperatorName
    }
}
