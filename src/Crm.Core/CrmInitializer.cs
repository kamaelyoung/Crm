using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Crm.Api;
using Coldew.Core.Organization;
using Coldew.Core;
using Coldew.Api;
using Coldew.Core.UI;
using Coldew.Api.UI;

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
                List<ColdewObject> forms = this._crmManager.ObjectManager.GetForms();
                if (forms.Count == 0)
                {
                    this.InitConfig();
                    this.InitAreas();
                    ColdewObject customerForm = this.InitCustomer();
                    ColdewObject contactForm = this.InitContact();
                    ColdewObject activityForm = this.InitActivity();
                    this.InitContract();

                    PropertySettingDictionary customerPropertys = new PropertySettingDictionary();
                    customerPropertys.Add(ColdewObjectCode.FIELD_NAME_NAME, "中华人民");
                    customerPropertys.Add(CrmObjectConstCode.CUST_FIELD_NAME_AREA, this._crmManager.AreaManager.GetAllArea()[0].ID.ToString());
                    customerPropertys.Add(CrmObjectConstCode.CUST_FIELD_NAME_SALES_USERS, "user1");
                    Metadata customer = customerForm.MetadataManager.Create(this._admin, customerPropertys);

                    PropertySettingDictionary contactPropertys = new PropertySettingDictionary();
                    contactPropertys.Add(ColdewObjectCode.FIELD_NAME_NAME, "李先生");
                    contactPropertys.Add(CrmObjectConstCode.FIELD_NAME_CUSTOMER, customer.ID);
                    contactForm.MetadataManager.Create(this._admin, contactPropertys);


                    
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

        private CustomerObject InitCustomer()
        {
            this._crmManager.Logger.Info("init customer");
            CustomerObject cobject = this._crmManager.ObjectManager.Create("客户", CrmObjectConstCode.FORM_CUSTOMER) as CustomerObject;
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "客户名称", true, true, true, 1, "");
            Field areaField = cobject.CreateCustomerAreaField(CrmObjectConstCode.CUST_FIELD_NAME_AREA, "区域", true, false, true, 2);
            Field salesUsersField = cobject.CreateUserListField(CrmObjectConstCode.CUST_FIELD_NAME_SALES_USERS, "销售员", true, true, true, 3, true);
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", true, false, false, 4, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", false, false, false, 5, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", true, false, false, 6, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", false, false, false, 7, true);
            Field addressField = cobject.CreateStringField(null, "地址", false, false, false, 8, "");
            Field phoneField = cobject.CreateStringField(null, "电话", false, false, true, 9, "");
            Field stateField = cobject.CreateDropdownField(null, "客户级别", false, false, true, 10, null, new List<string> { "潜在", "机会", "重要", "放弃" });
            Field websiteField = cobject.CreateStringField(null, "网站", false, false, true, 11, "");
            Field souceField = cobject.CreateDropdownField(null, "客户来源", false, false, true, 12, null, new List<string> { "搜索引擎", "代理商" });
            Field remarkField = cobject.CreateTextField(null, "备注", false, false, true, 13, "");

            List<Input> baseSectuibInputs = new List<Input>();
            baseSectuibInputs.Add(new Input(nameField, 1));
            baseSectuibInputs.Add(new Input(areaField, 2 ));
            baseSectuibInputs.Add(new Input(salesUsersField, 3 ));
            baseSectuibInputs.Add(new Input(addressField, 4 ));
            baseSectuibInputs.Add(new Input(phoneField, 5 ));
            baseSectuibInputs.Add(new Input(stateField, 6 ));
            baseSectuibInputs.Add(new Input(websiteField, 7 ));
            baseSectuibInputs.Add(new Input(souceField, 8 ));
            List<Section> sections = new List<Section>();
            sections.Add(new Section("基本信息", 2, baseSectuibInputs ));
            List<Input> reamarkSectuibInputs = new List<Input>();
            reamarkSectuibInputs.Add(new Input(remarkField, 1 ));
            sections.Add(new Section("备注信息", 1, reamarkSectuibInputs));

            Form createForm = cobject.FormManager.Create(FormConstCode.CreateFormCode, "创建客户", sections, null);
            Form editForm = cobject.FormManager.Create(FormConstCode.EditFormCode, "编辑客户", sections, null);

            baseSectuibInputs.Add(new Input (creatorField,8 ));
            baseSectuibInputs.Add(new Input (createTimeField,9 ));
            baseSectuibInputs.Add(new Input (modifiedUserField,10 ));
            baseSectuibInputs.Add(new Input (modifiedTimeField,11 ));

            List<RelatedObject> relatedObjects = new List<RelatedObject>();
            List<string> contractRelatedFieldCodes = new List<string>();
            contractRelatedFieldCodes.Add(ColdewObjectCode.FIELD_NAME_NAME);
            contractRelatedFieldCodes.Add("position");
            relatedObjects.Add(new RelatedObject(CrmObjectConstCode.FORM_CONTACT, contractRelatedFieldCodes, this._crmManager.ObjectManager));
            Form detailsForm = cobject.FormManager.Create(FormConstCode.DetailsFormCode, "客户信息", sections, relatedObjects);

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = nameField.ID, Width = 180});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = areaField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = salesUsersField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = addressField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = phoneField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = stateField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = websiteField.ID, Width = 80});
            viewColumns.Add(new GridViewColumnSetupInfo{ FieldId = souceField.ID, Width = 80});
            GridView manageView = cobject.GridViewManager.Create(GridViewType.Manage, "", "客户管理", this._admin, true, true, 1, "", viewColumns);
            GridView favoriteView = cobject.GridViewManager.Create(GridViewType.Favorite, "", "收藏客户", this._admin, true, true, 2, "", viewColumns);
            return cobject;
        }

        private ColdewObject InitContact()
        {
            this._crmManager.Logger.Info("init contact");
            ColdewObject cobject = this._crmManager.ObjectManager.Create("联系人", CrmObjectConstCode.FORM_CONTACT);
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "姓名", true, true, true, 1, "");
            Field customerField = cobject.CreateMetadataField(CrmObjectConstCode.FIELD_NAME_CUSTOMER, "客户", true, true, true, 2, CrmObjectConstCode.FORM_CUSTOMER);
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", true, false, false, 3, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", false, false, false, 4, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", true, false, false, 5, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", false, false, false, 6, true);
            Field positionField = cobject.CreateStringField("position", "职位", false, false, true, 6, "");
            Field deptField = cobject.CreateStringField(null, "部门", false, false, true, 7, "");
            Field sexField = cobject.CreateDropdownField(null, "性别", false, false, true, 8, null, new List<string> { "男", "女" });
            Field phoneField = cobject.CreateStringField(null, "联系电话", false, false, true, 9, "");
            Field qqField = cobject.CreateStringField(null, "QQ", false, false, true, 10, "");
            Field emailField = cobject.CreateStringField(null, "邮件", false, false, true, 11, "");
            Field remarkField = cobject.CreateTextField(null, "备注", false, false, true, 12, "");

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = nameField.ID, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = customerField.ID, Width = 180 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = positionField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = deptField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = sexField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = phoneField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = qqField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = emailField.ID, Width = 80 });
            GridView manageView = cobject.GridViewManager.Create(GridViewType.Manage, "", "联系人管理", this._admin, true, true, 1, "", viewColumns);

            GridView favoriteView = cobject.GridViewManager.Create(GridViewType.Favorite, "", "收藏联系人", this._admin, true, true, 2, "", viewColumns);
            return cobject;
        }

        private ColdewObject InitActivity()
        {
            this._crmManager.Logger.Info("init activity");
            ColdewObject cobject = this._crmManager.ObjectManager.Create("客户接触", CrmObjectConstCode.FORM_Activity);
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "主题", true, true, true, 1, "");
            Field customerField = cobject.CreateMetadataField(CrmObjectConstCode.FIELD_NAME_CUSTOMER, "客户", false, true, false, 2, CrmObjectConstCode.FORM_CUSTOMER);
            Field contactField = cobject.CreateMetadataField(CrmObjectConstCode.FIELD_NAME_CONTACT, "联系人", true, true, true, 2, CrmObjectConstCode.FORM_CONTACT);
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", true, false, false, 3, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", false, false, false, 4, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", true, false, false, 5, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", false, false, false, 6, true);
            Field wayField = cobject.CreateDropdownField(null, "联系方式", false, false, true, 1, null, new List<string> { "电话", "QQ", "Email", "到现场" });
            Field contentField = cobject.CreateTextField(null, "内容", false, false, true, 2, "");

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = nameField.ID, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = customerField.ID, Width = 180 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = contactField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = creatorField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = createTimeField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = wayField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = contentField.ID, Width = 80 });

            GridView manageView = cobject.GridViewManager.Create(GridViewType.Manage, "", "接触管理", this._admin, true, true, 1, "", viewColumns);

            GridView favoriteView = cobject.GridViewManager.Create(GridViewType.Favorite, "", "收藏接触", this._admin, true, true, 2, "", viewColumns);
            return cobject;
        }

        private void InitContract()
        {
            this._crmManager.Logger.Info("init contract");
            ColdewObject cobject = this._crmManager.ObjectManager.Create("合同", CrmObjectConstCode.FORM_Contract);
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "名称", true, true, true, 1, "");
            Field customerField = cobject.CreateMetadataField(CrmObjectConstCode.FIELD_NAME_CUSTOMER, "客户", true, true, true, 2, CrmObjectConstCode.FORM_CUSTOMER);
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", true, false, false, 3, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", false, false, false, 4, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", true, false, false, 5, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", false, false, false, 6, true);
            Field ownersField = cobject.CreateUserListField(CrmObjectConstCode.CONTRACT_FIELD_NAME_OWNER_USERS, "拥有者", true, true, true, 7, true);
            Field startDateField = cobject.CreateDateField("startDate", "开始时间", true, true, true, 8, true);
            Field endDateField = cobject.CreateDateField(CrmObjectConstCode.CONTRACT_FIELD_NAME_END_DATE, "结束时间", true, true, true, 9, true);
            Field valueField = cobject.CreateNumberField("value", "合同金额", true, true, true, 10, null, null, null, 2);
            Field expiredNotifyDaysField = cobject.CreateNumberField(CrmObjectConstCode.CONTRACT_FIELD_NAME_ExpiredComputeDays, "到期计算天数", true, true, true, 11, null, null, null, 0);
            Field clauseField = cobject.CreateStringField(null, "特别条款", false, true, true, 12, "");
            Field contentField = cobject.CreateTextField(null, "备注", false, true, true, 13, "");

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = nameField.ID, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = customerField.ID, Width = 180 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = startDateField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = endDateField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = ownersField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = valueField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = clauseField.ID, Width = 80 });

            GridView manageView = cobject.GridViewManager.Create(GridViewType.Manage, "", "合同管理", this._admin, true, true, 1, "", viewColumns);
            GridView favoriteView = cobject.GridViewManager.Create(GridViewType.Favorite, "", "收藏合同", this._admin, true, true, 2, "", viewColumns);
            GridView expiringView = cobject.GridViewManager.Create(GridViewType.Customized, CrmObjectConstCode.GRID_VIEW_CODE_EXPIRING_CONTRACT, "快到期合同", this._admin, true, true, 3, "", viewColumns);
            GridView expiredView = cobject.GridViewManager.Create(GridViewType.Customized, CrmObjectConstCode.GRID_VIEW_CODE_EXPIRED_CONTRACT, "已到期合同", this._admin, true, true, 4, "", viewColumns);
        }
    }
}
