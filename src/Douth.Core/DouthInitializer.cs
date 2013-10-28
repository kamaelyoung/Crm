using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Core.Organization;
using Coldew.Api.UI;
using Coldew.Api;
using Coldew.Core.UI;

namespace Douth.Core
{
    public class DouthInitializer
    {
        User _admin;
        ColdewManager _coldewManager;
        public DouthInitializer(ColdewManager crmManager)
        {
            this._coldewManager = crmManager;
            this._admin = crmManager.OrgManager.UserManager.GetUserByAccount("admin");
            this.Init();
        }

        void Init()
        {
            try
            {
                List<ColdewObject> objects = this._coldewManager.ObjectManager.GetForms();
                if (objects.Count == 0)
                {
                    this.InitConfig();
                    this.InitContract();
                    this.InitShouquanJihusa();
                    this.InitJiaohuoJihusa();
                    this.InitFapiao();
                }
            }
            catch(Exception ex)
            {
                this._coldewManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        private void InitConfig()
        {
            this._coldewManager.ConfigManager.SetEmailConfig("2593975773", "2593975773@qq.com", "qwert12345", "smtp.qq.com");
        }

        private void InitContract()
        {
            this._coldewManager.Logger.Info("init contract");
            ColdewObject cobject = this._coldewManager.ObjectManager.Create("合同", DouthObjectConstCode.FORM_Contract);
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "名称", true, true, true, 1, "");
            Field customerField = cobject.CreateStringField(DouthObjectConstCode.FIELD_NAME_CUSTOMER, "客户", true, true, true, 2, "");
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", true, false, false, 3, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", false, false, false, 4, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", true, false, false, 5, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", false, false, false, 6, true);
            Field ownersField = cobject.CreateUserListField(DouthObjectConstCode.CONTRACT_FIELD_NAME_OWNER_USERS, "拥有者", true, true, true, 7, true);
            Field startDateField = cobject.CreateDateField("startDate", "开始时间", true, true, true, 8, true);
            Field endDateField = cobject.CreateDateField(DouthObjectConstCode.CONTRACT_FIELD_NAME_END_DATE, "结束时间", true, true, true, 9, true);
            Field valueField = cobject.CreateNumberField("value", "合同金额", true, true, true, 10, null, null, null, 2);
            Field expiredNotifyDaysField = cobject.CreateNumberField(DouthObjectConstCode.CONTRACT_FIELD_NAME_ExpiredComputeDays, "到期计算天数", true, true, true, 11, null, null, null, 0);
            Field clauseField = cobject.CreateStringField(null, "特别条款", false, true, true, 12, "");
            Field remarkField = cobject.CreateTextField(null, "备注", false, true, true, 13, "");

            List<Input> baseSectuibInputs = new List<Input>();
            baseSectuibInputs.Add(new Input(nameField, 1));
            baseSectuibInputs.Add(new Input(customerField, 2));
            baseSectuibInputs.Add(new Input(ownersField, 3));
            baseSectuibInputs.Add(new Input(startDateField, 4));
            baseSectuibInputs.Add(new Input(endDateField, 5));
            baseSectuibInputs.Add(new Input(valueField, 6));
            baseSectuibInputs.Add(new Input(expiredNotifyDaysField, 7));
            baseSectuibInputs.Add(new Input(clauseField, 8));
            List<Section> sections = new List<Section>();
            sections.Add(new Section("基本信息", 2, baseSectuibInputs));
            List<Input> reamarkSectuibInputs = new List<Input>();
            reamarkSectuibInputs.Add(new Input(remarkField, 1));
            sections.Add(new Section("备注信息", 1, reamarkSectuibInputs));

            Form createForm = cobject.FormManager.Create(FormConstCode.CreateFormCode, "创建合同", sections, null);
            Form editForm = cobject.FormManager.Create(FormConstCode.EditFormCode, "编辑合同", sections, null);

            baseSectuibInputs.Add(new Input(creatorField, 9));
            baseSectuibInputs.Add(new Input(createTimeField, 10));
            baseSectuibInputs.Add(new Input(modifiedUserField, 11));
            baseSectuibInputs.Add(new Input(modifiedTimeField, 12));

            List<RelatedObject> relatedObjects = new List<RelatedObject>();

            List<string> shoukuanJihuaRelatedFields = new List<string>();
            shoukuanJihuaRelatedFields.Add(ColdewObjectCode.FIELD_NAME_NAME);
            shoukuanJihuaRelatedFields.Add("jihuaShouKuanRiqi");
            shoukuanJihuaRelatedFields.Add("jihuaShouKuanJine");
            shoukuanJihuaRelatedFields.Add("shijiShouKuanRiqi");
            shoukuanJihuaRelatedFields.Add("shijiShouKuanJine");
            relatedObjects.Add(new RelatedObject("shoukuanJihua", shoukuanJihuaRelatedFields, this._coldewManager.ObjectManager));

            List<string> jiaohuoJihuaRelatedFields = new List<string>();
            jiaohuoJihuaRelatedFields.Add(ColdewObjectCode.FIELD_NAME_NAME);
            jiaohuoJihuaRelatedFields.Add("jiaohuoRiqi");
            jiaohuoJihuaRelatedFields.Add("jiaohuoShuliang");
            jiaohuoJihuaRelatedFields.Add("shijiRiqi");
            jiaohuoJihuaRelatedFields.Add("shijiShuliang");
            relatedObjects.Add(new RelatedObject("jiaohuoJihua", jiaohuoJihuaRelatedFields, this._coldewManager.ObjectManager));

            List<string> fapiaoRelatedFields = new List<string>();
            fapiaoRelatedFields.Add(ColdewObjectCode.FIELD_NAME_NAME);
            fapiaoRelatedFields.Add("kaipiaoren");
            fapiaoRelatedFields.Add("kaipiaoRiqi");
            fapiaoRelatedFields.Add("fapiaoJine");
            fapiaoRelatedFields.Add("weikaipiaoJinge");
            relatedObjects.Add(new RelatedObject("fapiao", fapiaoRelatedFields, this._coldewManager.ObjectManager));

            Form detailsForm = cobject.FormManager.Create(FormConstCode.DetailsFormCode, "合同信息", sections, relatedObjects);

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
            GridView expiringView = cobject.GridViewManager.Create(GridViewType.Customized, DouthObjectConstCode.GRID_VIEW_CODE_EXPIRING_CONTRACT, "快到期合同", this._admin, true, true, 3, "", viewColumns);
            GridView expiredView = cobject.GridViewManager.Create(GridViewType.Customized, DouthObjectConstCode.GRID_VIEW_CODE_EXPIRED_CONTRACT, "已到期合同", this._admin, true, true, 4, "", viewColumns);
        }

