using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class GridViewColumnSetupInfo
    {
        public GridViewColumnSetupInfo(string fieldCode, int width)
        {
            this.FieldCode = fieldCode;
            this.Width = width;
        }

        public GridViewColumnSetupInfo() { }

        public string FieldCode { set; get; }

        public int Width { set; get; }
    }
}
