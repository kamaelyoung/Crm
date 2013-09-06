using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class GridViewColumnInfo
    {
        public int ID { set; get; }

        public int FieldId { set; get; }

        public string Name {  set; get; }

        public string Code {  set; get; }

        public int Width {  set; get; }
    }
}
