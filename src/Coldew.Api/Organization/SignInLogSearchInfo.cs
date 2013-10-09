using System;
using System.Collections.Generic;

using System.Text;

namespace Coldew.Api.Organization
{
    [Serializable]
    public class SignInLogSearchInfo
    {
        public int StartIndex { set; get; }
        public int EndIndex { set; get; }
        public string Account { set; get; }
        public List<SignInResult> Result { set; get; }
        public DateTime? SignInStartDate { set; get; }
        public DateTime? SignInEndDate { set; get; }
        public string Ip { set; get; }
        public SignInLogOrder? Order { set; get; }
        public bool OrderByDescending { set; get; }
    }

    public enum SignInLogOrder
    {
        Account,
        Result,
        SignInTime,
        ClientInfo,
        IP
    }
}
