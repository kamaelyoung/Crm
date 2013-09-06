using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Crm.Data.Organization;
using Crm.Api.Organization;
using Crm.Core.Extend;
using Crm.Core.Organization;
using Crm.Api;

namespace Crm.Core
{
    public class CrmInitializer
    {
        User _admin;
        CrmManager _crmManager;
        public CrmInitializer(CrmManager crmManager)
        {
            this._crmManager = crmManager;
            this._admin = crmManager.OrgManager.UserManager.GetUserByAccount("admin");
            this.Init();
        }

        void Init()
        {
            try
            {
                Form customerForm = this._crmManager.FormManager.GetForm(Api.FormType.Customer);
                if (customerForm == null)
                {
                    this.InitConfig();
                    this.InitAreas();
                    this.InitCustomer();
                    this.InitContact();
                    this.InitActivity();
                    this.InitContract();
                    
                }
            }
            catch(Exception ex)
            {
                this._crmManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        private void InitConfig()
        {
            this._crmManager.ConfigManager.SetEmailConfig("2593975773", "2593975773@qq.com", "qwert12345", "smtp.qq.com");
        }

        private void InitAreas()
        {
            this._crmManager.Logger.Info("init areas");
            this._crmManager.AreaManager.Create("华南区", null);
            this._crmManager.AreaManager.Create("东北区", null);
            this._crmManager.AreaManager.Create("西南区", null);
        }

        private void InitCustomer()
        {
            this._crmManager.Logger.Info("init customer");
            Form form = this._crmManager.FormManager.Create(FormType.Customer, "客户信息");
            Field nameField = form.CreateSystemField("name", "客户名称", true, true, 0);
            Field areaField = form.CreateSystemField("area", "区域", true, true, 0);
            Field salesUsersField = form.CreateSystemField("salesUsers", "销售员", true, true, 0);
            Field creatorField = form.CreateSystemField("creator", "创建人", true, false, 0);
            Field createTimeField = form.CreateSystemField("createTime", "创建时间", true, false, 0);
            Field addressField = form.CreateField(null, "地址", false, false, false, 1, FieldType.String, new StringFieldConfig(""));
            Field phoneField = form.CreateField(null, "电话", false, false, true, 2, FieldType.String, new StringFieldConfig(""));
            ListFieldConfig stateListFieldConfig = new ListFieldConfig(null, new List<string> { "潜在", "机会", "重要", "放弃" });
            Field stateField = form.CreateField(null, "客户级别", false, false, true, 3, FieldType.DropdownList, stateListFieldConfig);
            Field websiteField = form.CreateField(null, "网站", false, false, true, 4, FieldType.String, new StringFieldConfig(""));
            ListFieldConfig souceListFieldConfig = new ListFieldConfig(null, new List<string> { "搜索引擎", "代理商"});
            Field souceField = form.CreateField(null, "客户来源", false, false, true, 5, FieldType.DropdownList, stateListFieldConfig);
            Field remarkField = form.CreateField(null, "备注", false, false, true, 6, FieldType.Text, new StringFieldConfig(""));

            GridView manageView = this._crmManager.GridViewManager.CreateSystemGridView(this._admin, GridViewType.CustomerManage);
            manageView.CreateColumn(nameField, 180);
            manageView.CreateColumn(areaField, 80);
            manageView.CreateColumn(salesUsersField, 80);
            manageView.CreateColumn(addressField, 80);
            manageView.CreateColumn(phoneField, 80);
            manageView.CreateColumn(stateField, 80);
            manageView.CreateColumn(websiteField, 80);
            manageView.CreateColumn(souceField, 80);

            GridView favoriteView = this._crmManager.GridViewManager.CreateSystemGridView(this._admin, GridViewType.CustomerFavorite);
            favoriteView.CreateColumn(nameField, 180);
            favoriteView.CreateColumn(areaField, 80);
            favoriteView.CreateColumn(salesUsersField, 80);
            favoriteView.CreateColumn(addressField, 80);
            favoriteView.CreateColumn(phoneField, 80);
            favoriteView.CreateColumn(stateField, 80);
            favoriteView.CreateColumn(websiteField, 80);
            favoriteView.CreateColumn(souceField, 80);
        }

        private void InitContact()
        {
            this._crmManager.Logger.Info("init contact");
            Form form = this._crmManager.FormManager.Create(FormType.Contact, "联系人信息");
            Field nameField = form.CreateSystemField("name", "姓名", true, true, 0);
            Field customerField = form.CreateSystemField("customer", "客户", true, true, 0);
            Field creatorField = form.CreateSystemField("creator", "创建人", true, false, 0);
            Field createTimeField = form.CreateSystemField("createTime", "创建时间", true, false, 0);
            Field positionField = form.CreateField(null, "职位", false, false, true, 1, FieldType.String, new StringFieldConfig(""));
            Field deptField = form.CreateField(null, "部门", false, false, true, 2, FieldType.String, new StringFieldConfig(""));
            ListFieldConfig sexListFieldConfig = new ListFieldConfig(null, new List<string> { "男", "女" });
            Field sexField = form.CreateField(null, "性别", false, false, true, 3, FieldType.DropdownList, sexListFieldConfig);
            Field phoneField = form.CreateField(null, "联系电话", false, false, true, 4, FieldType.String, new StringFieldConfig(""));
            Field qqField = form.CreateField(null, "QQ", false, false, true, 5, FieldType.String, new StringFieldConfig(""));
            Field emailField = form.CreateField(null, "邮件", false, false, true, 6, FieldType.String, new StringFieldConfig(""));
            Field remarkField = form.CreateField(null, "备注", false, false, true, 7, FieldType.Text, new StringFieldConfig(""));

            GridView contactManageview = this._crmManager.GridViewManager.CreateSystemGridView(this._admin, GridViewType.ContactManage);
            contactManageview.CreateColumn(nameField, 100);
            contactManageview.CreateColumn(customerField, 180);
            contactManageview.CreateColumn(positionField, 80);
            contactManageview.CreateColumn(deptField, 80);
            contactManageview.CreateColumn(sexField, 80);
            contactManageview.CreateColumn(phoneField, 80);
            contactManageview.CreateColumn(qqField, 80);
        }

        private void InitActivity()
        {
            this._crmManager.Logger.Info("init activity");
            Form form = this._crmManager.FormManager.Create(FormType.Activity, "客户接触信息");
            Field subjectField = form.CreateSystemField("subject", "主题", true, true, 0);
            Field customerField = form.CreateSystemField("customer", "客户", true, true, 0);
            Field contactField = form.CreateSystemField("contact", "联系人", true, true, 0);
            Field creatorField = form.CreateSystemField("creator", "创建人", true, false, 0);
            Field createTimeField = form.CreateSystemField("createTime", "创建时间", true, false, 0);
            ListFieldConfig wayListFieldConfig = new ListFieldConfig(null, new List<string> { "电话", "QQ", "Email", "到现场" });
            Field wayField = form.CreateField(null, "联系方式", false, false, true, 1, FieldType.DropdownList, wayListFieldConfig);
            Field contentField = form.CreateField(null, "内容", false, false, true, 2, FieldType.Text, new StringFieldConfig(""));

            GridView manageView = this._crmManager.GridViewManager.CreateSystemGridView(this._admin, GridViewType.ActivityManage);
            manageView.CreateColumn(subjectField, 100);
            manageView.CreateColumn(customerField, 180);
            manageView.CreateColumn(creatorField, 80);
            manageView.CreateColumn(createTimeField, 80);
            manageView.CreateColumn(contactField, 80);
            manageView.CreateColumn(wayField, 80);
            manageView.CreateColumn(contentField, 80);
        }

        private void InitContract()
        {
            this._crmManager.Logger.Info("init contract");
            Form form = this._crmManager.FormManager.Create(FormType.Contract, "合同信息");
            Field nameField = form.CreateSystemField("name", "名称", true, true, 0);
            Field customerField = form.CreateSystemField("customer", "客户", true, true, 0);
            Field ownersField = form.CreateSystemField("owners", "拥有者", true, true, 0);
            Field startDateField = form.CreateSystemField("startDate", "开始时间", true, true, 0);
            Field endDateField = form.CreateSystemField("endDate", "结束时间", true, true, 0);
            Field valueField = form.CreateSystemField("value", "合同金额", true, true, 0);
            Field expiredNotifyDaysField = form.CreateSystemField("expiredComputeDays", "到期计算天数", true, true, 0);
            Field creatorField = form.CreateSystemField("creator", "创建人", true, false, 0);
            Field createTimeField = form.CreateSystemField("createTime", "创建时间", true, false, 0);
            Field clauseField = form.CreateField(null, "特别条款", false, false, true, 1, FieldType.String, new StringFieldConfig(""));
            Field contentField = form.CreateField(null, "备注", false, false, true, 2, FieldType.Text, new StringFieldConfig(""));

            GridView manageView = this._crmManager.GridViewManager.CreateSystemGridView(this._admin, GridViewType.ContractManage);
            manageView.CreateColumn(nameField, 100);
            manageView.CreateColumn(customerField, 180);
            manageView.CreateColumn(startDateField, 80);
            manageView.CreateColumn(endDateField, 80);
            manageView.CreateColumn(ownersField, 80);
            manageView.CreateColumn(valueField, 80);
            manageView.CreateColumn(clauseField, 80);

            GridView expiredView = this._crmManager.GridViewManager.CreateSystemGridView(this._admin, GridViewType.ExpiredContract);
            expiredView.CreateColumn(nameField, 100);
            expiredView.CreateColumn(customerField, 180);
            expiredView.CreateColumn(startDateField, 80);
            expiredView.CreateColumn(endDateField, 80);
            expiredView.CreateColumn(valueField, 80);
            expiredView.CreateColumn(expiredNotifyDaysField, 90);
            expiredView.CreateColumn(ownersField, 80);
            expiredView.CreateColumn(clauseField, 80);

            GridView expiringView = this._crmManager.GridViewManager.CreateSystemGridView(this._admin, GridViewType.ExpiringContract);
            expiringView.CreateColumn(nameField, 100);
            expiringView.CreateColumn(customerField, 180);
            expiringView.CreateColumn(startDateField, 80);
            expiringView.CreateColumn(endDateField, 80);
            expiringView.CreateColumn(ownersField, 80);
            expiringView.CreateColumn(valueField, 80);
            expiringView.CreateColumn(clauseField, 80);
        }
    }
}
