using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.IO;

namespace Coldew.Core
{
    internal class ExchangeMailSender: MailSender
    {
        public ExchangeMailSender(MailAddress from, string server, string account, string password)
            :base(from, server, account, password)
        {

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        public override void Send(List<string> to, List<string> cc, string subject, string body, bool isBodyHtml, List<Attachment> attachments)
        {
            CDO.Message _msg = new CDO.Message();

            string toAddress = string.Join(";", to);
            string ccAddress = string.Join(";", cc);
            _msg.To = toAddress;
            _msg.CC = ccAddress;
            _msg.BodyPart.Charset = Encoding.UTF8.WebName;
            _msg.Subject = subject;
            if (isBodyHtml)
            {
                _msg.HTMLBody = body;
            }
            else
            {
                _msg.TextBody = body;
            }

            string[] attachmentArray = new string[attachments.Count];
            if (attachments.Count > 0)
            {
                string tempAttachmentDir = this.GetTempAttachmentDir();
                int attachmentIndex = 0;
                foreach (Attachment attachment in attachments)
                {
                    //Attachment~
                    string attPath = Path.Combine(tempAttachmentDir, System.Guid.NewGuid().ToString());
                    if (!Directory.Exists(attPath))
                    {
                        Directory.CreateDirectory(attPath);
                    }
                    attPath += "\\" + attachment.Name;
                    using (Stream stream = attachment.ContentStream)
                    {
                        using (Stream fstream = new FileStream(attPath, FileMode.OpenOrCreate))
                        {
                            int bufSize = 10240;
                            byte[] buffer;
                            int read = 0;
                            while (stream.Position < stream.Length)
                            {
                                if (stream.Length - stream.Position < bufSize)
                                {
                                    bufSize = (int)(stream.Length - stream.Position);
                                }
                                buffer = new byte[bufSize];
                                read = stream.Read(buffer, 0, bufSize);
                                fstream.Write(buffer, 0, read);
                            }
                            stream.Close();
                            //close file stream
                            fstream.Close();
                        }
                    }
                    attachmentArray[attachmentIndex++] = attPath;
                    _msg.AddAttachment(attPath, this.Account, this.Password);
                }
            }
            
            CDO.IConfiguration iConfig = _msg.Configuration;
            ADODB.Fields fields = iConfig.Fields;

            fields["http://schemas.microsoft.com/cdo/configuration/sendusing"].Value = 2;
            fields["http://schemas.microsoft.com/cdo/configuration/sendemailaddress"].Value = _msg.From;
            fields["http://schemas.microsoft.com/cdo/configuration/smtpaccountname"].Value = _msg.From;
            fields["http://schemas.microsoft.com/cdo/configuration/sendusername"].Value = this.Account;
            fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"].Value = this.Password;
            fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"].Value = string.IsNullOrEmpty(this.Password) ? "0" : "1";
            fields["http://schemas.microsoft.com/cdo/configuration/languagecode"].Value = 0x0804;
            fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"].Value = this.Server;

            fields.Update();

            try
            {
                _msg.Send();
                _msg = null;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.DeleteTempAttachments(attachmentArray);
            }
        }

        private string GetTempAttachmentDir()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.GetDirectoryName(path);
            path = Path.Combine(path, "$TEMP");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        private void DeleteTempAttachments(string[] attachments)
        {
            foreach (string attachment in attachments)
            {
                try
                {
                    Directory.Delete(Path.GetDirectoryName(attachment), true);
                }
                catch
                {
                }
            }
            //Delete Temp files accessed one day ago.
            string tempDir = this.GetTempAttachmentDir();
            foreach (string dir in Directory.GetDirectories(tempDir))
            {
                foreach (string fileName in Directory.GetFiles(dir))
                {
                    FileInfo fi = new FileInfo(fileName);
                    TimeSpan ts = DateTime.Now - fi.LastAccessTime;
                    if (ts.TotalDays >= 1)
                    {
                        try
                        {
                            fi.Delete();
                        }
                        catch
                        {
                        }
                    }
                }
                if (Directory.GetFiles(dir).Length == 0)
                {
                    Directory.Delete(dir);
                }
            }
        }
    }
}