        private void InitShouquanJihusa()
        {
            ColdewObject cobject = this._coldewManager.ObjectManager.Create("收款计划", "shoukuanJihua");
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "名称", true, true, true, 1, "");
            Field contractField = cobject.CreateMetadataField("contractName", "合同名称", true, true, true, 2, DouthObjectConstCode.FORM_Contract);
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", true, false, false, 3, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", false, false, false, 4, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", true, false, false, 5, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", false, false, false, 6, true);
            Field jihuaShouKuanRiqiField = cobject.CreateDateField("jihuaShouKuanRiqi", "计划收款日期", true, true, true, 7, true);
            Field jihuaShouKuanJineField = cobject.CreateNumberField("jihuaShouKuanJine", "计划收款金额", true, true, true, 8, null, null, null, 2);
            Field shijiShoukuanKuanRiqiField = cobject.CreateDateField("shijiShouKuanRiqi", "实际收款日期", false, true, true, 9, false);
            Field shijiJineField = cobject.CreateNumberField("shijiShouKuanJine", "实际收款金额", false, true, true, 10, null, null, null, 2);
            Field remarkField = cobject.CreateTextField(null, "备注", false, true, true, 11, "");

            List<Input> baseSectuibInputs = new List<Input>();
            baseSectuibInputs.Add(new Input(nameField, 1));
            baseSectuibInputs.Add(new Input(contractField, 2));
            baseSectuibInputs.Add(new Input(jihuaShouKuanRiqiField, 3));
            baseSectuibInputs.Add(new Input(jihuaShouKuanJineField, 4));
            baseSectuibInputs.Add(new Input(shijiShoukuanKuanRiqiField, 5));
            baseSectuibInputs.Add(new Input(shijiJineField, 6));
            List<Section> sections = new List<Section>();
            sections.Add(new Section("基本信息", 2, baseSectuibInputs));
            List<Input> reamarkSectuibInputs = new List<Input>();
            reamarkSectuibInputs.Add(new Input(remarkField, 1));
            sections.Add(new Section("备注信息", 1, reamarkSectuibInputs));

