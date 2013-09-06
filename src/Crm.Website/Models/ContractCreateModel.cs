using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Crm.Website.Models
{
    public class ContractCreateModel
    {
        public string name;
        public string customerId;
        public DateTime startDate;
        public DateTime endDate;
        public int expiredComputeDays;
        public float value;
        public List<string> owners;
        public JObject extends;
    }
}