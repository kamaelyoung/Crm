using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Core.UI;
using Coldew.Data;
using Crm.Api;

namespace Crm.Core
{
    public class CustomerObject : ColdewObject
    {
        CrmManager _crmManager;
        public CustomerObject(string id, string code, string name, CrmManager crmManager)
            :base(id, code, name, crmManager)
        {
            this._crmManager = crmManager;
        }

        protected override FormManager CreateFormManager(ColdewManager coldewManager)
        {
            return base.CreateFormManager(coldewManager);
        }

        protected override MetadataManager CreateMetadataManager(ColdewManager coldewManager)
        {
            return new CustomerManager(this, coldewManager.OrgManager);
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
