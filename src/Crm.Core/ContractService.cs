using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Core.Organization;

namespace Crm.Core
{
    public class ContractService : IContractService
    {
        CrmManager _crmManager;
        public ContractService(CrmManager crmManager)
        {
            this._crmManager = crmManager;
        }

        public List<ContractInfo> GetContracts(string account, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            return this._crmManager.ContractManager
                .GetContracts(user, skipCount, takeCount, out totalCount)
                .Select(x => x.Map())
                .ToList();
        }

        public List<ContractInfo> GetExpiringContracts(string account, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            return this._crmManager.ContractManager
                .GetExpiringContracts(user, skipCount, takeCount, out totalCount)
                .Select(x => x.Map())
                .ToList();
        }

        public List<ContractInfo> GetExpiredContracts(string account, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            return this._crmManager.ContractManager
                .GetExpiredContracts(user, skipCount, takeCount, out totalCount)
                .Select(x => x.Map())
                .ToList();
        }

        public List<ContractInfo> GetContracts(string account)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            return this._crmManager.ContractManager
                .GetContracts(user)
                .Select(x => x.Map())
                .ToList();
        }

        public ContractInfo Create(string opUserAccount, string name, string customerId, DateTime startDate, DateTime endDate, int expiredComputeDays, float value, List<string> ownerAccounts, List<PropertyOperationInfo> propertys)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Customer customer = this._crmManager.CustomerManager.GetById(customerId) as Customer;
            List<User> owners = ownerAccounts.Select(x => this._crmManager.OrgManager.UserManager.GetUserByAccount(x)).ToList();
            Contract contract = this._crmManager.ContractManager.Create(new ContractCreateInfo { 
                OpUser = opUser, 
                Name = name, 
                Customer = customer, 
                EndDate = endDate,
                ExpiredComputeDays = expiredComputeDays,
                Owners = owners,
                StartDate = startDate,
                Value = value,
                PropertyInfos = propertys 
            });
            return contract.Map();
        }

        public void Modify(string opUserAccount, string contractId, string name, DateTime startDate, DateTime endDate, int expiredComputeDays, float value, List<string> ownerAccounts, List<PropertyOperationInfo> propertys)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            List<User> owners = ownerAccounts.Select(x => this._crmManager.OrgManager.UserManager.GetUserByAccount(x)).ToList();
            Contract contract = this._crmManager.ContractManager.GetContractById(contractId);
            contract.Modify(new ContractModifyInfo
            {
                OpUser = opUser,
                Name = name,
                EndDate = endDate,
                ExpiredComputeDays = expiredComputeDays,
                Owners = owners,
                StartDate = startDate,
                Value = value,
                PropertyInfos = propertys
            });
        }

        public void Delete(string opUserAccount, string contractId)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Contract contract = this._crmManager.ContractManager.GetContractById(contractId);
            contract.Delete(opUser);
        }

        public ContractInfo GetContractById(string id)
        {
            Contract contract = this._crmManager.ContractManager.GetContractById(id);
            return contract.Map();
        }

        public List<ContractInfo> Search(string account, ContractSearchInfo searchInfo, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            List<Contract> contracts = this._crmManager.ContractManager.Search(user, new ContractSearcher(searchInfo), skipCount, takeCount, out totalCount);
            return contracts.Select(x => x.Map()).ToList();
        }

        public List<ContractInfo> Search(string account, ContractSearchInfo searchInfo)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            List<Contract> contracts = this._crmManager.ContractManager.Search(user, new ContractSearcher(searchInfo));
            return contracts.Select(x => x.Map()).ToList();
        }
    }
}
