using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data
{
    public class CustomerFavoriteModel
    {
        public virtual int ID { set; get; }

        public virtual string UserID { set; get; }

        public virtual string CustomerID { set; get; }
    }
}
