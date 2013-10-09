using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Coldew.Data;
using Coldew.Core;
using Coldew.Api;

namespace Crm.Core
{
    public class CrmForm: Form
    {
        CrmManager _crmManager;
        public CrmForm(string id, string code, string name, CrmManager crmManager)
            :base(id, code, name, crmManager)
        {
            _crmManager = crmManager;
        }

        protected override MetadataManager CreateMetadataManager(ColdewManager coldewManager)
        {
            CrmManager crmManager = coldewManager as CrmManager;
            if (this.Code == CrmFormConstCode.FORM_CUSTOMER)
            {
                return new CustomerManager(this, crmManager.OrgManager);
            }
            else if (this.Code == CrmFormConstCode.FORM_CONTACT)
            {
                return new ContactManager(this, crmManager.OrgManager);
            }
            else if (this.Code == CrmFormConstCode.FORM_Activity)
            {
                return new ActivityManager(this, crmManager.OrgManager);
            }
            return base.CreateMetadataManager(coldewManager);
        }

        public Field CreateCustomerAreaField(string code, string name, bool required, bool canModify, bool canInput, int index)
        {
            return this.CreateField(code, name, required, canModify, canInput, index, CustomerFieldType.CustomerArea, "");
        }

        public override Field CreateField(FieldModel model)
        {
            FieldNewInfo newInfo = new FieldNewInfo(model.ID, model.Code, model.Name, model.Required, model.CanModify, model.Type, model.CanInput, model.Index, this);
            switch (newInfo.Type)
            {
                case CustomerFieldType.CustomerArea:
                    CustomerArea area = null;
                    if (!string.IsNullOrEmpty(model.Config))
                    {
                        area = this._crmManager.AreaManager.GetAreaById(int.Parse(model.Config));
                    }
                    return new CustomerAreaField(newInfo, area, this._crmManager.AreaManager);
            }
            return base.CreateField(model);
        }
    }
}
