using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coldew.Website.Models
{
    [Flags]
    public enum ChengyuanXuanzeTab
    {
        Yonghu = 1,
        Bumen = 2,
        Zhiwei = 4,
        Yonghuzu = 8
    }
}