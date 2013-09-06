using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Aop;

namespace Crm.Core
{
    public class ThrowingLogger : IThrowsAdvice
    {
        CrmManager _crmManager;

        public ThrowingLogger(CrmManager crmManager)
        {
            _crmManager = crmManager;
        }

        public void AfterThrowing(Exception ex)
        {
            this._crmManager.Logger.Error(ex.Message, ex);
        }
    }
}
