using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace Coldew.Core
{
    internal class SmtpMailSender : MailSender
    {
        private const int DEFAULT_MAIL_SERVER_PORT = 25;

        public SmtpMailSender(MailAddress from, string server, string account, string password)
            :base(from, server, account, password)
        {

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        public override void Send(List<string> to, List<string> cc, string subject, string body, bool isBodyHtml, List<Attachment> attachments)
        {
            if (to == null || to.Count == 0)
            {
                throw new ArgumentNullException("to");
            }

            MailMessage _mm = new MailMessage();
            _mm.From = this.From;
            _mm.Sender = this.From;
            _mm.Body = body;
            _mm.Subject = subject;
            _mm.IsBodyHtml = isBodyHtml;
            // 添加附件
            if (attachments != null
                && attachments.Count > 0)
            {
                foreach (Attachment eachAttachment in attachments)
                {
                    _mm.Attachments.Add(eachAttachment);
                }
            }
            foreach(string address in to)
            {
                _mm.To.Add(new MailAddress(address));
            }

            if (cc != null)
            {
                foreach (string address in cc)
                {
                    _mm.CC.Add(new MailAddress(address));
                }
            }

            System.Net.Mail.SmtpClient client = new SmtpClient();
            client.Host = this.Server;
            client.Port = DEFAULT_MAIL_SERVER_PORT;
            if(!string.IsNullOrEmpty(this.Account))
            {
                client.Credentials = new System.Net.NetworkCredential(this.Account, this.Password);
            }

            client.Send(_mm);

            _mm.Dispose();
        }
    }
}
