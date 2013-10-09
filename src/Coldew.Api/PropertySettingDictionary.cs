using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api
{
    [Serializable]
    public class PropertySettingDictionary : Dictionary<string, string>
    {
        public PropertySettingDictionary()
        {

        }

        public PropertySettingDictionary(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
        //public PropertySettingDictionary()
        //{
        //    _propertys = new Dictionary<string, string>();
        //}

        //Dictionary<string, string> _propertys;

        //public void Add(string code, string value)
        //{
        //    this._propertys.Add(code, value);
        //}

        //public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        //{
        //    return this._propertys.GetEnumerator();
        //}

        //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    return this._propertys.GetEnumerator();
        //}


        //public bool ContainsKey(string key)
        //{
        //    return this._propertys.ContainsKey(key);
        //}

        //public bool Remove(string key)
        //{
        //    return this._propertys.Remove(key);
        //}

        //public string this[string key]
        //{
        //    get
        //    {
        //        return this._propertys[key];
        //    }
        //    set
        //    {
        //        this._propertys[key] = value;
        //    }
        //}

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    this._propertys.GetObjectData(info, context);
        //}
    }
}
