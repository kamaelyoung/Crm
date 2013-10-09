using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api;

namespace Coldew.Website.Models
{
    public class EmailConfigModel
    {
        public EmailConfigModel(EmailConfigInfo info)
        {
            this.account = info.Account;
            this.address = info.Address;
            this.password = info.Password;
            this.server = info.Server;
        }

        public string account;
        public string address;
        public string password;
        public string server;
    }
}