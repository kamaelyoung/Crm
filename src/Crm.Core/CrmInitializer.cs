using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Crm.Api;
using Coldew.Core.Organization;
using Coldew.Core;
using Coldew.Api;

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
                List<Form> forms = this._crmManager.FormManager.GetForms();
                if (forms.Count == 0)
                {
                    this.InitConfig();
                    this.InitAreas();
                    Form customerForm = this.InitCustomer();
                    Form contactForm = this.InitContact();
                    Form activityForm = this.InitActivity();

                    PropertySettingDictionary customerPropertys = new PropertySettingDictionary();
                    customerPropertys.Add(FormConstCode.FIELD_NAME_NAME, "中华人民");
                    customerPropertys.Add(CrmFormConstCode.CUST_FIELD_NAME_AREA, this._crmManager.AreaManager.GetAllArea()[0].ID.ToString());
                    customerPropertys.Add(CrmFormConstCode.CUST_FIELD_NAME_SALES_USERS, "user1");
                    Metadata customer = customerForm.MetadataManager.Create(this._admin, customerPropertys);

                    PropertySettingDictionary contactPropertys = new PropertySettingDictionary();
                    contactPropertys.Add(FormConstCode.FIELD_NAME_NAME, "李先生");
                    contactPropertys.Add(CrmFormConstCode.FIELD_NAME_CUSTOMER, customer.ID);
                    contactForm.MetadataManager.Create(this._admin, contactPropertys);


                    //this.InitContract();
                    
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

        private CrmForm InitCustomer()
        {
            this._crmManager.Logger.Info("init customer");
            CrmForm form = this._crmManager.FormManager.Create("客户", CrmFormConstCode.FORM_CUSTOMER) as CrmForm;
            Field nameField = form.CreateStringField(FormConstCode.FIELD_NAME_NAME, "客户名称", true, true, true, 1, "");
            Field areaField = form.CreateCustomerAreaField(CrmFormConstCode.CUST_FIELD_NAME_AREA, "区域", true, false, true, 2);
            Field salesUsersField = form.CreateUserListField(CrmFormConstCode.CUST_FIELD_NAME_SALES_USERS, "销售员", true, true, true, 3, true);
            Field creatorField = form.CreateUserField(FormConstCode.FIELD_NAME_CREATOR, "创建人", true, false, false, 4, true);
            Field createTimeField = form.CreateDateField(FormConstCode.FIELD_NAME_CREATE_TIME, "创建时间", false, false, false, 5, true);
            Field modifiedUserField = form.CreateUserField(FormConstCode.FIELD_NAME_MODIFIED_USER, "修改人", true, false, false, 6, true);
            Field modifiedTimeField = form.CreateDateField(FormConstCode.FIELD_NAME_MODIFIED_TIME, "修改时间", false, false, false, 7, true);
            Field addressField = form.CreateStringField(null, "地址", false, false, false, 8, "");
            Field phoneField = form.CreateStringField(null, "电话", false, false, true, 9, "");
            Field stateField = form.CreateDropdownField(null, "客户级别", false, false, true, 10, null, new List<string> { "潜在", "机会", "重要", "放弃" });
            Field websiteField = form.CreateStringField(null, "网站", false, false, true, 11, "");
            Field souceField = form.CreateDropdownField(null, "客户来源", false, false, true, 12, null, new List<string> { "搜索引擎", "代理商" });
            Field remarkField = form.CreateTextField(null, "备注", false, false, true, 13, "");

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = nameField.ID, Width = 180});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = areaField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = salesUsersField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = addressField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = phoneField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = stateField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = websiteField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = souceField.ID, Width = 80});
            GridView manageView = this._crmManager.GridViewManager.Create(GridViewType.Manage, "客户管理", this._admin, true, true, 1, "", form, viewColumns);

            GridView favoriteView = this._crmManager.GridViewManager.Create(GridViewType.Favorite, "收藏客户", this._admin, true, true, 2, "", form, viewColumns);
            return form;
        }

        private CrmForm InitContact()
        {
            this._crmManager.Logger.Info("init contact");
            CrmForm form = this._crmManager.FormManager.Create("联系人", CrmFormConstCode.FORM_CONTACT) as CrmForm;
            Field nameField = form.CreateStringField(FormConstCode.FIELD_NAME_NAME, "姓名", true, true, true, 1, "");
            Field customerField = form.CreateMetadataField(CrmFormConstCode.FIELD_NAME_CUSTOMER, "客户", true, true, true, 2, CrmFormConstCode.FORM_CUSTOMER);
            Field creatorField = form.CreateUserField(FormConstCode.FIELD_NAME_CREATOR, "创建人", true, false, false, 3, true);
            Field createTimeField = form.CreateDateField(FormConstCode.FIELD_NAME_CREATE_TIME, "创建时间", false, false, false, 4, true);
            Field modifiedUserField = form.CreateUserField(FormConstCode.FIELD_NAME_MODIFIED_USER, "修改人", true, false, false, 5, true);
            Field modifiedTimeField = form.CreateDateField(FormConstCode.FIELD_NAME_MODIFIED_TIME, "修改时间", false, false, false, 6, true);
            Field positionField = form.CreateStringField(null, "职位", false, false, true, 6, "");
            Field deptField = form.CreateStringField(null, "部门", false, false, true, 7, "");
            Field sexField = form.CreateDropdownField(null, "性别", false, false, true, 8, null, new List<string> { "男", "女" });
            Field phoneField = form.CreateStringField(null, "联系电话", false, false, true, 9, "");
            Field qqField = form.CreateStringField(null, "QQ", false, false, true, 10, "");
            Field emailField = form.CreateStringField(null, "邮件", false, false, true, 11, "");
            Field remarkField = form.CreateTextField(null, "备注", false, false, true, 12, "");

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = nameField.ID, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = customerField.ID, Width = 180 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = positionField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = deptField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = sexField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = phoneField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = qqField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = emailField.ID, Width = 80 });
            GridView manageView = this._crmManager.GridViewManager.Create(GridViewType.Manage, "联系人管理", this._admin, true, true, 1, "", form, viewColumns);

            GridView favoriteView = this._crmManager.GridViewManager.Create(GridViewType.Favorite, "收藏联系人", this._admin, true, true, 2, "", form, viewColumns);
            return form;
        }

        private CrmForm InitActivity()
        {
            this._crmManager.Logger.Info("init activity");
            CrmForm form = this._crmManager.FormManager.Create("客户接触", CrmFormConstCode.FORM_Activity) as CrmForm;
            Field nameField = form.CreateStringField(FormConstCode.FIELD_NAME_NAME, "主题", true, true, true, 1, "");
            Field customerField = form.CreateMetadataField(CrmFormConstCode.FIELD_NAME_CUSTOMER, "客户", false, true, false, 2, CrmFormConstCode.FORM_CUSTOMER);
            Field contactField = form.CreateMetadataField(CrmFormConstCode.FIELD_NAME_CONTACT, "联系人", true, true, true, 2, CrmFormConstCode.FORM_CONTACT);
            Field creatorField = form.CreateUserField(FormConstCode.FIELD_NAME_CREATOR, "创建人", true, false, false, 3, true);
            Field createTimeField = form.CreateDateField(FormConstCode.FIELD_NAME_CREATE_TIME, "创建时间", false, false, false, 4, true);
            Field modifiedUserField = form.CreateUserField(FormConstCode.FIELD_NAME_MODIFIED_USER, "修改人", true, false, false, 5, true);
            Field modifiedTimeField = form.CreateDateField(FormConstCode.FIELD_NAME_MODIFIED_TIME, "修改时间", false, false, false, 6, true);
            Field wayField = form.CreateDropdownField(null, "联系方式", false, false, true, 1, null, new List<string> { "电话", "QQ", "Email", "到现场" });
            Field contentField = form.CreateTextField(null, "内容", false, false, true, 2, "");

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = nameField.ID, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = customerField.ID, Width = 180 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = contactField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = creatorField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = createTimeField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = wayField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = contentField.ID, Width = 80 });

            GridView manageView = this._crmManager.GridViewManager.Create(GridViewType.Manage, "接触管理", this._admin, true, true, 1, "", form, viewColumns);

            GridView favoriteView = this._crmManager.GridViewManager.Create(GridViewType.Favorite, "收藏接触", this._admin, true, true, 2, "", form, viewColumns);
            return form;
        }

        private void InitContract()
        {
            //this._crmManager.Logger.Info("init contract");
            //Form form = this._crmManager.FormManager.Create(FormType.Contract, "合同信息");
            //Field nameField = form.CreateSystemField("name", "名称", true, true, 0);
            //Field customerField = form.CreateSystemField("customer", "客户", true, true, 0);
            //Field ownersField = form.CreateSystemField("owners", "拥有者", true, true, 0);
            //Field startDateField = form.CreateSystemField("startDate", "开始时间", true, true, 0);
            //Field endDateField = form.CreateSystemField("endDate", "结束时间", true, true, 0);
            //Field valueField = form.CreateSystemField("value", "合同金额", true, true, 0);
            //Field expiredNotifyDaysField = form.CreateSystemField("expiredComputeDays", "到期计算天数", true, true, 0);
            //Field creatorField = form.CreateSystemField("creator", "创建人", true, false, 0);
            //Field createTimeField = form.CreateSystemField("createTime", "创建时间", true, false, 0);
            //Field clauseField = form.CreateStringField(null, "特别条款", false, false, true, 1, "");
            //Field contentField = form.CreateTextField(null, "备注", false, false, true, 2, "");

            //GridView manageView = this._crmManager.GridViewManager.CreateSystemGridView(this._admin, GridViewType.ContractManage);
            //manageView.CreateColumn(nameField, 100);
            //manageView.CreateColumn(customerField, 180);
            //manageView.CreateColumn(startDateField, 80);
            //manageView.CreateColumn(endDateField, 80);
            //manageView.CreateColumn(ownersField, 80);
            //manageView.CreateColumn(valueField, 80);
            //manageView.CreateColumn(clauseField, 80);

            //GridView expiredView = this._crmManager.GridViewManager.CreateSystemGridView(this._admin, GridViewType.ExpiredContract);
            //expiredView.CreateColumn(nameField, 100);
            //expiredView.CreateColumn(customerField, 180);
            //expiredView.CreateColumn(startDateField, 80);
            //expiredView.CreateColumn(endDateField, 80);
            //expiredView.CreateColumn(valueField, 80);
            //expiredView.CreateColumn(expiredNotifyDaysField, 90);
            //expiredView.CreateColumn(ownersField, 80);
            //expiredView.CreateColumn(clauseField, 80);

            //GridView expiringView = this._crmManager.GridViewManager.CreateSystemGridView(this._admin, GridViewType.ExpiringContract);
            //expiringView.CreateColumn(nameField, 100);
            //expiringView.CreateColumn(customerField, 180);
            //expiringView.CreateColumn(startDateField, 80);
            //expiringView.CreateColumn(endDateField, 80);
            //expiringView.CreateColumn(ownersField, 80);
            //expiringView.CreateColumn(valueField, 80);
            //expiringView.CreateColumn(clauseField, 80);
        }
    }
}
