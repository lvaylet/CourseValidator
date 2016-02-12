using System.IO;
using System.Xml;

namespace CourseValidator
{
    public class Topic : TocEntry
    {
        public Topic(XmlNode xmlNode)
        {
            Attributes = xmlNode.Attributes;
            XmlDoc = new XmlDocument();
            // TODO this["Link"] is a relative path ("/Content/EN/..."). How to add information about project root without coupling a Topic to a Course?
            if (File.Exists(this["Link"]))
            {
                XmlDoc.Load(this["Link"]);
            }
        }
    }
}
