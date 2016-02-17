using System.IO;
using System.Xml;

namespace CourseValidator
{
    public class Topic : TocEntry
    {
        private readonly string _absolutePath; // Full absolute path to Topic file

        public Topic()
        {
            _absolutePath = "";
        }

        public Topic(XmlNode xmlNode, Course c)
        {
            _course = c; // Store a reference to the Couse this Topic is attached to

            Attributes = xmlNode.Attributes;

            _absolutePath = Path.GetFullPath(Path.Combine(_course.ProjectRoot, this["Link"].Substring(1)));

            XmlDoc = new XmlDocument();
            if (File.Exists(_absolutePath))
            {
                XmlDoc.Load(_absolutePath);
            }
        }

        public bool XmlDocumentHasThisNode(string xPathRequest)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(this.XmlDoc.NameTable);
            nsmgr.AddNamespace("MadCap", "http://www.madcapsoftware.com/Schemas/MadCap.xsd");
            // Return true if a node matching the XPath request was found
            return this.XmlDoc.SelectSingleNode(xPathRequest, nsmgr) != null;
        }
    }
}
