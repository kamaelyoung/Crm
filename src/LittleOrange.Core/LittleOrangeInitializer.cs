using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Core.Organization;
using Coldew.Api.UI;
using Coldew.Api;
using Coldew.Core.UI;
using Coldew.Api.Organization;

namespace LittleOrange.Core
{
    public class LittleOrangeInitializer
    {
        User _admin;
        Group kehuAdminGroup;
        ColdewManager _coldewManager;
        public LittleOrangeInitializer(ColdewManager crmManager)
        {
            this._coldewManager = crmManager;
            this._admin = crmManager.OrgManager.UserManager.GetUserByAccount("admin");
#if DEBUG
            this.Init();
#else
            try
            {
                this.Init();
            }
            catch (Exception ex)
            {
                this._coldewManager.Logger.Error(ex.Message, ex);
                throw;
            }
#endif
        }

        void Init()
        {

            List<ColdewObject> objects = this._coldewManager.ObjectManager.GetObjects();
            if (objects.Count == 0)
            {
                User user1 = this._coldewManager.OrgManager.UserManager.Create(this._coldewManager.OrgManager.System, new UserCreateInfo
                {
                    Name = "user1",
                    Account = "user1",
                    Password = "123456",
                    Status = UserStatus.Normal,
                    MainPositionId = this._coldewManager.OrgManager.PositionManager.TopPosition.ID
                });

                User user2 = this._coldewManager.OrgManager.UserManager.Create(this._coldewManager.OrgManager.System, new UserCreateInfo
                {
                    Name = "user2",
                    Account = "user2",
                    Password = "123456",
                    Status = UserStatus.Normal,
                    MainPositionId = this._coldewManager.OrgManager.PositionManager.TopPosition.ID
                });

                this.kehuAdminGroup = this._coldewManager.OrgManager.GroupManager.Create(this._admin, new GroupCreateInfo { GroupType = GroupType.Group, Name = "管理员" });
                this.kehuAdminGroup.AddUser(this._admin, user1);
                this.kehuAdminGroup.AddUser(this._admin, this._admin);

                this.InitConfig();
                this.InitZiranren();
                this.InitGongsiKehu();
                this.InitFahuo();
                this.InitFahuoLiucheng();
            }
        }

        private void InitConfig()
        {
            this._coldewManager.ConfigManager.SetEmailConfig("2593975773", "2593975773@qq.com", "qwert12345", "smtp.qq.com");
        }

