using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Coldew.Api.Organization.Exceptions;
using System.Linq;
using Coldew.Data.Organization;
using Coldew.Api.Organization;


namespace Coldew.Core.Organization
{
    public class GroupManagement
    {
        public GroupManagement(OrganizationManagement orgMnger)
        {
            this._orgMnger = orgMnger;
            this._groups = new List<Group>();
        }

        private List<Group> _groups;
        private List<Group> _Groups
        {
            get
            {
                this.Load();
                return this._groups;
            }
        }

        public EveryoneGroup Everyone { private set; get; }

        private bool _loaded;

        private void Load()
        {
            if (!this._loaded)
            {
                lock (this)
                {
                    if (!this._loaded)
                    {
                        List<GroupModel> models = NHibernateHelper.CurrentSession.QueryOver<GroupModel>().List().ToList();
                        if (models != null)
                        {

                            models.ForEach(x =>
                            {
                                this._groups.Add(this.Create((GroupType)x.GroupType, x));
                            });
                        }
                        this._loaded = true;
                        if (this.Loaded != null)
                        {
                            this.Loaded(this, this._groups);
                        }
                    }
                }
            }
        }

        private OrganizationManagement _orgMnger;

        /// <summary>
        /// 加载
        /// </summary>
        public event TEventHandler<GroupManagement, List<Group>> Loaded;
        /// <summary>
        /// 创建之前
        /// </summary>
        public event TEventHandler<GroupManagement, CreateEventArgs<GroupCreateInfo, GroupInfo, Group>> Creating;

        /// <summary>
        /// 创建之后
        /// </summary>
        public event TEventHandler<GroupManagement, CreateEventArgs<GroupCreateInfo, GroupInfo, Group>> Created;

        /// <summary>
        /// 删除之前
        /// </summary>
        public event TEventHandler<GroupManagement, DeleteEventArgs<Group>> Deleting;

        /// <summary>
        /// 删除之后
        /// </summary>
        public event TEventHandler<GroupManagement, DeleteEventArgs<Group>> Deleted;

        private object _updateLockObject = new object();

        /// <summary>
        /// 创建用户组
        /// </summary>
        /// <param name="group">用户组</param>
        /// <returns>用户组</returns>
        public Group Create(User operationUser, GroupCreateInfo createInfo)
        {
            lock (this._updateLockObject)
            {
                if (operationUser == null)
                {
                    throw new ArgumentNullException("operationUser");
                }
                if (createInfo == null)
                {
                    throw new ArgumentNullException("groupInfo");
                }

                if (createInfo.GroupType == GroupType.Group)
                {
                    if (this._Groups.Exists(x => x.GroupType != GroupType.Personal && x.Name == createInfo.Name))
                    {
                        throw new GroupNameReapeatException();
                    }
                }
                else if (createInfo.GroupType == GroupType.Personal)
                {
                    if (this._Groups.Exists(x => x.GroupType == GroupType.Personal && x.Creator == operationUser && x.Name == createInfo.Name))
                    {
                        throw new GroupNameReapeatException();
                    }
                }
                List<Group> groups = this._Groups;
                CreateEventArgs<GroupCreateInfo, GroupInfo, Group> args = new CreateEventArgs<GroupCreateInfo, GroupInfo, Group>
                {
                    CreateInfo = createInfo,
                    Operator = operationUser
                };
                if (this.Creating != null)
                {
                    this.Creating(this, args);
                }
                GroupModel model = new GroupModel
                    {
                        CreateTime = DateTime.Now,
                        CreatorId = operationUser.ID,
                        GroupType = (int)GroupType.Group,
                        Name = createInfo.Name,
                        Remark = createInfo.Remark
                    };
                model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
                NHibernateHelper.CurrentSession.Flush();

                Group group = this.Create(createInfo.GroupType, model);

                List<Group> tempGroup = groups.ToList();
                tempGroup.Add(group);
                this._groups = tempGroup;

                if (this.Created != null)
                {
                    args.CreatedObject = group;
                    args.CreatedSnapshotInfo = group.MapGroupInfo();
                    this.Created(this, args);
                }

                return group;
            }
        }

        private Group Create(GroupType groupType, GroupModel model)
        {
            Group group = new Group(model, this._orgMnger);
            return group;
        }

        

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        public void Delete(User operationUser, string groupId)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (string.IsNullOrWhiteSpace(groupId))
            {
                throw new ArgumentNullException("groupId");
            }
            Group group = this.GetGroupById(groupId);
            if (group != null)
            {

                lock (_updateLockObject)
                {
                    DeleteEventArgs<Group> args = new DeleteEventArgs<Group>()
                    {
                        DeleteObject = group,
                        Operator = operationUser
                    };

                    group.ClearMembers(operationUser);

                    if (Deleting != null)
                    {
                        this.Deleting(this, args);
                    }
                    GroupModel model = NHibernateHelper.CurrentSession.Get<GroupModel>(groupId);
                    NHibernateHelper.CurrentSession.Delete(groupId);
                    NHibernateHelper.CurrentSession.Flush();
                    List<Group> tempGroup = this._Groups.ToList();
                    tempGroup.Remove(group);
                    this._groups = tempGroup;

                    if (this.Deleted != null)
                    {
                        this.Deleted(this, args);
                    }
                }
            }
        }

        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <returns>用户组</returns>
        public Group GetGroupById(string groupId)
        {
            if (string.IsNullOrWhiteSpace(groupId))
            {
                return null;
            }
            return this._Groups.Find(x => x.ID == groupId);
        }

        public List<Group> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<Group>();
            }
            return this._Groups.Where(x=>(x.Name.IndexOf(name,StringComparison.InvariantCultureIgnoreCase)>-1)).ToList();
        }

        public ReadOnlyCollection<Group> Groups
        {
            get
            {
                return this._Groups.AsReadOnly();
            }
        }
    }
}