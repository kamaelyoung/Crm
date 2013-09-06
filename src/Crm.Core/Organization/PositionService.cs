using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using Crm.Api.Organization;

namespace Crm.Core.Organization
{
    public class PositionService : IPositionService
    {
        public PositionService(CrmManager crmManager)
        {
            this.OrganizationManager = crmManager.OrgManager;
        }

        public OrganizationManagement OrganizationManager { set; get; }

        public PositionInfo Create(string operationUserId, PositionCreateInfo createInfo)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Position position = this.OrganizationManager.PositionManager.Create(opUser, createInfo, OrganizationType.Position);
            return position.MapPositionInfo();
        }

        public void DeletePositionById(string operationUserId, string positionId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            this.OrganizationManager.PositionManager.Delete(opUser, positionId);
        }

        public void ChangePositionInfo(string operationUserId, PositionChangeInfo changeInfo)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Position position = this.OrganizationManager.PositionManager.GetPositionById(changeInfo.ID);
            position.Change(opUser, changeInfo);
        }

        public void AddUserToPosition(string operationUserId, UserPositionInfo userPositionInfo)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            this.OrganizationManager.UserPositionManager.Create(opUser, userPositionInfo);
        }

        public void RemoveUserFromPosition(string operationUserId, string positionId, string userId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            this.OrganizationManager.UserPositionManager.Delete(opUser, userId, positionId);
        }

        public PositionInfo GetPositionById(string positionId)
        {
            Position position = this.OrganizationManager.PositionManager.GetPositionById(positionId);
            if (position != null)
            {
                return position.MapPositionInfo();
            }
            return null;
        }

        public PositionInfo GetTopPosition()
        {
            return this.OrganizationManager.PositionManager.TopPosition.MapPositionInfo();
        }

        public IList<PositionInfo> GetChildPositions(string positionId)
        {
            Position position = this.OrganizationManager.PositionManager.GetPositionById(positionId);
            return position.SelfChildren.Select(x => x.MapPositionInfo()).ToList();
        }

        public IList<PositionInfo> GetPositionsByDepartmentId(string departmentId)
        {
            Department department = this.OrganizationManager.DepartmentManager.GetDepartmentById(departmentId);
            return department.Positions.Select(x => x.MapPositionInfo()).ToList();
        }

        public IList<PositionInfo> GetPositionsOfUser(string userId)
        {
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            return user.Positions.Select(x => x.MapPositionInfo()).ToList();
        }

        public IList<DepartmentInfo> GetManagerialDepartments(string positionId)
        {
            Position position = this.OrganizationManager.PositionManager.GetPositionById(positionId);
            if (position != null)
            {
                return position.ManagerialDepartments.Select(x => x.MapDepartmentInfo()).ToList();
            }
            return null;
        }

        public IList<UserPositionInfo> GetUserPositionsOfUser(string userId)
        {
            return this.OrganizationManager.UserPositionManager
                .GetUserPositionsByUserId(userId)
                .Select(x => x.MapUserPositionInfo())
                .ToList();
        }

        public UserPositionInfo GetUserPositions(string userId, string positionId)
        {
            UserPosition userPosition = this.OrganizationManager.UserPositionManager
                .GetUserPosition(userId, positionId);
            return userPosition == null ? null : userPosition.MapUserPositionInfo();
        }

        public void ChangeUserPosition(string operationUserId, string userId, string positionId, string changePositionId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            this.OrganizationManager.UserPositionManager
                .ChangeUserPosition(opUser, userId, positionId, changePositionId);
        }

        public PositionInfo GetPositionByName(string name)
        {
            Position position = this.OrganizationManager.PositionManager.GetPositionByName(name);
            if (position != null)
            {
                return position.MapPositionInfo();
            }
            return null;
        }


        public IList<PositionInfo> GetAllPositions()
        {
            return this.OrganizationManager.PositionManager.Positions
                .Select(x => x.MapPositionInfo())
                .ToList();
        }


        public void AddUserToPosition(string operationUserId, string positionId, string userId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            this.OrganizationManager.UserPositionManager.Create(opUser, new UserPositionInfo { PositionId = positionId, UserId = userId });
        }

        public IList<PositionInfo> SearchByName(string name)
        {
            List<Position> posInfoList = this.OrganizationManager.PositionManager.SearchName(name);
            var result = new List<PositionInfo>();
            if (posInfoList != null && posInfoList.Count > 0)
                result.AddRange(from posInfo in posInfoList where posInfoList != null select posInfo.MapPositionInfo());
            return result;
        }
    }
}