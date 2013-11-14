using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.UI;
using Coldew.Data;
using Newtonsoft.Json;
using System.Threading;

namespace Coldew.Core.UI
{
    public class RelatedObject
    {
        ColdewObjectManager _objectManager;
        string _objectCode;
        List<string> _fieldCodes;
        public RelatedObject(string objectCode, List<string> fieldCodes, ColdewObjectManager objectManager)
        {
            this._objectManager = objectManager;
            this._objectCode = objectCode;
            this._fieldCodes = fieldCodes;
        }

        ColdewObject _object;
        public ColdewObject Object
        {
            get
            {
                if (this._object == null)
                {
                    this._object = this._objectManager.GetObjectByCode(this._objectCode);
                }
                return _object;
            }
        }

        List<Field> _fields;
        public List<Field> Fields
        {
            get
            {
                if (this._fields == null)
                {
                    this._fields = this._fieldCodes.Select(x => this.Object.GetFieldByCode(x)).ToList();
                }
                return this._fields;
            }
        }

        public void ClearFieldData(Field field)
        {
            List<Field> fields = this.Fields.ToList();
            fields.Remove(field);
            this._fields = fields;
            this._fieldCodes = this._fields.Select(x => x.Code).ToList();
        }

        public RelatedObjectInfo Map()
        {
            return new RelatedObjectInfo
            {
                Object = this.Object.Map(),
                ShowFields = this.Fields.Select(x => x.Map()).ToList()
            };
        }

        internal RelatedObjectModel MapModel()
        {
            RelatedObjectModel model = new RelatedObjectModel();
            model.FieldCodes = this._fieldCodes;
            model.ObjectCode = this._objectCode;
            return model;
        }
    }
}
