using System;
using System.Collections.Generic;
using System.Linq;
using Coldew.Api.Organization.Exceptions;
using System.Collections.ObjectModel;

using Coldew.Data.Organization;
using Coldew.Api.Organization;


namespace Coldew.Core.Organization
{
    public class PositionManagement
    {

        private Dictionary<string, Position> _positionByIdDictionary;

        public PositionManagement(OrganizationManagement orgMnger)
        {
            this._orgMnger = orgMnger;
            this._positions = new List<Position>();
            this._positionByIdDictionary = new Dictionary<string, Position>();
        }

        private bool _loaded;
        OrganizationManagement _orgMnger;

        List<Position> _positions;
        List<Position> _Positions
        {
            get
            {
                return _positions;
            }
        }

        public event TEventHandler<PositionManagement, List<Position>> Loaded;

        /// <summary>
        /// 创建用户之前
        /// </summary>
        public event TEventHandler<PositionManagement, CreateEventArgs<PositionCreateInfo, PositionInfo, Position>> Creating;

        /// <summary>
        /// 创建用户之后
        /// </summary>
        public event TEventHandler<PositionManagement, CreateEventArgs<PositionCreateInfo, PositionInfo, Position>> Created;

        /// <summary>
        /// 删除用户之前
        /// </summary>
        public event TEventHandler<PositionManagement, DeleteEventArgs<Position>> Deleting;

        /// <summary>
        /// 删除用户之后
        /// </summary>
        public event TEventHandler<PositionManagement, DeleteEventArgs<Position>> Deleted;

        private object _updateLockObject = new object();

        internal Position Create(User operationUser, PositionCreateInfo createInfo, OrganizationType positionType)
        {
            lock (this._updateLockObject)
            {
                if (operationUser == null)
                {
                    throw new ArgumentNullException("operationUser");
                }
                if (string.IsNullOrWhiteSpace(createInfo.Name))
                {
                    throw new OrganizationException("createInfo.Name不能为空!");
                }

                List<Position> positions = this._Positions;
                Position parent = this.GetPositionById(createInfo.ParentId);
                if (parent != null)
                {
                    Position child = parent.Children.FirstOrDefault(x => x.Name == createInfo.Name);
                    if (child != null)
                    {
                        throw new PositionNameReapeatException();
                    }
                }

                CreateEventArgs<PositionCreateInfo, PositionInfo, Position> args = new CreateEventArgs<PositionCreateInfo, PositionInfo, Position>
                {
                    CreateInfo = createInfo,
                    Operator = operationUser
                };
                if (this.Creating != null)
                {
                    this.Creating(this, args);
                }
                PositionModel model = new PositionModel
                    {
                        Name = createInfo.Name,
                        ParentId = createInfo.ParentId,
                        Remark = createInfo.Remark
                    };

                model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
                NHibernateHelper.CurrentSession.Flush();

                Position position = new Position(model, this._orgMnger);

                List<Position> tempPositions = positions.ToList();
                tempPositions.Add(position);
                this._positions = tempPositions;

                this.IndexPosition();
                if (this.Created != null)
                {
                    args.CreatedObject = position;
                    args.CreatedSnapshotInfo = position.MapPositionInfo();
                    this.Created(this, args);
                }
                return position;
            }
        }

        /// <summary>
        /// 创建职位
        /// </summary>
        /// <param name="position">职位</param>
        /// <returns>职位</returns>
        public Position Create(User operationUser, PositionCreateInfo createInfo)
        {
            return this.Create(operationUser, createInfo, OrganizationType.Position);
        }

        
        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="positionId">职位ID</param>
        public void Delete(User operationUser, string positionId)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }

            Position position = this.GetPositionById(positionId);
            if (position != null)
            {
                if (position == this.TopPosition)
                {
                    throw new PositionDeleteException("无法删除顶级职位!");
                }

                if (position.Users.Count > 0)
                {
                    throw new PositionHasUserDeleteException();
                }
                if (position.SelfChildrenHasUser())
                {
                    throw new PositionHasUserDeleteException();
                }
                if (position.LogoffedUsers.Count > 0)
                {
                    throw new PositionHasLogoffUserDeleteException();
                }
                if (position.SelfChildrenHasLogoffedUser())
                {
                    throw new PositionChildHasLogoffUserDeleteException();
                }
                lock (_updateLockObject)
                {
                    foreach (Position child in position.Children)
                    {
                        if (child.PositionType != OrganizationType.ManagerPosition)
                        {
                            this.Delete(operationUser, child.ID);
                        }
                    }
                    foreach (Department managerDepartment in position.SelfManagerialDepartments)
                    {
                        this._orgMnger.DepartmentManager.Delete(operationUser, managerDepartment.ID);
                    }
                    DeleteEventArgs<Position> args = new DeleteEventArgs<Position>
                    {
                        DeleteObject = position,
                        Operator = operationUser,
                    };
                    if (Deleting != null)
                    {
                        this.Deleting(this, args);
                    }
                    PositionModel model = NHibernateHelper.CurrentSession.Get<PositionModel>(positionId);
                    NHibernateHelper.CurrentSession.Delete(model);
                    NHibernateHelper.CurrentSession.Flush();

                    List<Position> tempPositions = this._Positions.ToList();
                    tempPositions.Remove(position);
                    this._positions = tempPositions;

                    this.IndexPosition();
                    if (this.Deleted != null)
                    {
                        this.Deleted(this, args);
                    }
                }
            }
        }

        public Position TopPosition
        {
            get
            {
                
                return (from p in this._Positions
                        where p.Parent == null
                        select p).FirstOrDefault();
            }
        }

        public Position GetPositionById(string positionId)
        {
            if (string.IsNullOrEmpty(positionId))
            {
                return null;
            }
            Load();
            try
            {
                return this._positionByIdDictionary[positionId];
            }
            catch (KeyNotFoundException)
            {

            }
            return null;
        }

        public Position GetPositionByName(string name)
        {

            return this._Positions.Find(x => x.Name == name);
        }

        public List<Position> SearchName(string name)
        {
            return this._Positions.Where(x => x.Name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) > -1).ToList();
        }

        private void IndexPosition()
        {
            this._positionByIdDictionary = this._positions.ToDictionary(x => x.ID);
        }

        /// <summary>
        /// 获取用户的所属职位列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>职位列表</returns>
        public ReadOnlyCollection<Position> Positions
        {
            get
            {
                
                return this._Positions.AsReadOnly();
            }
        }

        internal virtual void Load()
        {
            if (!this._loaded)
            {
                lock (this)
                {
                    if (!this._loaded)
                    {
                        List<PositionModel> models = NHibernateHelper.CurrentSession.QueryOver<PositionModel>().List().ToList();
                        if (models != null)
                        {
                            models.ForEach(x =>
                            {
                                Position position = new Position(x, this._orgMnger);
                                this._positions.Add(position);
                            });
                        }
                        this.IndexPosition();
                        this._loaded = true;
                        if (this.Loaded != null)
                        {
                            this.Loaded(this, this._positions);
                        }
                    }
                }
            }
        }
    }
}
