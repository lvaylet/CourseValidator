using System.IO;
using System.Xml;

namespace CourseValidator
{
    public class Topic : TocEntry
    {
        private readonly string _absolutePath; // Full absolute path to Topic file

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
    }
}
