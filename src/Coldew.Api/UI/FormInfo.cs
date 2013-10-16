using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.UI
{
    [Serializable]
    public class FormInfo
    {
        public string ID { set; get; }

        public string Code { set; get; }

        public string Title { set; get; }

        public List<SectionInfo> Sections { set; get; }

        public List<RelatedObjectInfo> Relateds { set; get; }
    }
}
