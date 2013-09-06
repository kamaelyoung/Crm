using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;

namespace Crm.Core
{
    public class CrmInitModel
    {
        public List<string> areas;

        public List<FormInitModel> forms;

        public List<GridViewInitModel> gridviews;
    }

    public class FormInitModel
    {
        public FormType type;
        public string name;
        public List<FormColumnInitModel> columns;
    }

    public class FormColumnInitModel
    {
        public string code;
        public string name;
        public bool required;
        public bool canImport;
        public bool isSystem;
        public int index;
        public FieldType type;
        public string config;
    }

    public class GridViewInitModel
    {
        public GridViewType type;
        public FormType formType;
        public List<GridViewColumnInitModel> columns;
    }

    public class GridViewColumnInitModel
    {
        public string fieldCode;
        public int width;
    }
}
