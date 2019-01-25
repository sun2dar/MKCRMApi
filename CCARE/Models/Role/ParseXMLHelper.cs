using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Serialization;

namespace CCARE.Models.Role
{
    class ParseXMLHelper
    {
        public static string SerializeXml(Root root)
        {
            XmlSerializer s = new XmlSerializer(root.GetType());
            var sw = new System.IO.StringWriter();

            s.Serialize(sw, root);
            return sw.ToString();
        }

        //Read XML
        public static Root DeserializeXml(string xmlFileorString, string type)
        {
            XmlSerializer s = new XmlSerializer(typeof(Root));
            //Root root = new Root();
            TextReader textReader;

            if (type == "file")
            {
                textReader = new StreamReader(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + xmlFileorString);
            }
            else
            {
                textReader = new StringReader(xmlFileorString);
            }

            Root root = (Root)s.Deserialize(textReader);

            return root;
        }
    }
}