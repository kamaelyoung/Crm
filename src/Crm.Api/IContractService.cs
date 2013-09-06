using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    public interface IContractService
    {
        List<ContractInfo> GetContracts(string account, int skipCount, int takeCount, out int totalCount);

        List<ContractInfo> GetContracts(string account);

        List<ContractInfo> GetExpiringContracts(string account, int skipCount, int takeCount, out int totalCount);

        List<ContractInfo> GetExpiredContracts(string account, int skipCount, int takeCount, out int totalCount);

        ContractInfo Create(string opUserAccount, string name, string customerId, DateTime startDate, DateTime endDate, int expiredComputeDays, float value, List<string> ownerAccounts, List<PropertyOperationInfo> propertys);

        void Modify(string opUserAccount, string contractId, string name, DateTime startDate, DateTime endDate, int expiredComputeDays, float value, List<string> ownerAccounts, List<PropertyOperationInfo> propertys);

        void Delete(string opUserAccount, string contractId);

        ContractInfo GetContractById(string id);

        List<ContractInfo> Search(string account, ContractSearchInfo searchInfo, int skipCount, int takeCount, out int totalCount);

        List<ContractInfo> Search(string account, ContractSearchInfo searchInfo);
    }
}
