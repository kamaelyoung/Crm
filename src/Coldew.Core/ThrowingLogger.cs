using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Aop;

namespace Coldew.Core
{
    public class ThrowingLogger : IThrowsAdvice
    {
        ColdewManager _coldewManager;

        public ThrowingLogger(ColdewManager coldewManager)
        {
            _coldewManager = coldewManager;
        }

        public void AfterThrowing(Exception ex)
        {
            this._coldewManager.Logger.Error(ex.Message, ex);
        }
    }
}
