﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data
{
    public class ContractModel
    {
        public virtual string ID { set; get; }

        public virtual string PropertysJson { get; set; }
    }
}
