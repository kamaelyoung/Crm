using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Crm.Website.Models
{
    public class ContractEditPostModel
    {
        public string id;
        public string name;
        public DateTime startDate;
        public DateTime endDate;
        public int expiredComputeDays;
        public List<string> owners;
        public float value;
        public JObject extends;
    }
}