using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;

namespace Coldew.Core
{
    public class MetadataMetadataValue : MetadataValue
    {
        string _metadataId;
        MetadataField _metadataField;
        public MetadataMetadataValue(string metadataId, MetadataField field)
            : base(metadataId, field)
        {
            this._metadataId = metadataId;
            this._metadataField = field;
        }

        Metadata _metadata;
        public virtual Metadata Metadata
        {
            get
            {
                if (this._metadata == null)
                {
                    this._metadata = this._metadataField.ValueForm.MetadataManager.GetById(this._metadataId);
                }
                return this._metadata;
            }
        }

        public override string PersistenceValue
        {
            get 
            {
                return this.Metadata.ID; 
            }
        }

        public override string ShowValue
        {
            get 
            {
                if (this.Metadata != null)
                {
                    return this.Metadata.Name; 
                }
                return "";
            }
        }

        public override dynamic OrderValue
        {
            get { return this.ShowValue; }
        }

        public override dynamic EditValue
        {
            get { return this.Metadata.ID; }
        }
    }
}
