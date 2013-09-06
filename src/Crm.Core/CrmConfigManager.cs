﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data;
using Newtonsoft.Json;
using System.Net.Mail;
using Crm.Api;

namespace Crm.Core
{
    public class CrmConfigManager
    {
        CrmManager _crmManager;
        public CrmConfigManager(CrmManager crmManager)
        {
            this._crmManager = crmManager;

            this.Load();
        }

        public void SetEmailConfig(string account, string address, string password, string server)
        {
            EmailConfigModel emailConfigModel = new EmailConfigModel { account = account, address = address, password = password, server = server };
            string emailConfigModelJson = JsonConvert.SerializeObject(emailConfigModel);
            ConfigModel model = NHibernateHelper.CurrentSession.Get<ConfigModel>("email_config");
            if (model != null)
            {
                model.Value = emailConfigModelJson;
                NHibernateHelper.CurrentSession.Update(model);
                NHibernateHelper.CurrentSession.Flush();
            }
            else
            {
                model = new ConfigModel();
                model.Name = "email_config";
                model.Value = emailConfigModelJson;
                NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();
            }

            this._crmManager.MailSender = new SmtpMailSender(new MailAddress(address), server, account, password);
        }

        public EmailConfigInfo GetEmailConfig()
        {
            EmailConfigInfo info = new EmailConfigInfo();
            ConfigModel model = NHibernateHelper.CurrentSession.Get<ConfigModel>("email_config");
            if (model != null && !string.IsNullOrEmpty(model.Value))
            {
                EmailConfigModel emailConfigModel = JsonConvert.DeserializeObject<EmailConfigModel>(model.Value);
                info.Account = emailConfigModel.account;
                info.Address = emailConfigModel.address;
                info.Password = emailConfigModel.password;
                info.Server = emailConfigModel.server;
            }

            return info;
        }

        private void Load()
        {
            ConfigModel model = NHibernateHelper.CurrentSession.Get<ConfigModel>("email_config");
            if (model != null && !string.IsNullOrEmpty(model.Value))
            {
                EmailConfigModel emailConfigModel = JsonConvert.DeserializeObject<EmailConfigModel>(model.Value);
                this._crmManager.MailSender = new SmtpMailSender(new MailAddress(emailConfigModel.address), emailConfigModel.server, emailConfigModel.account, emailConfigModel.password);
            }
        }
    }
}
