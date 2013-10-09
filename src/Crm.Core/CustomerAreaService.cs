using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Coldew.Core.Organization;

namespace Crm.Core
{
    public class CustomerAreaService : ICustomerAreaService
    {
        CrmManager _crmManger;
        public CustomerAreaService(CrmManager crmManger)
        {
            this._crmManger = crmManger;
        }

        public CustomerAreaInfo GetAreaById(int areaId)
        {
            CustomerArea area = this._crmManger.AreaManager.GetAreaById(areaId);
            if (area != null)
            {
                return area.Map();
            }
            return null;
        }

        public List<CustomerAreaInfo> GetAllArea()
        {
            return this._crmManger.AreaManager.GetAllArea().Select(x => x.Map()).ToList();
        }

        public CustomerAreaInfo Create(string name, List<string> managerAccounts)
        {
            List<User> managers = new List<User>();
            if (managerAccounts != null)
            {
                managers = managerAccounts.Select(x => this._crmManger.OrgManager.UserManager.GetUserByAccount(x)).ToList();
            }
            
            CustomerArea area = this._crmManger.AreaManager.Create(name, managers);
            return area.Map();
        }

        public void Modify(int id, string name, List<string> managerAccounts)
        {
            List<User> managers = new List<User>();
            if (managerAccounts != null)
            {
                managers = managerAccounts.Select(x => this._crmManger.OrgManager.UserManager.GetUserByAccount(x)).ToList();
            }
            CustomerArea area = this._crmManger.AreaManager.GetAreaById(id);
            area.Modify(new CustomerAreaModifyInfo { Name = name, ManagerUsers = managers });
        }

        public void Delete(int id)
        {
            CustomerArea area = this._crmManger.AreaManager.GetAreaById(id);
            area.Delete();
        }
    }
}
