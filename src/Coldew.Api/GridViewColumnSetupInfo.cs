using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class GridViewColumnSetupInfo
    {
        public GridViewColumnSetupInfo(int fieldId, int width)
        {
            this.FieldId = fieldId;
            this.Width = width;
        }

        public GridViewColumnSetupInfo() { }

        public int FieldId { set; get; }

        public int Width { set; get; }
    }
}
