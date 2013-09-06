using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Crm.Website.Models
{
    public class ActivityEditPostModel
    {
        public string id;
        public string subject;
        public JObject extends;
    }
}