        private void InitZiranren()
        {
            this._coldewManager.Logger.Info("init ziranren");
            ColdewObject cobject = this._coldewManager.ObjectManager.Create(new ColdewObjectCreateInfo("自然人客户", LittleOrangeObjectConstCode.Object_Ziranren, ColdewObjectType.Standard, true, "客户名称"));
            Field shenfenField = cobject.CreateDropdownField(new FieldCreateBaseInfo("shenfen", "省份", "", true, true), "广东省", new List<string> { "广东省" });
            Field diquField = cobject.CreateDropdownField(new FieldCreateBaseInfo("diqu", "地区", "", true, true), null, new List<string> { "天河区", "番禺区" });
            Field yewuyuanField = cobject.CreateUserListField(new FieldCreateBaseInfo("yewuyuan", "业务员", "", true, true), true);
            Field lianxidianhuaField = cobject.CreateStringField(new FieldCreateBaseInfo("lianxidianhua", "联系电话", "", false, true), "");
            Field guakaoGongsiField = cobject.CreateStringField(new FieldCreateBaseInfo("guakaoGongsi", "挂靠公司名称", "配送点、回款周期", false, true), "");
            Field lianxirenField = cobject.CreateStringField(new FieldCreateBaseInfo("lianxiren", "联系人", "传真/邮箱", false, true), "");
            Field yunyiFangsiField = cobject.CreateStringField(new FieldCreateBaseInfo("yunyiFangsi", "运营方式", "有开发团队、招商、个人开发医院", false, true), "");
            Field zhuyinQuyuField = cobject.CreateStringField(new FieldCreateBaseInfo("zhuyinQuyu", "主营区域及医院", "", false, true), "");
            Field zhuyinChanpinField = cobject.CreateStringField(new FieldCreateBaseInfo("zhuyinChanpin", "主营产品及月销量", "", false, true), "");
            Field yixiangChanpinField = cobject.CreateStringField(new FieldCreateBaseInfo("yixiangChanpin", "意向产品", "品名、月销量、区域", false, true), "");
            Field huifangRiqiField = cobject.CreateDateField(new FieldCreateBaseInfo("huifangRiqi", "回访日期", "", false, true), false);
            Field lianxiQingkuangField = cobject.CreateTextField(new FieldCreateBaseInfo("lianxiQingkuang", "联系情况", "", false, true), "");
            Field xiaciHuifangRiqiField = cobject.CreateDateField(new FieldCreateBaseInfo("xiaciHuifangRiqi", "下次回访时间", "", false, true), false);
            Field lianxiDizhiField = cobject.CreateStringField(new FieldCreateBaseInfo("lianxiDizhiRiqi", "联系地址", "", false, true), "");
            Field remarkField = cobject.CreateTextField(new FieldCreateBaseInfo("beizhu", "备注", "", false, true), "");

            List<Input> baseSectuibInputs = new List<Input>();
            baseSectuibInputs.Add(new Input(cobject.NameField));
            baseSectuibInputs.Add(new Input(shenfenField));
            baseSectuibInputs.Add(new Input(yewuyuanField));
            baseSectuibInputs.Add(new Input(diquField));
            baseSectuibInputs.Add(new Input(lianxidianhuaField));
            baseSectuibInputs.Add(new Input(guakaoGongsiField));
            baseSectuibInputs.Add(new Input(lianxirenField));
            baseSectuibInputs.Add(new Input(yunyiFangsiField));
            baseSectuibInputs.Add(new Input(zhuyinQuyuField));
            baseSectuibInputs.Add(new Input(zhuyinChanpinField));
            baseSectuibInputs.Add(new Input(yixiangChanpinField));
            baseSectuibInputs.Add(new Input(lianxiDizhiField));
            List<Section> sections = new List<Section>();
            sections.Add(new Section("基本信息", 2, baseSectuibInputs));

            List<Input> huifangInputs = new List<Input>();
            huifangInputs.Add(new Input(huifangRiqiField));
            huifangInputs.Add(new Input(lianxiQingkuangField));
            huifangInputs.Add(new Input(xiaciHuifangRiqiField));
            sections.Add(new Section("回访信息", 2, huifangInputs));

            List<Input> reamarkSectuibInputs = new List<Input>();
            reamarkSectuibInputs.Add(new Input(remarkField));
            sections.Add(new Section("备注信息", 1, reamarkSectuibInputs));

            Form createForm = cobject.FormManager.Create(FormConstCode.CreateFormCode, "创建自然人客户", sections, null);
            Form editForm = cobject.FormManager.Create(FormConstCode.EditFormCode, "编辑自然人客户", sections, null);

            baseSectuibInputs.Add(new Input(cobject.CreatedUserField));
            baseSectuibInputs.Add(new Input(cobject.CreatedTimeField));
            baseSectuibInputs.Add(new Input(cobject.ModifiedUserField));
            baseSectuibInputs.Add(new Input(cobject.ModifiedTimeField));

            Form detailsForm = cobject.FormManager.Create(FormConstCode.DetailsFormCode, "自然人客户信息", sections, null);


            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = yewuyuanField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = shenfenField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = diquField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = cobject.NameField.Code, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = lianxidianhuaField.Code, Width = 90 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = guakaoGongsiField.Code, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = lianxirenField.Code, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = yunyiFangsiField.Code, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = zhuyinQuyuField.Code, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = zhuyinChanpinField.Code, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = yixiangChanpinField.Code, Width = 100 });

