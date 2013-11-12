using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data.Organization;
using NHibernate.Criterion;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public class FunctionService : IFunctionService
    {
        public FunctionService(OrganizationManagement orgMnger)
        {
            this._orgMnger = orgMnger;
        }

        OrganizationManagement _orgMnger;

        public FunctionInfo GetFunctionInfoById(string functionId)
        {
            Function function = this._orgMnger.FunctionManager.GetFunctionInfoById(functionId);
            if (function != null)
            {
                return function.Map();
            }
            return null;
        }

        public bool HasPermission(string userId, string functionId)
        {
            User user = this._orgMnger.UserManager.GetUserById(userId);
            Function function = this._orgMnger.FunctionManager.GetFunctionInfoById(functionId);
            if (user != null && function != null)
            {
                return function.HasPermission(user);
            }
            return false;
        }

        public List<FunctionInfo> GetAllFunction()
        {
            List<Function> functions = this._orgMnger.FunctionManager.GetAllFunction();
            if (functions != null)
            {
                return functions.Select(x => x.Map()).ToList();
            }
            return null;
        }
    }
}
