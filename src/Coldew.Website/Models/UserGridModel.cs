using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api.Organization;

namespace Coldew.Website.Models
{
    public class UserGridModel
    {
        public UserGridModel(UserInfo userInfo)
        {
            this.id = userInfo.ID;
            this.account = userInfo.Account;
            this.name = userInfo.Name;
            this.position = userInfo.MainPositionName;
            this.email = userInfo.Email;
            this.status = this.Map(userInfo.Status);
        }
        private string Map(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Lock: return "锁定";
                case UserStatus.Normal: return "正常";
                case UserStatus.Logoff: return "注销";
            }
            return "";
        }
        public string id;
        public string account;
        public string name;
        public string position;
        public string status;
        public string email;
    }
}