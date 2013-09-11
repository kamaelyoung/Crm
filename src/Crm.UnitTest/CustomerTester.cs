using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Crm.Core;
using Crm.Core.Organization;
using Crm.Core.Extend;
using Crm.Api;

namespace Crm.UnitTest
{
    public class CustomerTester : UnitTestBase
    {
        [Test]
        public void CustomerManagerTest()
        {
            //Customer customer = this.CreateCustomer();
            //Contact contact = this.CreateContact(customer);
            //Activity activity = this.CreateActivity(contact);

            Form form = this.CrmManager.FormManager.GetForm(FormType.Customer);
            Assert.NotNull(form);
        }
    }
}
