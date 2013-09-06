﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Crm.Website.Models
{
    public class CustomerEditPostModel
    {
        public string id;
        public string name;
        public int area;
        public List<string> salesAccounts;
        public JObject extends;
    }
}