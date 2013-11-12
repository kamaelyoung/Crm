using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Coldew.Core.Organization;
using Coldew.Core;
using Coldew.Api;
using Coldew.Core.MetadataPermission;
using Coldew.Core.Search;
using Newtonsoft.Json.Linq;

namespace Coldew.NnitTest
{
    public class ColdewTester : UnitTestBase
    {
        public ColdewTester()
        {
            
        }

        [Test]
        public void PermissionTest()
        {
            ColdewObject cobject = this.ColdewManager.ObjectManager.Create("testObject", "testObject");
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "名称", "", true, true, true, 1, "");
            Field diquField = cobject.CreateDropdownField("diqu", "地区", false, false, true, 3, null, new List<string> { "天河区", "番禺区" });
            Field salesUsersField = cobject.CreateUserField("userField", "业务员", "", true, true, true, 4, true);
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", "", true, false, false, 16, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", "", false, false, false, 17, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", "", true, false, false, 18, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", "", false, false, false, 19, true);

            JObject dictionary = new JObject();
            dictionary.Add(nameField.Code, "name1");
            dictionary.Add(diquField.Code, "天河区");
            dictionary.Add(salesUsersField.Code, "user5");
            Metadata metadata = cobject.MetadataManager.Create(this.User1, dictionary);

            Assert.IsFalse(metadata.CanPreview(this.User2));

            List<MetadataPermission> entityPermissionValues = new List<MetadataPermission>();
            cobject.PermissionManager.EntityPermissionManager.Create(metadata.ID, new MetadataOrgMember(this.User2), MetadataPermissionValue.View);

            Assert.IsTrue(metadata.CanPreview(this.User2));

            //strategy permission
            Assert.IsFalse(metadata.CanPreview(this.User4));
            Assert.IsFalse(metadata.CanPreview(this.User5));

            cobject.PermissionManager.PermissionStrategyManager.Create(cobject.ID, new MetadataOrgMember(this.User4), MetadataPermissionValue.View, "{diqu: '天河区'}");
            cobject.PermissionManager.PermissionStrategyManager.Create(cobject.ID, new MetadataFieldMember("userField"), MetadataPermissionValue.View, null);
            
            Assert.IsTrue(metadata.CanPreview(this.User4));
            Assert.IsTrue(metadata.CanPreview(this.User5));
        }
    }
}
