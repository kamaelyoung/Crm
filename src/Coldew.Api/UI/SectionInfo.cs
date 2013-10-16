using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.UI
{
    [Serializable]
    public class SectionInfo
    {
        public string Title { set; get; }

        public int ColumnCount { set; get; }

        public List<InputInfo> Inputs { set; get; }
    }
}
