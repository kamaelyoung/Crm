using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Coldew.Api;

namespace Coldew.Core
{
    public class ColdewConfigService : IColdewConfigService
    {
        ColdewManager _crmManager;
        public ColdewConfigService(ColdewManager crmManager)
        {
            _crmManager = crmManager;
        }

        public EmailConfigInfo GetEmailConfig()
        {
            return this._crmManager.ConfigManager.GetEmailConfig();
        }

        public void SetEmailConfig(string account, string address, string password, string server)
        {
            this._crmManager.ConfigManager.SetEmailConfig(account, address, password, server);
        }

        public void TestEmailConfig(string account, string address, string password, string server)
        {
            SmtpMailSender sender = new SmtpMailSender(new MailAddress(address), server, account, password);
            List<string> to = new List<string>();
            to.Add(address);
            sender.Send(to, null, "test", "test", false, null);
        }
    }
}
