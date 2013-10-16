using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.UI;

namespace Coldew.Core.UI
{
    public class Input
    {
        public Input(Field field, int index)
        {
            this.Field = field;
            this.Index = index;
        }

        public Field Field { private set; get; }

        public int Index { private set; get; }

        public InputInfo Map()
        {
            return new InputInfo
            {
                Field = this.Field.Map(),
                Index = this.Index
            };
        }
    }
}
