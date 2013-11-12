﻿using System;
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
        Group adminGroup;
        ColdewManager _coldewManager;
        public LittleOrangeInitializer(ColdewManager crmManager)
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
                    this._coldewManager.OrgManager.UserManager.Create(this._coldewManager.OrgManager.System, new UserCreateInfo
                    {
                        Name = "user1",
                        Account = "user1",
                        Password = "123456",
                        Status = UserStatus.Normal,
                        MainPositionId = this._coldewManager.OrgManager.PositionManager.TopPosition.ID
                    });

                    this._coldewManager.OrgManager.UserManager.Create(this._coldewManager.OrgManager.System, new UserCreateInfo
                    {
                        Name = "user2",
                        Account = "user2",
                        Password = "123456",
                        Status = UserStatus.Normal,
                        MainPositionId = this._coldewManager.OrgManager.PositionManager.TopPosition.ID
                    });

                    this.adminGroup = this._coldewManager.OrgManager.GroupManager.Create(this._admin, new GroupCreateInfo { GroupType = GroupType.Group, Name = "管理员" });
                    

                    this.InitConfig();
                    this.InitZiranren();
                    this.InitGongsiKehu();
                    this.InitFahuo();
                    this.InitFahuoLiucheng();
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

        private void InitZiranren()
        {
            this._coldewManager.Logger.Info("init ziranren");
            ColdewObject cobject = this._coldewManager.ObjectManager.Create("自然人客户", LittleOrangeObjectConstCode.Object_Ziranren);
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "客户名称", "", true, true, true, 1, "");
            Field shenfenField = cobject.CreateDropdownField("shenfen", "省份", "", false, false, true, 2, "广东省", new List<string> { "广东省" });
            Field diquField = cobject.CreateDropdownField("diqu", "地区", "", false, false, true, 3, null, new List<string> { "天河区", "番禺区" });
            Field salesUsersField = cobject.CreateUserListField(LittleOrangeObjectConstCode.CUST_FIELD_NAME_SALES_USERS, "业务员", "", true, true, true, 4, true);
            Field lianxidianhuaField = cobject.CreateStringField("lianxidianhua", "联系电话", "", false, true, true, 5, "");
            Field guakaoGongsiField = cobject.CreateStringField("guakaoGongsi", "挂靠公司名称", "配送点、回款周期", false, true, true, 5, "");
            Field lianxirenField = cobject.CreateStringField("lianxiren", "联系人", "传真/邮箱/QQ", false, true, true, 6, "");
            Field yunyiFangsiField = cobject.CreateStringField("yunyiFangsi", "运营方式", "有开发团队、招商、个人开发医院", false, true, true, 7, "");
            Field zhuyinQuyuField = cobject.CreateStringField("zhuyinQuyu", "主营区域及医院", "", false, true, true, 8, "");
            Field zhuyinChanpinField = cobject.CreateStringField("zhuyinChanpin", "主营产品及月销量", "", false, true, true, 9, "");
            Field yixiangChanpinField = cobject.CreateStringField("yixiangChanpin", "意向产品", "品名、月销量、区域", false, true, true, 10, "");
            Field huifangRiqiField = cobject.CreateDateField("huifangRiqi", "回访日期", "", false, true, true, 11, false);
            Field lianxiQingkuangField = cobject.CreateTextField("lianxiQingkuang", "联系情况", "", false, true, true, 12, "");
            Field xiaciHuifangRiqiField = cobject.CreateDateField("xiaciHuifangRiqi", "下次回访时间", "", false, true, true, 13, false);
            Field lianxiDizhiField = cobject.CreateStringField("lianxiDizhiRiqi", "联系地址", "", false, true, true, 14, "");
            Field remarkField = cobject.CreateTextField(null, "备注", "", false, true, true, 15, "");
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", "", true, false, false, 16, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", "", false, false, false, 17, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", "", true, false, false, 18, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", "", false, false, false, 19, true);

            List<Input> baseSectuibInputs = new List<Input>();
            baseSectuibInputs.Add(new Input(nameField, 1));
            baseSectuibInputs.Add(new Input(shenfenField, 2));
            baseSectuibInputs.Add(new Input(salesUsersField, 3));
            baseSectuibInputs.Add(new Input(diquField, 4));
            baseSectuibInputs.Add(new Input(lianxidianhuaField, 4));
            baseSectuibInputs.Add(new Input(guakaoGongsiField, 5));
            baseSectuibInputs.Add(new Input(lianxirenField, 6));
            baseSectuibInputs.Add(new Input(yunyiFangsiField, 7));
            baseSectuibInputs.Add(new Input(zhuyinQuyuField, 8));
            baseSectuibInputs.Add(new Input(zhuyinChanpinField, 9));
            baseSectuibInputs.Add(new Input(yixiangChanpinField, 10));
            baseSectuibInputs.Add(new Input(lianxiDizhiField, 11));
            List<Section> sections = new List<Section>();
            sections.Add(new Section("基本信息", 2, baseSectuibInputs));

            List<Input> huifangInputs = new List<Input>();
            huifangInputs.Add(new Input(huifangRiqiField, 1));
            huifangInputs.Add(new Input(lianxiQingkuangField, 2));
            huifangInputs.Add(new Input(xiaciHuifangRiqiField, 3));
            sections.Add(new Section("回访信息", 2, huifangInputs));

            List<Input> reamarkSectuibInputs = new List<Input>();
            reamarkSectuibInputs.Add(new Input(remarkField, 1));
            sections.Add(new Section("备注信息", 1, reamarkSectuibInputs));

            Form createForm = cobject.FormManager.Create(FormConstCode.CreateFormCode, "创建自然人客户", sections, null);
            Form editForm = cobject.FormManager.Create(FormConstCode.EditFormCode, "编辑自然人客户", sections, null);

            baseSectuibInputs.Add(new Input(creatorField, 12));
            baseSectuibInputs.Add(new Input(createTimeField, 13));
            baseSectuibInputs.Add(new Input(modifiedUserField, 14));
            baseSectuibInputs.Add(new Input(modifiedTimeField, 15));

            Form detailsForm = cobject.FormManager.Create(FormConstCode.DetailsFormCode, "自然人客户信息", sections, null);


            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            foreach (Field field in cobject.GetFields().Take(8))
            {
                viewColumns.Add(new GridViewColumnSetupInfo { FieldId = field.ID, Width = 80 });
            }

            GridView manageView = cobject.GridViewManager.Create(GridViewType.Manage, "", "自然人客户管理", this._admin, true, true, 1, "", viewColumns);
            GridView favoriteView = cobject.GridViewManager.Create(GridViewType.Favorite, "", "收藏自然人客户", this._admin, true, true, 2, "", viewColumns);

            List<Member> members = new List<Member>();
            members.Add(this._coldewManager.OrgManager.Everyone);
            this._coldewManager.OrgManager.FunctionManager.Create(cobject.ID, cobject.Name, "", "", 1, members);
        }

        private void InitGongsiKehu()
        {
            this._coldewManager.Logger.Info("init gongsiKehu");
            ColdewObject cobject = this._coldewManager.ObjectManager.Create("医药公司客户", LittleOrangeObjectConstCode.Object_GongsiKehu);
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "公司名称", "", true, true, true, 1, "");
            Field shenfenField = cobject.CreateDropdownField("shenfen", "省份", "", false, false, true, 2, "广东省", new List<string> { "广东省" });
            Field diquField = cobject.CreateDropdownField("diqu", "地区", "", false, false, true, 3, null, new List<string> { "天河区", "番禺区" });
            Field salesUsersField = cobject.CreateUserListField(LittleOrangeObjectConstCode.CUST_FIELD_NAME_SALES_USERS, "业务员", "", true, true, true, 4, true);
            Field lianxidianhuaField = cobject.CreateStringField("lianxidianhua", "联系电话", "", false, true, true, 5, "");
            Field farenDianhuaField = cobject.CreateStringField("farenDianhua", "法人电话", "", false, true, true, 5, "");
            Field yewuJingliDianhuaField = cobject.CreateStringField("yewuJingliDianhua", "业务经理电话", "", false, true, true, 6, "");
            Field caigouJingliDianhuaField = cobject.CreateStringField("caigouJingliDianhua", "采购经理电话", "", false, true, true, 7, "");
            Field lianxirenField = cobject.CreateStringField("lianxiren", "联系人", "传真/邮箱/QQ", false, true, true, 8, "");
            Field gongsiXinzhiField = cobject.CreateStringField("gongsiXinzhi", "公司性质", "国有、民营、私企、药厂直属公司", false, true, true, 9, "");
            Field yunyiFangsiField = cobject.CreateStringField("yunyiFangsi", "运营方式", "临床.OTC；自己开发、挂靠、纯配送、招商、批发", false, true, true, 10, "");
            Field yixiangChanpinField = cobject.CreateStringField("yixiangChanpin", "意向产品", "品名、月销量、区域", false, true, true, 11, "");
            Field huifangRiqiField = cobject.CreateDateField("huifangRiqi", "回访日期", "", false, true, true, 11, false);
            Field lianxiQingkuangField = cobject.CreateTextField("lianxiQingkuang", "联系情况", "", false, true, true, 12, "");
            Field xiaciHuifangRiqiField = cobject.CreateDateField("xiaciHuifangRiqi", "下次回访时间", "", false, true, true, 13, false);
            Field lianxiDizhiField = cobject.CreateStringField("lianxiDizhiRiqi", "联系地址", "", false, true, true, 14, "");
            Field remarkField = cobject.CreateTextField(null, "备注", "", false, true, true, 15, "");
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", "", true, false, false, 16, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", "", false, false, false, 17, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", "", true, false, false, 18, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", "", false, false, false, 19, true);

            List<Input> baseSectuibInputs = new List<Input>();
            baseSectuibInputs.Add(new Input(nameField, 1));
            baseSectuibInputs.Add(new Input(shenfenField, 2));
            baseSectuibInputs.Add(new Input(diquField, 3));
            baseSectuibInputs.Add(new Input(salesUsersField, 4));
            baseSectuibInputs.Add(new Input(lianxidianhuaField, 5));
            baseSectuibInputs.Add(new Input(farenDianhuaField, 5));
            baseSectuibInputs.Add(new Input(yewuJingliDianhuaField, 6));
            baseSectuibInputs.Add(new Input(caigouJingliDianhuaField, 7));
            baseSectuibInputs.Add(new Input(lianxirenField, 8));
            baseSectuibInputs.Add(new Input(gongsiXinzhiField, 9));
            baseSectuibInputs.Add(new Input(yunyiFangsiField, 10));
            baseSectuibInputs.Add(new Input(yixiangChanpinField, 11));
            baseSectuibInputs.Add(new Input(lianxiDizhiField, 12));
            List<Section> sections = new List<Section>();
            sections.Add(new Section("基本信息", 2, baseSectuibInputs));

            List<Input> huifangInputs = new List<Input>();
            huifangInputs.Add(new Input(huifangRiqiField, 1));
            huifangInputs.Add(new Input(lianxiQingkuangField, 2));
            huifangInputs.Add(new Input(xiaciHuifangRiqiField, 3));
            sections.Add(new Section("回访信息", 2, huifangInputs));

            List<Input> reamarkSectuibInputs = new List<Input>();
            reamarkSectuibInputs.Add(new Input(remarkField, 1));
            sections.Add(new Section("备注信息", 1, reamarkSectuibInputs));

            Form createForm = cobject.FormManager.Create(FormConstCode.CreateFormCode, "创建医药公司客户", sections, null);
            Form editForm = cobject.FormManager.Create(FormConstCode.EditFormCode, "编辑医药公司客户", sections, null);

            baseSectuibInputs.Add(new Input(creatorField, 12));
            baseSectuibInputs.Add(new Input(createTimeField, 13));
            baseSectuibInputs.Add(new Input(modifiedUserField, 14));
            baseSectuibInputs.Add(new Input(modifiedTimeField, 15));

            Form detailsForm = cobject.FormManager.Create(FormConstCode.DetailsFormCode, "医药公司客户信息", sections, null);

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            foreach (Field field in cobject.GetFields().Take(8))
            {
                viewColumns.Add(new GridViewColumnSetupInfo { FieldId = field.ID, Width = 80 });
            }

            GridView manageView = cobject.GridViewManager.Create(GridViewType.Manage, "", "公司客户管理", this._admin, true, true, 1, "", viewColumns);
            GridView favoriteView = cobject.GridViewManager.Create(GridViewType.Favorite, "", "收藏公司客户", this._admin, true, true, 2, "", viewColumns);

            List<Member> members = new List<Member>();
            members.Add(this._coldewManager.OrgManager.Everyone);
            this._coldewManager.OrgManager.FunctionManager.Create(cobject.ID, cobject.Name, "", "", 1, members);
        }

        private void InitFahuo()
        {
            this._coldewManager.Logger.Info("init fahuo");
            ColdewObject cobject = this._coldewManager.ObjectManager.Create("订单总表", "dingdanZhongbiao");
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "产品名称", "", true, true, true, 1, "");
            Field yewuyuanField = cobject.CreateStringField("yewuyuan", "业务员", "", true, true, true, 1, "");
            Field shengfenField = cobject.CreateStringField("shengfen", "省份", "", false, true, true, 1, "");
            Field diquField = cobject.CreateStringField("diqu", "地区", "", false, true, true, 1, "");
            Field fahuoRiqiField = cobject.CreateDateField("fahuoRiqi", "发货日期", "", false, true, true, 1, true);
            Field guigeField = cobject.CreateStringField("guige", "规格", "", false, true, true, 2, "");
            Field shengchanQiyeField = cobject.CreateStringField("shengchanQiye", "生产企业", "", false, true, true, 3, "");
            Field zongshuliangField = cobject.CreateStringField("zongshuliang", "总数量", "", false, true, true, 5, "");
            Field chengbenjiaField = cobject.CreateStringField("chengbenjia", "成本价", "", false, true, true, 5, "");
            Field xiaoshoujiaField = cobject.CreateStringField("xiaoshoujia", "销售价", "", false, true, true, 5, "");
            Field chukujiaField = cobject.CreateStringField("chukujia", "出库单价", "", false, true, true, 5, "");
            Field zongjineField = cobject.CreateStringField("zongjine", "总金额", "", false, true, true, 7, "");
            Field huikuanRiqiField = cobject.CreateDateField("huikuanRiqi", "汇款日期", "", false, true, true, 1, false);
            Field huikuanJineField = cobject.CreateNumberField("huikuanJine", "汇款金额", "", false, true, true, 1, null, null, null, 2);
            Field huikuanDanweiField = cobject.CreateStringField("huikuanDanwei", "汇款单位", "", false, true, true, 1, "");
            Field daokuanDanweiField = cobject.CreateStringField("daokuanDanwei", "到款单位", "", false, true, true, 1, "");
            Field kaipiaoDanweiField = cobject.CreateStringField("kaipiaoDanwei", "开票单位", "", false, true, true, 1, "");
            Field beizhuField = cobject.CreateTextField("beizhu", "备注", "", false, true, true, 1, "");
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", "", true, false, false, 16, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", "", false, false, false, 17, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", "", true, false, false, 18, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", "", false, false, false, 19, true);

            List<Input> baseSectuibInputs = new List<Input>();
            baseSectuibInputs.Add(new Input(nameField, 1));
            baseSectuibInputs.Add(new Input(guigeField, 2));
            baseSectuibInputs.Add(new Input(shengchanQiyeField, 3));
            baseSectuibInputs.Add(new Input(zongshuliangField, 5));
            baseSectuibInputs.Add(new Input(zongjineField, 7));
            List<Section> sections = new List<Section>();
            sections.Add(new Section("基本信息", 2, baseSectuibInputs));

            Form createForm = cobject.FormManager.Create(FormConstCode.CreateFormCode, "创建发货", sections, null);
            Form editForm = cobject.FormManager.Create(FormConstCode.EditFormCode, "编辑发货", sections, null);

            baseSectuibInputs.Add(new Input(creatorField, 12));
            baseSectuibInputs.Add(new Input(createTimeField, 13));
            baseSectuibInputs.Add(new Input(modifiedUserField, 14));
            baseSectuibInputs.Add(new Input(modifiedTimeField, 15));

            Form detailsForm = cobject.FormManager.Create(FormConstCode.DetailsFormCode, "发货信息", sections, null);

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            foreach (Field field in cobject.GetFields().Take(8))
            {
                viewColumns.Add(new GridViewColumnSetupInfo { FieldId = field.ID, Width = 80 });
            }

            GridView manageView = cobject.GridViewManager.Create(GridViewType.Manage, "", "发货管理", this._admin, true, true, 1, "", viewColumns);
            GridView favoriteView = cobject.GridViewManager.Create(GridViewType.Favorite, "", "收藏发货", this._admin, true, true, 2, "", viewColumns);
        }

        private void InitFahuoLiucheng()
        {
            this._coldewManager.Logger.Info("init fahuo liucheng");
            ColdewObject cobject = this._coldewManager.ObjectManager.Create("发货流程", "FahuoLiucheng");
            Field nameField = cobject.CreateStringField(ColdewObjectCode.FIELD_NAME_NAME, "公司", "", false, true, true, 1, "");
            Field shengfenField = cobject.CreateStringField("shengfen", "省份", "", false, true, true, 1, "");
            Field diquField = cobject.CreateStringField("diqu", "地区", "", false, true, true, 1, "");
            Field fahuoRiqiField = cobject.CreateDateField("fahuoRiqi", "发货日期", "", false, true, true, 1, true);
            Field huikuanRiqiField = cobject.CreateDateField("huikuanRiqi", "汇款日期", "", false, true, true, 1, false);
            Field huikuanJineField = cobject.CreateNumberField("huikuanJine", "汇款金额", "", false, true, true, 1, null, null, null, 2);
            Field huikuanLeixingField = cobject.CreateStringField("huikuanLeixing", "汇款类型", "", false, true, true, 1, "");
            Field huikuanDanweiField = cobject.CreateStringField("huikuanDanwei", "汇款单位", "", false, true, true, 1, "");
            Field daokuanDanweiField = cobject.CreateStringField("daokuanDanwei", "到款单位", "", false, true, true, 1, "");
            Field kaipiaoDanweiField = cobject.CreateStringField("kaipiaoDanwei", "开票单位", "", false, true, true, 1, "");
            Field shouhuoDizhiField = cobject.CreateStringField("shouhuoDizhi", "收货地址", "", false, true, true, 1, "");
            Field shouhuorenField = cobject.CreateStringField("shouhuoren", "收货人及电话", "", false, true, true, 1, "");
            Field suihuoFudaiField = cobject.CreateTextField("suihuoFudai", "随货附带", "", false, true, true, 1, "");
            Field chanpinListField = cobject.CreateJsonField("chanpinList", "产品信息", "", false, true, true, 1);
            Field beizhuField = cobject.CreateTextField("beizhu", "备注", "", false, true, true, 1, "");
            Field guigeField = cobject.CreateStringField("guige", "规格", "", false, true, true, 2, "");
            Field liuchengIdField = cobject.CreateStringField("liuchengId", "流程ID", "", false, true, true, 2, "");
            Field creatorField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_CREATOR, "创建人", "", true, false, false, 16, true);
            Field createTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_CREATE_TIME, "创建时间", "", false, false, false, 17, true);
            Field modifiedUserField = cobject.CreateUserField(ColdewObjectCode.FIELD_NAME_MODIFIED_USER, "修改人", "", true, false, false, 18, true);
            Field modifiedTimeField = cobject.CreateDateField(ColdewObjectCode.FIELD_NAME_MODIFIED_TIME, "修改时间", "", false, false, false, 19, true);

            List<Input> baseSectuibInputs = new List<Input>();
            foreach (Field field in cobject.GetFields())
            {
                baseSectuibInputs.Add(new Input(field, field.Index));
            }

            List<GridViewColumnSetupInfo> viewColumns = new List<GridViewColumnSetupInfo>();
            foreach (Field field in cobject.GetFields().Take(8))
            {
                viewColumns.Add(new GridViewColumnSetupInfo { FieldId = field.ID, Width = 80 });
            }

            GridView manageView = cobject.GridViewManager.Create(GridViewType.Manage, "", "发货流程管理", this._admin, true, true, 1, "", viewColumns);
            GridView favoriteView = cobject.GridViewManager.Create(GridViewType.Favorite, "", "收藏发货流程", this._admin, true, true, 2, "", viewColumns);

            this._coldewManager.LiuchengYinqing.LiuchengMobanManager.Create("FahuoLiucheng", "发货流程", cobject, "~/FahuoLiucheng", "");
        }

    }
}
