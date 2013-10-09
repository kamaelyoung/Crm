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

        public FunctionInfo Create(FunctionCreateInfo createInfo)
        {
            Function function = this._orgMnger.FunctionManager.Create(createInfo);
            return function.Map();
        }

        public FunctionInfo GetFunctionInfoById(string functionId)
        {
            Function function = this._orgMnger.FunctionManager.GetFunctionInfoById(functionId);
            if (function != null)
            {
                return function.Map();
            }
            return null;
        }

        public List<FunctionInfo> GetTopFunctionInfos()
        {
            List<Function> functions = this._orgMnger.FunctionManager.GetTopFunctions();
            if (functions != null)
            {
                return functions.Select(x => x.Map()).ToList();
            }
            return null;
        }

        public List<FunctionInfo> GetChildren(string parentId)
        {
            Function function = this._orgMnger.FunctionManager.GetFunctionInfoById(parentId);
            if (function != null)
            {
                List<Function> functions = this._orgMnger.FunctionManager.GetChildren(function);
                if (functions != null)
                {
                    return functions.Select(x => x.Map()).OrderBy(x => x.Sort).ToList();
                }
            }
            return null;
        }

        public void SetMemberFunction(string operationUserId, string functionId, string memberId, MemberType memberType, bool permission)
        {
            User opUser = this._orgMnger.UserManager.GetUserById(operationUserId);

            Function function = this._orgMnger.FunctionManager.GetFunctionInfoById(functionId);
            if (function == null)
            {
                return;
            }
            if (memberType == MemberType.User)
            {
                User user = this._orgMnger.UserManager.GetUserById(memberId);
                if (user == null)
                {
                    return;
                }
                if (function.Contains(user))
                {
                    FunctionUserPermission userPerm = function.GetPermission(user);
                    userPerm.Modify(permission);
                }
                else
                {
                    function.Add(user, permission);
                }
            }
            else
            {
                Group group = this._orgMnger.GroupManager.GetGroupById(memberId);
                if (group == null)
                {
                    return;
                }

                if (function.Contains(group))
                {
                    FunctionGroupPermission groupPerm = function.GetPermission(group);
                    groupPerm.Modify(permission);
                }
                else
                {
                    function.Add(group, permission);
                }
            }
        }

        public List<MemberFunctionInfo> GetMemberFunction(string functionId)
        {
            Function function = this._orgMnger.FunctionManager.GetFunctionInfoById(functionId);
            if (function != null)
            {
                return function.UserPermissions.Select(x => x.Map())
                    .Concat(function.GroupPermissions.Select(x => x.Map()))
                    .ToList();
            }
            return null;
        }

        public MemberFunctionInfo GetMemberFunction(string memberId, MemberType memberType, string functionCode)
        {
            Function function = this._orgMnger.FunctionManager.GetFunctionInfoById(functionCode);
            if (function != null)
            {
                if (memberType == MemberType.User)
                {
                    User user = this._orgMnger.UserManager.GetUserById(memberId);
                    if (user != null)
                    {
                        FunctionUserPermission userPerm = function.GetPermission(user);
                        if (userPerm != null)
                        {
                            return userPerm.Map();
                        }
                    }
                }
                else
                {
                    Group group = this._orgMnger.GroupManager.GetGroupById(memberId);
                    if (group != null)
                    {
                        FunctionGroupPermission groupPerm = function.GetPermission(group);
                        if (groupPerm != null)
                        {
                            return groupPerm.Map();
                        }
                    }
                }
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

        public void DeleteMember(string operationUserId, string memberId, MemberType memberType)
        {
            User opUser = this._orgMnger.UserManager.GetUserById(operationUserId);

            if (memberType == MemberType.User)
            {
                User user = this._orgMnger.UserManager.GetUserById(memberId);
                if (user != null)
                {
                    List<Function> functions = this._orgMnger.FunctionManager.GetAllFunction();
                    foreach (Function function in functions)
                    {
                        if (function.Contains(user))
                        {
                            function.Remove(user);
                        }
                    }
                }
            }
            else
            {
                Group group = this._orgMnger.GroupManager.GetGroupById(memberId);
                if (group != null)
                {
                    List<Function> functions = this._orgMnger.FunctionManager.GetAllFunction();
                    foreach (Function function in functions)
                    {
                        if (function.Contains(group))
                        {
                            function.Remove(group);
                        }
                    }
                }
            }
        }

        public void DeleteMember(string operationUserId, string functionId, string memberId, MemberType memberType)
        {
            User opUser = this._orgMnger.UserManager.GetUserById(operationUserId);
            Function function = this._orgMnger.FunctionManager.GetFunctionInfoById(functionId);
            if (function != null)
            {
                if (memberType == MemberType.User)
                {
                    User user = this._orgMnger.UserManager.GetUserById(memberId);
                    function.Remove(user);
                }
                else
                {
                    Group group = this._orgMnger.GroupManager.GetGroupById(memberId);
                    if (group != null)
                    {
                        function.Remove(group);
                    }
                }
            }
        }

        public List<FunctionInfo> GetUserFunctions(string userId, string parentId)
        {
            User user = this._orgMnger.UserManager.GetUserById(userId);
            Function function = this._orgMnger.FunctionManager.GetFunctionInfoById(parentId);
            if (user != null && function != null)
            {
                List<Function> functions = this._orgMnger.FunctionManager.GetChildren(function);

                if (functions != null)
                {
                    if (user.Role == UserRole.Administrator)
                    {
                        return functions.Select(x => x.Map()).OrderBy(x => x.Sort).ToList();
                    }
                    else
                    {
                        return functions.Where(x => x.HasPermission(user)).Select(x => x.Map()).OrderBy(x => x.Sort).ToList();
                    }
                }
            }
            return null;
        }
    }
}