            GridView manageView = cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Standard, "", "自然人客户管理", true, true, "", viewColumns, cobject.CreatedTimeField.Code, this._admin.Account));
            GridView favoriteView = cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Favorite, "", "收藏自然人客户", true, true, "", viewColumns, cobject.CreatedTimeField.Code, this._admin.Account));

            List<GridViewColumnSetupInfo> huifangColumns = new List<GridViewColumnSetupInfo>();
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = yewuyuanField.Code, Width = 50 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = shenfenField.Code, Width = 50 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = diquField.Code, Width = 50 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = cobject.NameField.Code, Width = 80 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = huifangRiqiField.Code, Width = 80 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = xiaciHuifangRiqiField.Code, Width = 80 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = lianxiQingkuangField.Code, Width = 120 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = yunyiFangsiField.Code, Width = 100 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = zhuyinQuyuField.Code, Width = 100 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = zhuyinChanpinField.Code, Width = 100 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = yixiangChanpinField.Code, Width = 100 });

            cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Standard, "jihuaHuifang", "计划回访客户", true, false, null, huifangColumns, xiaciHuifangRiqiField.Code + " desc", this._admin.Account));
            cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Standard, "huifangde", "客户回访", true, false, null, huifangColumns, huifangRiqiField.Code + " desc", this._admin.Account));

            cobject.ObjectPermission.Create(this._coldewManager.OrgManager.Everyone, ObjectPermissionValue.Create | ObjectPermissionValue.View);
            cobject.ObjectPermission.Create(this.kehuAdminGroup, ObjectPermissionValue.All);
            cobject.MetadataPermission.StrategyManager.Create(new MetadataOrgMember(this.kehuAdminGroup), MetadataPermissionValue.All, null);
            cobject.MetadataPermission.StrategyManager.Create(new MetadataFieldMember(yewuyuanField), MetadataPermissionValue.View, null);
            cobject.MetadataPermission.StrategyManager.Create(new MetadataFieldMember(cobject.CreatedUserField), MetadataPermissionValue.All, null);
        }

        private void InitGongsiKehu()
        {
            this._coldewManager.Logger.Info("init gongsiKehu");
            ColdewObject cobject = this._coldewManager.ObjectManager.Create(new ColdewObjectCreateInfo("医药公司客户", LittleOrangeObjectConstCode.Object_GongsiKehu, ColdewObjectType.Standard, true, "公司名称"));
            Field shenfenField = cobject.CreateDropdownField(new FieldCreateBaseInfo("shenfen", "省份", "", false, false), "广东省", new List<string> { "广东省" });
            Field diquField = cobject.CreateDropdownField(new FieldCreateBaseInfo("diqu", "地区", "", false, false), null, new List<string> { "天河区", "番禺区" });
            Field yewuyuanField = cobject.CreateUserListField(new FieldCreateBaseInfo("yewuyuan", "业务员", "", true, true), true);
            Field lianxidianhuaField = cobject.CreateStringField(new FieldCreateBaseInfo("lianxidianhua", "联系电话", "", false, true), "");
            Field farenDianhuaField = cobject.CreateStringField(new FieldCreateBaseInfo("farenDianhua", "法人电话", "", false, true), "");
            Field yewuJingliDianhuaField = cobject.CreateStringField(new FieldCreateBaseInfo("yewuJingliDianhua", "业务经理电话", "", false, true), "");
            Field caigouJingliDianhuaField = cobject.CreateStringField(new FieldCreateBaseInfo("caigouJingliDianhua", "采购经理电话", "", false, true), "");
            Field lianxirenField = cobject.CreateStringField(new FieldCreateBaseInfo("lianxiren", "联系人", "传真/邮箱", false, true), "");
            Field gongsiXinzhiField = cobject.CreateStringField(new FieldCreateBaseInfo("gongsiXinzhi", "公司性质", "国有、民营、私企、药厂直属公司", false, true), "");
            Field yunyiFangsiField = cobject.CreateStringField(new FieldCreateBaseInfo("yunyiFangsi", "运营方式", "临床.OTC；自己开发、挂靠、纯配送、招商、批发", false, true), "");
            Field yixiangChanpinField = cobject.CreateStringField(new FieldCreateBaseInfo("yixiangChanpin", "意向产品", "品名、月销量、区域", false, true), "");
            Field huifangRiqiField = cobject.CreateDateField(new FieldCreateBaseInfo("huifangRiqi", "回访日期", "", false, true), false);
            Field lianxiQingkuangField = cobject.CreateTextField(new FieldCreateBaseInfo("lianxiQingkuang", "联系情况", "", false, true), "");
            Field xiaciHuifangRiqiField = cobject.CreateDateField(new FieldCreateBaseInfo("xiaciHuifangRiqi", "下次回访时间", "", false, true), false);
            Field lianxiDizhiField = cobject.CreateStringField(new FieldCreateBaseInfo("lianxiDizhiRiqi", "联系地址", "", false, true), "");
            Field remarkField = cobject.CreateTextField(new FieldCreateBaseInfo("beizhu", "备注", "", false, true), "");

            List<Input> baseSectuibInputs = new List<Input>();
            baseSectuibInputs.Add(new Input(cobject.NameField));
            baseSectuibInputs.Add(new Input(shenfenField));
            baseSectuibInputs.Add(new Input(diquField));
            baseSectuibInputs.Add(new Input(yewuyuanField));
            baseSectuibInputs.Add(new Input(lianxidianhuaField));
            baseSectuibInputs.Add(new Input(farenDianhuaField));
            baseSectuibInputs.Add(new Input(yewuJingliDianhuaField));
            baseSectuibInputs.Add(new Input(caigouJingliDianhuaField));
            baseSectuibInputs.Add(new Input(lianxirenField));
            baseSectuibInputs.Add(new Input(gongsiXinzhiField));
            baseSectuibInputs.Add(new Input(yunyiFangsiField));
            baseSectuibInputs.Add(new Input(yixiangChanpinField));
            baseSectuibInputs.Add(new Input(lianxiDizhiField));
            List<Section> sections = new List<Section>();
            sections.Add(new Section("基本信息", 2, baseSectuibInputs));

            List<Input> huifangInputs = new List<Input>();
            huifangInputs.Add(new Input(huifangRiqiField));
            huifangInputs.Add(new Input(lianxiQingkuangField));
            huifangInputs.Add(new Input(xiaciHuifangRiqiField));
            sections.Add(new Section("回访信息", 2, huifangInputs));

            List<Input> reamarkSectuibInputs = new List<Input>();
            reamarkSectuibInputs.Add(new Input(remarkField));
            sections.Add(new Section("备注信息", 1, reamarkSectuibInputs));

            Form createForm = cobject.FormManager.Create(FormConstCode.CreateFormCode, "创建医药公司客户", sections, null);
            Form editForm = cobject.FormManager.Create(FormConstCode.EditFormCode, "编辑医药公司客户", sections, null);

            baseSectuibInputs.Add(new Input(cobject.CreatedUserField));
            baseSectuibInputs.Add(new Input(cobject.CreatedTimeField));
            baseSectuibInputs.Add(new Input(cobject.ModifiedUserField));
            baseSectuibInputs.Add(new Input(cobject.ModifiedTimeField));

            Form detailsForm = cobject.FormManager.Create(FormConstCode.DetailsFormCode, "医药公司客户信息", sections, null);

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = yewuyuanField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = shenfenField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = diquField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = cobject.NameField.Code, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = lianxidianhuaField.Code, Width = 90 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = farenDianhuaField.Code, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = yewuJingliDianhuaField.Code, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = caigouJingliDianhuaField.Code, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = lianxirenField.Code, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = gongsiXinzhiField.Code, Width = 60 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = yunyiFangsiField.Code, Width = 100 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = yixiangChanpinField.Code, Width = 100 });

            GridView manageView = cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Standard, "", "公司客户管理", true, true, "", viewColumns, cobject.CreatedTimeField.Code, "admin"));
            GridView favoriteView = cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Favorite, "", "收藏公司客户", true, true, "", viewColumns, cobject.CreatedTimeField.Code, "admin"));

            List<GridViewColumnSetupInfo> huifangColumns = new List<GridViewColumnSetupInfo>();
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = yewuyuanField.Code, Width = 50 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = shenfenField.Code, Width = 50 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = diquField.Code, Width = 50 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = cobject.NameField.Code, Width = 80 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = huifangRiqiField.Code, Width = 80 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = xiaciHuifangRiqiField.Code, Width = 80 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = lianxiQingkuangField.Code, Width = 120 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = yunyiFangsiField.Code, Width = 100 });
            huifangColumns.Add(new GridViewColumnSetupInfo { FieldCode = yixiangChanpinField.Code, Width = 100 });

            cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Standard, "jihuaHuifang", "计划回访客户", true, false, null, huifangColumns, xiaciHuifangRiqiField.Code + " desc", "admin"));
            cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Standard, "huifangde", "客户回访", true, false, null, huifangColumns, huifangRiqiField.Code + " desc", "admin"));

            cobject.ObjectPermission.Create(this._coldewManager.OrgManager.Everyone, ObjectPermissionValue.Create | ObjectPermissionValue.View);
            cobject.ObjectPermission.Create(this.kehuAdminGroup, ObjectPermissionValue.All);
            cobject.MetadataPermission.StrategyManager.Create(new MetadataOrgMember(this.kehuAdminGroup), MetadataPermissionValue.All, null);
            cobject.MetadataPermission.StrategyManager.Create(new MetadataFieldMember(yewuyuanField), MetadataPermissionValue.View, null);
            cobject.MetadataPermission.StrategyManager.Create(new MetadataFieldMember(cobject.CreatedUserField), MetadataPermissionValue.All, null);
        }

        private void InitFahuo()
        {
            this._coldewManager.Logger.Info("init fahuo");
            ColdewObject cobject = this._coldewManager.ObjectManager.Create(new ColdewObjectCreateInfo("订单总表", "dingdanZhongbiao", ColdewObjectType.Standard, true, "产品名称"));
            Field yewuyuanField = cobject.CreateStringField(new FieldCreateBaseInfo("yewuyuan", "业务员", "", true, true), "");
            Field shengfenField = cobject.CreateStringField(new FieldCreateBaseInfo("shengfen", "省份", "", false, true), "");
            Field diquField = cobject.CreateStringField(new FieldCreateBaseInfo("diqu", "地区", "", false, true), "");
            Field fahuoRiqiField = cobject.CreateDateField(new FieldCreateBaseInfo("fahuoRiqi", "发货日期", "", false, true), true);
            Field guigeField = cobject.CreateStringField(new FieldCreateBaseInfo("guige", "规格", "", false, true), "");
            Field shengchanQiyeField = cobject.CreateStringField(new FieldCreateBaseInfo("shengchanQiye", "生产企业", "", false, true), "");
            Field zongshuliangField = cobject.CreateNumberField(new FieldCreateBaseInfo("zongshuliang", "总数量", "", false, true), null, null, null, 2);
            Field chengbenjiaField = cobject.CreateNumberField(new FieldCreateBaseInfo("chengbenjia", "成本价", "", false, true), null, null, null, 2);
            Field xiaoshoujiaField = cobject.CreateNumberField(new FieldCreateBaseInfo("xiaoshoujia", "销售价", "", false, true), null, null, null, 2);
            Field chukujiaField = cobject.CreateNumberField(new FieldCreateBaseInfo("chukuDanjia", "出库单价", "", false, true), null, null, null, 2);
            Field zongjineField = cobject.CreateNumberField(new FieldCreateBaseInfo("zongjine", "总金额", "", false, true), null, null, null, 2);
            Field huikuanRiqiField = cobject.CreateDateField(new FieldCreateBaseInfo("huikuanRiqi", "汇款日期", "", false, true), false);
            Field huikuanJineField = cobject.CreateNumberField(new FieldCreateBaseInfo("huikuanJine", "汇款金额", "", false, true), null, null, null, 2);
            Field huikuanDanweiField = cobject.CreateStringField(new FieldCreateBaseInfo("huikuanDanwei", "汇款单位", "", false, true), "");
            Field daokuanDanweiField = cobject.CreateStringField(new FieldCreateBaseInfo("daokuanDanwei", "到款单位", "", false, true), "");
            Field kaipiaoDanweiField = cobject.CreateStringField(new FieldCreateBaseInfo("kaipiaoDanwei", "开票单位", "", false, true), "");
            Field beizhuField = cobject.CreateTextField(new FieldCreateBaseInfo("beizhu", "备注", "", false, true), "");

            List<Input> baseSectuibInputs = new List<Input>();
            foreach (Field field in cobject.GetFields())
            {
                if (field == cobject.CreatedUserField ||
                    field == cobject.CreatedTimeField ||
                    field == cobject.ModifiedUserField ||
                    field == cobject.ModifiedTimeField)
                {
                    continue;
                }
                baseSectuibInputs.Add(new Input(field));
            }
            List<Section> sections = new List<Section>();
            sections.Add(new Section("基本信息", 2, baseSectuibInputs));

            Form editForm = cobject.FormManager.Create(FormConstCode.EditFormCode, "编辑发货", sections, null);
            Form detailsForm = cobject.FormManager.Create(FormConstCode.DetailsFormCode, "发货信息", sections, null);

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = yewuyuanField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = shengfenField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = diquField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = fahuoRiqiField.Code, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = cobject.NameField.Code, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = guigeField.Code, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = shengchanQiyeField.Code, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = zongshuliangField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = chengbenjiaField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = xiaoshoujiaField.Code, Width = 50 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = chukujiaField.Code, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = zongjineField.Code, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = huikuanRiqiField.Code, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = huikuanJineField.Code, Width = 80 });
            viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = huikuanJineField.Code, Width = 80 });

            GridView manageView = cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Standard, "", "发货管理", true, true, "", viewColumns, cobject.CreatedTimeField.Code, "admin"));
            GridView favoriteView = cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Favorite, "", "收藏发货", true, true, "", viewColumns, cobject.CreatedTimeField.Code, "admin"));

            cobject.ObjectPermission.Create(this.kehuAdminGroup, ObjectPermissionValue.View | ObjectPermissionValue.Export | ObjectPermissionValue.PermissionSetting);
            cobject.MetadataPermission.StrategyManager.Create(new MetadataFieldMember(yewuyuanField), MetadataPermissionValue.View, null);
            cobject.MetadataPermission.StrategyManager.Create(new MetadataOrgMember(this.kehuAdminGroup), MetadataPermissionValue.View | MetadataPermissionValue.Modify, null);
        }

        private void InitFahuoLiucheng()
        {
            this._coldewManager.Logger.Info("init fahuo liucheng");
            ColdewObject cobject = this._coldewManager.ObjectManager.Create(new ColdewObjectCreateInfo("发货流程", "FahuoLiucheng", ColdewObjectType.Workflow, true, "公司"));
            Field shengfenField = cobject.CreateStringField(new FieldCreateBaseInfo("shengfen", "省份", "", false, true), "");
            Field diquField = cobject.CreateStringField(new FieldCreateBaseInfo("diqu", "地区", "", false, true), "");
            Field fahuoRiqiField = cobject.CreateDateField(new FieldCreateBaseInfo("fahuoRiqi", "发货日期", "", false, true), true);
            Field huikuanRiqiField = cobject.CreateDateField(new FieldCreateBaseInfo("huikuanRiqi", "汇款日期", "", false, true), false);
            Field huikuanJineField = cobject.CreateNumberField(new FieldCreateBaseInfo("huikuanJine", "汇款金额", "", false, true), null, null, null, 2);
            Field huikuanLeixingField = cobject.CreateStringField(new FieldCreateBaseInfo("huikuanLeixing", "汇款类型", "", false, true), "");
            Field huikuanDanweiField = cobject.CreateStringField(new FieldCreateBaseInfo("huikuanDanwei", "汇款单位", "", false, true), "");
            Field daokuanDanweiField = cobject.CreateStringField(new FieldCreateBaseInfo("daokuanDanwei", "到款单位", "", false, true), "");
            Field kaipiaoDanweiField = cobject.CreateStringField(new FieldCreateBaseInfo("kaipiaoDanwei", "开票单位", "", false, true), "");
            Field shouhuoDizhiField = cobject.CreateStringField(new FieldCreateBaseInfo("shouhuoDizhi", "收货地址", "", false, true), "");
            Field shouhuorenField = cobject.CreateStringField(new FieldCreateBaseInfo("shouhuoren", "收货人及电话", "", false, true), "");
            Field suihuoFudaiField = cobject.CreateTextField(new FieldCreateBaseInfo("suihuoFudai", "随货附带", "", false, true), "");
            Field chanpinListField = cobject.CreateJsonField(new FieldCreateBaseInfo("chanpinList", "产品信息", "", false, true));
            Field beizhuField = cobject.CreateTextField(new FieldCreateBaseInfo("beizhu", "备注", "", false, true), "");
            Field guigeField = cobject.CreateStringField(new FieldCreateBaseInfo("guige", "规格", "", false, true), "");
            Field liuchengIdField = cobject.CreateStringField(new FieldCreateBaseInfo("liuchengId", "流程ID", "", false, true), "");

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            foreach (Field field in cobject.GetFields().Take(8))
            {
                viewColumns.Add(new GridViewColumnSetupInfo { FieldCode = field.Code, Width = 80 });
            }

            GridView manageView = cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Standard, "", "发货流程管理", true, true, "", viewColumns, cobject.CreatedTimeField.Code + " desc", "admin"));
            GridView favoriteView = cobject.GridViewManager.Create(new GridViewCreateInfo(GridViewType.Favorite, "", "收藏发货流程", true, true, "", viewColumns, cobject.CreatedTimeField.Code + " desc", "admin"));

            this._coldewManager.LiuchengYinqing.LiuchengMobanManager.Create("FahuoLiucheng", "发货流程", cobject, "~/FahuoLiucheng", "");

            cobject.ObjectPermission.Create(this.kehuAdminGroup, ObjectPermissionValue.View);
            cobject.MetadataPermission.StrategyManager.Create(new MetadataOrgMember(this.kehuAdminGroup), MetadataPermissionValue.View, null);
        }

    }
}
