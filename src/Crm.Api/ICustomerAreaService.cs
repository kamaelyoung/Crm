using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    public interface ICustomerAreaService
    {
        CustomerAreaInfo Create(string name, List<string> managerAccounts);

        void Modify(int id, string name, List<string> managerAccounts);

        void Delete(int id);

        CustomerAreaInfo GetAreaById(int areaId);

        List<CustomerAreaInfo> GetAllArea();
    }
}
