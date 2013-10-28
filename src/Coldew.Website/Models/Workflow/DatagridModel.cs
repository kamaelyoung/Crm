using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Coldew.Api.Workflow;

namespace Coldew.Website.Models
{
    public class DatagridModel<T>
    {
        public IEnumerable<T> list;
        public int count;
    }
}