using System.IO;
using System.Xml.XPath;

namespace Pandv.AriesDoc.Generator
{
    public class XmlComments : IComments
    {
        private XPathDocument doc;

        public XmlComments(string xmlCommentsFile)
        {
            if (File.Exists(xmlCommentsFile))
                doc = new XPathDocument(xmlCommentsFile);
        }
    }
}