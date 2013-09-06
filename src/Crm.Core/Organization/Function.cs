using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data.Organization;
using System.Collections.ObjectModel;
using NHibernate.Criterion;
using Crm.Api.Organization;

namespace Crm.Core.Organization
{
    public class Function
    {
        OrganizationManagement _orgManager;
        bool _loaded;

        public Function(FunctionModel model, OrganizationManagement orgManager)
        {
            _loaded = false;
            this._orgManager = orgManager;
            this.ID = model.ID;
            this.Name = model.Name;
            this._parentId = model.ParentId;
            this.Url = model.Url;
            this.IconClass = model.IconClass;
            this.Sort = model.Sort;

            this._userPermissions = new List<FunctionUserPermission>();
            this._groupPermissions = new List<FunctionGroupPermission>();
        }

        private void Load()
        {
            if (!_loaded)
            {
                lock (this)
                {
                    if (!_loaded)
                    {
                        ICriterion criterion = Expression.Where<MemberFunctionModel>(model => model.FunctionId == this.ID);
                        IList<MemberFunctionModel> models = NHibernateHelper.CurrentSession.QueryOver<MemberFunctionModel>().Where(criterion).List();
                        if (models != null)
                        {
                            foreach (MemberFunctionModel model in models)
                            {
                                if (model.MemberType == (int)MemberType.User)
                                {
                                    User user = this._orgManager.UserManager.GetUserById(model.MemberId);
                                    if (user != null)
                                    {
                                        FunctionUserPermission userPerm = new FunctionUserPermission(model.ID, user, model.HasPermission, this._orgManager);
                                        this._userPermissions.Add(userPerm);
                                    }
                                }
                                else if (model.MemberType == (int)MemberType.Group)
                                {
                                    Group group = this._orgManager.GroupManager.GetGroupById(model.MemberId);
                                    if (group != null)
                                    {
                                        FunctionGroupPermission groupPerm = new FunctionGroupPermission(model.ID, group, model.HasPermission, this._orgManager);
                                        this._groupPermissions.Add(groupPerm);
                                    }
                                }
                            }
                        }
                        _loaded = true;
                    }
                }
            }
        }

        public string ID { private set; get; }

        public string Name { private set; get; }

        private string _parentId;

        public Function _parent;
        public Function Parent
        {
            get
            {
                if (_parent == null && !string.IsNullOrEmpty(_parentId))
                {
                    _parent = this._orgManager.FunctionManager.GetFunctionInfoById(_parentId);
                }
                return _parent;
            }
        }

        public string Url { private set; get; }

        public string IconClass { set; get; }

        public int Sort { private set; get; }

        private List<FunctionUserPermission> _userPermissions;

        public ReadOnlyCollection<FunctionUserPermission> UserPermissions
        {
            get
            {
                this.Load();
                return this._userPermissions.AsReadOnly();
            }
        }

        private List<FunctionGroupPermission> _groupPermissions;

        public ReadOnlyCollection<FunctionGroupPermission> GroupPermissions
        {
            get
            {
                this.Load();
                return this._groupPermissions.AsReadOnly();
            }
        }

        public void Add(User user, bool permission)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            this.Load();
            MemberFunctionModel model = new MemberFunctionModel();
            model.FunctionId = this.ID;
            model.MemberId = user.ID;
            model.MemberType = (int)MemberType.User;
            model.HasPermission = permission;
            model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();

            FunctionUserPermission userPerm = new FunctionUserPermission(model.ID, user, permission, this._orgManager);
            this._userPermissions.Add(userPerm);
        }

        public void Remove(User user)
        {
            this.Load();
            FunctionUserPermission userPerm = this._userPermissions.FirstOrDefault(x => x.User == user);
            if (userPerm != null)
            {
                MemberFunctionModel funModel = NHibernateHelper.CurrentSession.Get<MemberFunctionModel>(userPerm.ID);
                NHibernateHelper.CurrentSession.Delete(funModel);
                NHibernateHelper.CurrentSession.Flush();
                this._userPermissions.Remove(userPerm);
            }
        }

        public bool Contains(User user)
        {
            this.Load();
            return this._userPermissions.Any(x => x.User == user);
        }

        public FunctionUserPermission GetPermission(User user)
        {
            this.Load();
            return this._userPermissions.Find(x => x.User == user);
        }

        public void Add(Group group, bool permission)
        {
            this.Load();
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            MemberFunctionModel model = new MemberFunctionModel();
            model.FunctionId = this.ID;
            model.MemberId = group.ID;
            model.MemberType = (int)MemberType.Group;
            model.HasPermission = permission;
            model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();

            FunctionGroupPermission groupPerm = new FunctionGroupPermission(model.ID, group, permission, this._orgManager);
            this._groupPermissions.Add(groupPerm);
        }

        public void Remove(Group group)
        {
            this.Load();
            FunctionGroupPermission groupPerm = this._groupPermissions.FirstOrDefault(x => x.Group == group);
            if (groupPerm != null)
            {
                MemberFunctionModel funModel = NHibernateHelper.CurrentSession.Get<MemberFunctionModel>(groupPerm.ID);
                NHibernateHelper.CurrentSession.Delete(funModel);
                NHibernateHelper.CurrentSession.Flush();
                this._groupPermissions.Remove(groupPerm);
            }
        }

        public bool Contains(Group group)
        {
            this.Load();
            return this._groupPermissions.Any(x => x.Group == group);
        }

        public FunctionGroupPermission GetPermission(Group group)
        {
            this.Load();
            return this._groupPermissions.Find(x => x.Group == group);
        }

        public bool HasPermission(User user)
        {
            this.Load();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (user.Role == UserRole.Administrator)
            {
                return true;
            }
            bool hasPerm = false;
            hasPerm = this._userPermissions.Any(x => x.User == user && x.HasPermission);
            if (hasPerm)
            {
                return hasPerm;
            }
            hasPerm = this._groupPermissions.Any(x => x.Group.InGroup(user) && x.HasPermission);
            if(hasPerm)
            {
                return hasPerm;
            }
            return false;
        }

        public FunctionInfo Map()
        {
            FunctionInfo info = new FunctionInfo();
            info.ID = this.ID;
            info.Name = this.Name;
            info.Url = this.Url;
            info.Sort = this.Sort;
            info.IconClass = this.IconClass;
            if (this.Parent != null)
            {
                info.ParentId = this.Parent.ID;
                info.ParentName = this.Parent.Name;
            }
            return info;
        }
    }
}
