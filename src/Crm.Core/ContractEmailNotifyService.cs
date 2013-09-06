using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Crm.Core.Organization;

namespace Crm.Core
{
    public class ContractEmailNotifyService
    {
        CrmManager _crmManager;
        public ContractEmailNotifyService(CrmManager crmManager)
        {
            this._crmManager = crmManager;
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(this._Start));
            thread.IsBackground = true;
            thread.Start();
        }

        private void _Start()
        {
#if DEBUG
            Thread.Sleep(1000 * 60 * 1);
#else
                Thread.Sleep(1000 * 60 * 5);
#endif
            while (true)
            {
                try
                {
                    if (this._crmManager.MailSender == null)
                    {
                        this._crmManager.Logger.Error("MailSender is null");
                        continue;
                    }

                    List<Contract> contracts = this._crmManager.ContractManager.GetNeedEmailNotifyContracts();
                    foreach (Contract contract in contracts)
                    {
                        try
                        {
                            string subject = string.Format("{0}-合同到期提醒", contract.Name);
                            string body = string.Format("合同{0}将于{1}过期", contract.Name, contract.EndDate.ToString("yyyy-MM-dd"));
                            List<User> notifiedUsers = new List<User>();
                            notifiedUsers.Add(contract.Customer.Creator);
                            notifiedUsers.AddRange(contract.Customer.SalesUsers);
                            notifiedUsers.AddRange(contract.Owners);
                            notifiedUsers.Add(contract.Creator);
                            notifiedUsers = notifiedUsers.Distinct().Where(x => !string.IsNullOrEmpty(x.Email)).ToList();
                            this._crmManager.MailSender.Send(notifiedUsers.Select(x => x.Email).ToList(), null, subject, body, false, null);
                            contract.SetEmailNotified(true);
                        }
                        catch (Exception ex)
                        {
                            this._crmManager.Logger.Error(ex.Message, ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._crmManager.Logger.Error(ex.Message, ex);
                }
#if DEBUG
                Thread.Sleep(1000 * 60);
#else
                Thread.Sleep(1000 * 60 * 30);
#endif
            }
        }
    }
}
