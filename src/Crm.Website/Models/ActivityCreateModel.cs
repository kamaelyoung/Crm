using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Crm.Website.Models
{
    public class ActivityCreateModel
    {
        public string subject;
        public string contactId;
        public JObject extends;
    }
}