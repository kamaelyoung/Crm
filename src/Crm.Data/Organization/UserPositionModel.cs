using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data.Organization
{
    public class UserPositionModel
    {
        

        public UserPositionModel()
        {

        }

        public virtual int ID { get; set; }

        public virtual string UserId { set; get; }

        public virtual string PositionId { set; get; }

        public virtual bool Main { set; get; }
    }
}
