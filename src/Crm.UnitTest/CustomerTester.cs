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
        private Customer CreateCustomer()
        {
            CustomerArea area = this.CrmManager.AreaManager.Create("上海市", null);
            List<User> salesUsers = new List<User>();
            salesUsers.Add(this.Admin);
            Form customerForm = this.CrmManager.FormManager.Create(FormType.Customer, "客户信息");
            Field nameField = customerForm.CreateSystemField("name", "客户名称", true, true, 1);
            Field areaField = customerForm.CreateSystemField("area", "区域", true, true, 2);
            Field salesUsersField = customerForm.CreateSystemField("salesUsers", "销售员", true, true, 3);
            Field creatorField = customerForm.CreateSystemField("createUser", "创建人", true, false, 4);
            Field createTimeField = customerForm.CreateSystemField("createTime", "创建时间", true, false, 5);
            Field field1 = customerForm.CreateField(null, "地址1", false, false, false, 6, FieldType.String, new StringFieldConfig("地址1"));
            Field field2 = customerForm.CreateField(null, "地址2", false, false, true, 7, FieldType.String, new StringFieldConfig("地址2"));
            ListFieldConfig listFieldConfig = new ListFieldConfig("1", new List<string> { "1", "2", "3" });
            customerForm.CreateField(null, "地址3", false, false, false, 8, FieldType.DropdownList, listFieldConfig);

            GridView customerManageview = this.CrmManager.GridViewManager.CreateSystemGridView(this.Admin, GridViewType.CustomerManage);
            customerManageview.CreateColumn(nameField, 120);
            customerManageview.CreateColumn(areaField, 80);
            customerManageview.CreateColumn(salesUsersField, 80);
            customerManageview.CreateColumn(field1, 80);
            customerManageview.CreateColumn(field2, 80);

            GridView customerFavoriteview = this.CrmManager.GridViewManager.CreateSystemGridView(this.Admin, GridViewType.CustomerFavorite);
            customerFavoriteview.CreateColumn(nameField, 120);
            customerFavoriteview.CreateColumn(areaField, 80);
            customerFavoriteview.CreateColumn(salesUsersField, 80);
            customerFavoriteview.CreateColumn(field1, 80);
            customerFavoriteview.CreateColumn(field2, 80);

            List<PropertyOperationInfo> propertys = new List<PropertyOperationInfo>();
            propertys.Add(new PropertyOperationInfo { Code = field1.Code, Value = "地址1"});
            propertys.Add(new PropertyOperationInfo { Code = field2.Code, Value = "地址2"});

            Customer customer = this.CustomerManager.Create(new CustomerCreateInfo { OpUser = this.Admin, Name = "Burlington Textiles Corp of America", Area = area, SalesUsers = salesUsers, PropertyInfos = propertys });
            return customer;
        }

        private Contact CreateContact(Customer customer)
        {
            Form contactForm = this.CrmManager.FormManager.Create(FormType.Contact, "联系人信息");
            Field nameField = contactForm.CreateSystemField("name", "姓名", true, true, 1);
            Field customerField = contactForm.CreateSystemField("customer", "客户", true, true, 2);
            Field qqField = contactForm.CreateField(null, "QQ", false, false, true, 3, FieldType.String, new StringFieldConfig("1"));
            Field remarkField = contactForm.CreateField(null, "备注", false, false, true, 4, FieldType.Text, new StringFieldConfig(""));

            GridView contactManageview = this.CrmManager.GridViewManager.CreateSystemGridView(this.Admin, GridViewType.ContactManage);
            contactManageview.CreateColumn(nameField, 100);
            contactManageview.CreateColumn(customerField, 80);
            contactManageview.CreateColumn(qqField, 80);
            contactManageview.CreateColumn(remarkField, 80);

            List<PropertyOperationInfo> contactPropertys = new List<PropertyOperationInfo>();
            contactPropertys.Add(new PropertyOperationInfo { Code = qqField.Code, Value = "214434311"});
            contactPropertys.Add(new PropertyOperationInfo { Code = remarkField.Code, Value = "地址收到是说的都是2"});

            Contact contact = this.CrmManager.ContactManager.Create(new ContactCreateInfo { OpUser = this.Admin, Name = "America", Customer = customer, PropertyInfos = contactPropertys });
            return contact;
        }

        private Activity CreateActivity(Contact contact)
        {
            Form activityForm = this.CrmManager.FormManager.Create(FormType.Activity, "客户接触信息");
            Field subjectField = activityForm.CreateSystemField("subject", "主题", true, true,1);
            Field customerField = activityForm.CreateSystemField("customer", "客户", true, true,2);
            Field contactField = activityForm.CreateSystemField("contact", "联系人", true, true,3);
            Field contentField = activityForm.CreateField(null, "内容", false, false, true, 4, FieldType.Text, new StringFieldConfig(""));

            GridView activityManageview = this.CrmManager.GridViewManager.CreateSystemGridView(this.Admin, GridViewType.ActivityManage);
            activityManageview.CreateColumn(subjectField, 100);
            activityManageview.CreateColumn(customerField, 180);
            activityManageview.CreateColumn(contactField, 80);
            activityManageview.CreateColumn(contentField, 80);

            List<PropertyOperationInfo> activityPropertys = new List<PropertyOperationInfo>();
            activityPropertys.Add(new PropertyOperationInfo { Code = contentField.Code, Value = "地址收到是说的都是2"});

            Activity activity = this.CrmManager.ActivityManager.Create(new ActivityCreateInfo { OpUser = this.Admin, Subject = "America", Contact = contact, PropertyInfos = activityPropertys });
            return activity;
        }

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
