using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Crm.Website.Models
{
    public class ContactEditPostModel
    {
        public string id;
        public string name;
        public JObject extends;
    }
}