using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.UI;

namespace Coldew.Core.UI
{
    public class FormService : IFormService
    {
        ColdewManager _coldewManager;

        public FormService(ColdewManager crmManager)
        {
            this._coldewManager = crmManager;
        }

        public FormInfo GetForm(string objectId, string code)
        {
            ColdewObject cObject = this._coldewManager.ObjectManager.GetObjectById(objectId);
            Form form = cObject.FormManager.GetFormByCode(code);
            if (form != null)
            {
                return form.Map();
            }
            return null;
        }

        public FormInfo GetFormByCode(string objectCode, string code)
        {
            ColdewObject cObject = this._coldewManager.ObjectManager.GetObjectByCode(objectCode);
            Form form = cObject.FormManager.GetFormByCode(code);
            if (form != null)
            {
                return form.Map();
            }
            return null;
        }
    }
}
