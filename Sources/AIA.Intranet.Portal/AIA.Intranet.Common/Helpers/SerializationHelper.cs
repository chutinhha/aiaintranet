using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AIA.Intranet.Common.Extensions;

namespace AIA.Intranet.Common.Helpers
{
    public class SerializationHelper
    {
        public static string SerializeToXml<T>(T obj)
        {
            return SerializeToXml<T>(obj, string.Empty);
        }

        public static string SerializeToXml<T>(T obj, string nameSpace)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), nameSpace);
            StringBuilder builder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            using (XmlWriter stringWriter = XmlWriter.Create(builder, settings))
            {
                xmlSerializer.Serialize(stringWriter, obj);
                return builder.ToString();
            }
        }

        public static T DeserializeFromXml<T>(string xml)
        {
            return DeserializeFromXml<T>(xml, string.Empty);          
        }

        public static T DeserializeFromXml<T>(string xml, string nameSpace)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), nameSpace);
            using (XmlTextReader reader = new XmlTextReader(new StringReader(xml)))
            {
                reader.WhitespaceHandling = WhitespaceHandling.Significant;
                var obj =  (T)xmlSerializer.Deserialize(reader);
                obj.PopulaResources();
                return obj;
            }
        }
    }
}
