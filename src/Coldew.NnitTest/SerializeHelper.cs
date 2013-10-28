using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Crm.UnitTest
{
    public class SerializeHelper
    {
        public const char UNICODE_BOOM = (char)((254 << 8) | 255);

        public static string XmlSerialize(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            XmlSerializer se = new XmlSerializer(obj.GetType());

            StringWriter sw = new StringWriter();
            se.Serialize(sw, obj);

            return sw.ToString();
        }

        public static void XmlSerializeToFile(object obj, string fileName)
        {
            string strPath = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            StreamWriter sw = File.CreateText(fileName);
            sw.Write(XmlSerialize(obj));
            sw.Close();
        }

        public static object XmlDeserialize(string xml, Type objType)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }
            if (xml.IndexOf("<?xml") == -1)
            {
                xml = "<?xml version=\"1.0\"?>" + xml;
            }
            else
            {
                xml = "<?xml version=\"1.0\"?>" + xml.Substring(xml.IndexOf('>', xml.IndexOf("<?xml") + 5) + 1);
            }

            XmlSerializer se = new XmlSerializer(objType);
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(System.Text.ASCIIEncoding.Unicode.GetBytes(xml));
            bw.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            Object obj = se.Deserialize(ms);

            ms.Close();
            return obj;
        }

        public static T XmlDeserialize<T>(string xml)
        {
            return (T)XmlDeserialize(xml, typeof(T));
        }

        public static T XmlDeserializeFromFile<T>(string fileName)
        {
            string strXml = File.ReadAllText(fileName);

            return XmlDeserialize<T>(strXml);
        }
    }
}
