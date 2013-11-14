using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Coldew.Api;

namespace Coldew.Core.Permission
{
    public class ObjectPermission
    {
        public ObjectPermission(string id, Member member, string objectId, ObjectPermissionValue value)
        {
            this.ID = id;
            this.Member = member;
            this.ObjectId = objectId;
            this.Value = value;
        }

        public string ID{ private set; get; }

        public Member Member { private set; get; }

        public string ObjectId { private set; get; }

        public ObjectPermissionValue Value { private set; get; }
    }
}
