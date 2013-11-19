using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core.Permission
{
    public class FieldPermission
    {
        public FieldPermission(string id, string fieldCode, Member member, FieldPermissionValue value)
        {
            this.ID = id;
            this.FieldCode = fieldCode;
            this.Member = member;
            this.Value = value;
        }

        public string ID { private set; get; }

        public string FieldCode { private set; get; }

        public FieldPermissionValue Value { private set; get; }

        public Member Member { private set; get; }

        public virtual bool HasValue(User user, FieldPermissionValue value)
        {
            return this.Member.Contains(user) && this.Value.HasFlag(value);
        }
    }
}
