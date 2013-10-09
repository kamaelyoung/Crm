using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Coldew.Data.Organization;
using System.IO;
using Coldew.Api.Organization.Exceptions;
using System.Collections.Specialized;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public class OrganizationManagement
    {
        public OrganizationManagement()
        {
            log4net.Config.XmlConfigurator.Configure();
            this.Logger = log4net.LogManager.GetLogger("logger");
            try
            {
                this.InitManagers();

                this.UserManager.Deleted += new TEventHandler<UserManagement, DeleteEventArgs<User>>(UserManager_Deleted);
                this.PositionManager.Deleted += new TEventHandler<PositionManagement, DeleteEventArgs<Position>>(PositionManager_Deleted);
                this.DepartmentManager.Deleted += new TEventHandler<DepartmentManagement, DeleteEventArgs<Department>>(DepartmentManager_Deleted);
                this.GroupManager.Deleted += new TEventHandler<GroupManagement, DeleteEventArgs<Group>>(GroupManager_Deleted);
            }
            catch(Exception ex)
            {
                this.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void ValidateLicense()
        {
            
        }

        void GroupManager_Deleted(GroupManagement sender, DeleteEventArgs<Group> args)
        {
            foreach (Group group in this.GroupManager.Groups)
            {
                if (group.Groups.Contains(args.DeleteObject))
                {
                    group.RemoveGroup(args.Operator, args.DeleteObject);
                }
            }
            List<Function> functions = this.FunctionManager.GetAllFunction();
            foreach (Function function in functions)
            {
                if (function.Contains(args.DeleteObject))
                {
                    function.Remove(args.DeleteObject);
                }
            }
        }

        void DepartmentManager_Deleted(DepartmentManagement sender, DeleteEventArgs<Department> args)
        {
            foreach (Group group in this.GroupManager.Groups)
            {
                if (group.Departments.Contains(args.DeleteObject))
                {
                    group.RemoveDepartment(args.Operator, args.DeleteObject);
                }
            }
        }

        void PositionManager_Deleted(PositionManagement sender, DeleteEventArgs<Position> args)
        {
            foreach (Group group in this.GroupManager.Groups)
            {
                if (group.Positions.Contains(args.DeleteObject))
                {
                    group.RemovePoisition(args.Operator, args.DeleteObject);
                }
            }
        }

        void UserManager_Deleted(UserManagement sender, DeleteEventArgs<User> args)
        {
            foreach (Group group in this.GroupManager.Groups)
            {
                if (group.GroupUsers.Contains(args.DeleteObject))
                {
                    group.RemoveUser(args.Operator, args.DeleteObject);
                }
            }
            List<Function> functions = this.FunctionManager.GetAllFunction();
            foreach (Function function in functions)
            {
                if (function.Contains(args.DeleteObject))
                {
                    function.Remove(args.DeleteObject);
                }
            }
        }

        #region Managers
        /// <summary>
        /// 用户管理
        /// </summary>

        UserManagement _userManager;
        public UserManagement UserManager
        {
            get
            {
                return this._userManager;
            }
        }

        /// <summary>
        /// 部门管理
        /// </summary>
        DepartmentManagement _departmentManager;
        public DepartmentManagement DepartmentManager
        {
            get
            {
                return this._departmentManager;
            }
        }

        /// <summary>
        /// 职位管理
        /// </summary>
        PositionManagement _positionManager;
        public PositionManagement PositionManager
        {
            get
            {
                return this._positionManager;
            }
        }

        UserPositionManagement _userPositionManager;
        public UserPositionManagement UserPositionManager
        {
            get
            {
                return this._userPositionManager;
            }
        }

        /// <summary>
        /// 用户组管理
        /// </summary>
        GroupManagement _groupManager;
        public GroupManagement GroupManager
        {
            get
            {
                return this._groupManager;
            }
        }

        /// <summary>
        /// 登录认证管理
        /// </summary>
        AuthenticationManagement _authenticationManager;
        public AuthenticationManagement AuthenticationManager
        {
            get
            {
                return this._authenticationManager;
            }
        }

        /// <summary>
        /// 操作日志管理
        /// </summary>
        OperationLogManagement _operationLogManager;
        public OperationLogManagement OperationLogManager
        {
            get
            {
                return this._operationLogManager;
            }
        }

        /// <summary>
        /// 功能权限管理
        /// </summary>
        FunctionManagement _functionManager;
        public FunctionManagement FunctionManager
        {
            get
            {
                return this._functionManager;
            }
        }
        #endregion

        ILog _logger;
        public ILog Logger 
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _logger = value;
            }
            get
            {
                return _logger;
            }
        }

        private void InitManagers()
        {
            this._userManager = new UserManagement(this);
            this._positionManager = new PositionManagement(this);
            this._departmentManager = new DepartmentManagement(this);
            this._groupManager = new GroupManagement(this);
            this._authenticationManager = new AuthenticationManagement(this);
            this._userPositionManager = new UserPositionManagement(this);
            this._operationLogManager = new OperationLogManagement(this);
            this._functionManager = new FunctionManagement(this);

            this.UserManager.Loading +=
                new TEventHandler<UserManagement, List<User>>(this.UserService_OnLoading);
        }

        void UserService_OnLoading(UserManagement sender, List<User> args)
        {
            args.Add(System);
        }

        User _system;
        public User System
        {
            get
            {
                if (_system == null)
                {
                    _system = new User(this, new UserModel
                    {
                        ID = "system",
                        Account = "system",
                        Gender = (int)UserGender.Man,
                        Role = (int)UserRole.System,
                        Name = "System",
                        Password = Cryptography.MD5Encode("edoc2"),
                    });
                }
                return _system;
            }
        }
    }
}
