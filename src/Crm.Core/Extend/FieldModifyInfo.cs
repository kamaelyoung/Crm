﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Core.Extend
{
    public class FieldModifyInfo
    {
        public string Name{set;get;} 
        public bool Required{set;get;} 
        public int Index{set;get;}
        public FieldConfig Config { set; get; } 
    }
}
