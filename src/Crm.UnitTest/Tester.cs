using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Crm.Core;
using Coldew.Core.Organization;
using Crm.Api;
using System.Xml.Serialization;
using System.IO;
using Coldew.Api;

namespace Crm.UnitTest
{
    public class Tester
    {
        [Test]
        public void CustomerManagerTest()
        {
            PropertySettingDictionary diction = new PropertySettingDictionary();
            diction.Add("1", "1");

            string str = SerializeHelper.XmlSerialize(diction);

            diction = SerializeHelper.XmlDeserialize<PropertySettingDictionary>(str);
        }
    }
}