            Form createForm = cobject.FormManager.Create(FormConstCode.CreateFormCode, "创建收款计划", sections, null);
            Form editForm = cobject.FormManager.Create(FormConstCode.EditFormCode, "编辑收款计划", sections, null);

            baseSectuibInputs.Add(new Input(creatorField, 9));
            baseSectuibInputs.Add(new Input(createTimeField, 10));
            baseSectuibInputs.Add(new Input(modifiedUserField, 11));
            baseSectuibInputs.Add(new Input(modifiedTimeField, 12));

            Form detailsForm = cobject.FormManager.Create(FormConstCode.DetailsFormCode, "收款计划", sections, null);

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = nameField.ID, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = contractField.ID, Width = 180 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = creatorField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = createTimeField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = jihuaShouKuanJineField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = jihuaShouKuanRiqiField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = shijiJineField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = shijiShoukuanKuanRiqiField.ID, Width = 80 });

            GridView manageView = cobject.GridViewManager.Create(GridViewType.Manage, "", "收款计划管理", this._admin, true, true, 1, "", viewColumns);
            GridView favoriteView = cobject.GridViewManager.Create(GridViewType.Favorite, "", "收藏收款计划", this._admin, true, true, 2, "", viewColumns);
        }

        private void InitJiaohuoJihusa()
        {
            ColdewObject cobject = this._coldewManager.ObjectManager.Create("交货计划", "jiaohuoJihua");
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "名称", true, true, true, 1, "");
            Field contractField = cobject.CreateMetadataField("contractName", "合同名称", true, true, true, 2, DouthObjectConstCode.FORM_Contract);
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", true, false, false, 3, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", false, false, false, 4, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", true, false, false, 5, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", false, false, false, 6, true);
            Field jihuaRiqiField = cobject.CreateDateField("jiaohuoRiqi", "计划交货日期", true, true, true, 7, true);
            Field jihuaShuliangField = cobject.CreateNumberField("jiaohuoShuliang", "计划交货数量", true, true, true, 8, null, null, null, 2);
            Field shijiRiqiField = cobject.CreateDateField("shijiRiqi", "实际交货日期", false, true, true, 9, false);
            Field shijiShuliangField = cobject.CreateNumberField("shijiShuliang", "实际交货数量", false, true, true, 10, null, null, null, 2);
            Field remarkField = cobject.CreateTextField(null, "备注", false, true, true, 11, "");

            List<Input> baseSectuibInputs = new List<Input>();
            baseSectuibInputs.Add(new Input(nameField, 1));
            baseSectuibInputs.Add(new Input(contractField, 2));
            baseSectuibInputs.Add(new Input(jihuaRiqiField, 3));
            baseSectuibInputs.Add(new Input(jihuaShuliangField, 4));
            baseSectuibInputs.Add(new Input(shijiRiqiField, 5));
            baseSectuibInputs.Add(new Input(shijiShuliangField, 6));
            List<Section> sections = new List<Section>();
            sections.Add(new Section("基本信息", 2, baseSectuibInputs));
            List<Input> reamarkSectuibInputs = new List<Input>();
            reamarkSectuibInputs.Add(new Input(remarkField, 1));
            sections.Add(new Section("备注信息", 1, reamarkSectuibInputs));

            Form createForm = cobject.FormManager.Create(FormConstCode.CreateFormCode, "创建交货计划", sections, null);
            Form editForm = cobject.FormManager.Create(FormConstCode.EditFormCode, "编辑交货计划", sections, null);

            baseSectuibInputs.Add(new Input(creatorField, 9));
            baseSectuibInputs.Add(new Input(createTimeField, 10));
            baseSectuibInputs.Add(new Input(modifiedUserField, 11));
            baseSectuibInputs.Add(new Input(modifiedTimeField, 12));

            Form detailsForm = cobject.FormManager.Create(FormConstCode.DetailsFormCode, "交货计划", sections, null);

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = nameField.ID, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = contractField.ID, Width = 180 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = creatorField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = createTimeField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = jihuaShuliangField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = jihuaRiqiField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = shijiShuliangField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = shijiRiqiField.ID, Width = 80 });

            GridView manageView = cobject.GridViewManager.Create(GridViewType.Manage, "", "交货计划管理", this._admin, true, true, 1, "", viewColumns);
            GridView favoriteView = cobject.GridViewManager.Create(GridViewType.Favorite, "", "收藏交货计划", this._admin, true, true, 2, "", viewColumns);

        }

        private void InitFapiao()
        {
            ColdewObject cobject = this._coldewManager.ObjectManager.Create("发票", "fapiao");
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "名称", true, true, true, 1, "");
            Field contractField = cobject.CreateMetadataField("contractName", "合同名称", true, true, true, 2, DouthObjectConstCode.FORM_Contract);
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", true, false, false, 3, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", false, false, false, 4, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", true, false, false, 5, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", false, false, false, 6, true);
            Field kaipiaorenField = cobject.CreateStringField("kaipiaoren", "开票人", true, true, true, 7, "");
            Field kaipiaoRiqiField = cobject.CreateDateField("kaipiaoRiqi", "开票日期", true, true, true, 8, true);
            Field fapiaoJineField = cobject.CreateNumberField("fapiaoJine", "开票金额", true, true, true, 9, null, null, null, 2);
            Field weikaipiaoJingeField = cobject.CreateNumberField("weikaipiaoJinge", "未开票金额", true, true, true, 12, null, null, null, 2);
            Field remarkField = cobject.CreateTextField(null, "备注", false, true, true, 14, "");

            List<Input> baseSectuibInputs = new List<Input>();
            baseSectuibInputs.Add(new Input(nameField, 1));
            baseSectuibInputs.Add(new Input(contractField, 2));
            baseSectuibInputs.Add(new Input(kaipiaorenField, 3));
            baseSectuibInputs.Add(new Input(kaipiaoRiqiField, 4));
            baseSectuibInputs.Add(new Input(fapiaoJineField, 5));
            baseSectuibInputs.Add(new Input(weikaipiaoJingeField, 6));
            List<Section> sections = new List<Section>();
            sections.Add(new Section("基本信息", 2, baseSectuibInputs));
            List<Input> reamarkSectuibInputs = new List<Input>();
            reamarkSectuibInputs.Add(new Input(remarkField, 1));
            sections.Add(new Section("备注信息", 1, reamarkSectuibInputs));

            Form createForm = cobject.FormManager.Create(FormConstCode.CreateFormCode, "创建开票", sections, null);
            Form editForm = cobject.FormManager.Create(FormConstCode.EditFormCode, "编辑开票", sections, null);

            baseSectuibInputs.Add(new Input(creatorField, 9));
            baseSectuibInputs.Add(new Input(createTimeField, 10));
            baseSectuibInputs.Add(new Input(modifiedUserField, 11));
            baseSectuibInputs.Add(new Input(modifiedTimeField, 12));

            Form detailsForm = cobject.FormManager.Create(FormConstCode.DetailsFormCode, "开票信息", sections, null);

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = nameField.ID, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = contractField.ID, Width = 180 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = kaipiaorenField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = kaipiaoRiqiField.ID, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldId = weikaipiaoJingeField.ID, Width = 80 });

            GridView manageView = cobject.GridViewManager.Create(GridViewType.Manage, "", "开票管理", this._admin, true, true, 1, "", viewColumns);
            GridView favoriteView = cobject.GridViewManager.Create(GridViewType.Favorite, "", "收藏开票", this._admin, true, true, 2, "", viewColumns);
        }
    }
}
