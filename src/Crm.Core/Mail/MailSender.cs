using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Crm.Core
{
    public abstract class MailSender
    {
        public MailSender(MailAddress from, string server, string account, string password)
        {
            this.From = from;
            this.Server = server;
            this.Account = account;
            this.Password = password;
        }

        public MailAddress From
        {
            get;
            set;
        }

        public string Server
        {
            get;
            set;
        }

        public string Account
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        public abstract void Send(List<string> to, List<string> cc, string subject, string body, bool isBodyHtml, List<Attachment> attachments);
    }
}
