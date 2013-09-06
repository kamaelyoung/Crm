using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Crm.Website.Models
{
    public class ContactCreateModel
    {
        public string name;
        public string customerId;
        public JObject extends;
    }
}