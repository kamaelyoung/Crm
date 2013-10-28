using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Aop;

namespace Coldew.Core.Workflow
{
    public class FuwuThrowingLogger : IThrowsAdvice
    {
        Yinqing _yinqing;

        public FuwuThrowingLogger(Yinqing yinqing)
        {
            _yinqing = yinqing;
        }

        public void AfterThrowing(Exception ex)
        {
            this._yinqing.Logger.Error(ex.Message, ex);
        }
    }
}
