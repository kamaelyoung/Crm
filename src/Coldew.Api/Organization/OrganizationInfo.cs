using System;
using System.Collections.Generic;

using System.Text;

namespace Coldew.Api.Organization
{
    [Serializable]
    public class OrganizationInfo
    {
        public OrganizationInfo()
        {

        }

        public OrganizationInfo(PositionInfo positionInfo, OrganizationType organizationType)
        {
            this.ID = positionInfo.ID;
            this.Name = positionInfo.Name;
            this.ParentId = positionInfo.ParentId;
            this.OrganizationType = organizationType;
            if (organizationType == Organization.OrganizationType.ManagerPosition)
            {
                this.ParentId = positionInfo.DepartmentId;
            }
            this.HaveChildren = positionInfo.HaveChildren;
        }

        public OrganizationInfo(DepartmentInfo departmentInfo, OrganizationType organizationType, 
            PositionInfo ManagerPositionParentInfo)
        {
            this.ID = departmentInfo.ID;
            this.Name = departmentInfo.Name;
            if (ManagerPositionParentInfo != null)
            {
                this.ParentId = ManagerPositionParentInfo.ID;
            }
            this.OrganizationType = organizationType;
        }


        public OrganizationInfo(DepartmentInfo departmentInfo, OrganizationType organizationType)
        {
            this.ID = departmentInfo.ID;
            this.Name = departmentInfo.Name;
            this.ParentId = departmentInfo.ParentId;
            this.OrganizationType = organizationType;
        }

        public string ID { set; get; }

        public string Name { set; get; }

        public string ParentId { set; get; }

        public long PermissionValue { set; get; }

        public OrganizationType OrganizationType { set; get; }

        public IList<OrganizationInfo> Children { set; get; }

        public bool HaveChildren { set; get; }
    }
}
