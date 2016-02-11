using System.Xml;

namespace CourseValidator
{
    public class Topic : TocEntry
    {
        public Topic(XmlNode xmlNode)
        {
            Attributes = xmlNode.Attributes;
        }
    }
}
