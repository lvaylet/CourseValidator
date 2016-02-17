using System.Xml.Serialization;

// Notes about serialization:
// 1. XML serialization only serializes public fields and properties.
// 2. XML serialization does not include any type information.
// 3. A default/non-parameterised constructor is needed in order to serialize an object. 
// 4. ReadOnly properties are not serialized.

namespace CourseValidator
{
    [XmlRoot("Courses")]
    public class Course
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("projectRoot")]
        public string ProjectRoot { get; set; }
        public TableOfContents Toc { get; set; }

        public Course()
        {
            Name = "";
            ProjectRoot = "";
            Toc = null;
        }

        public Course(string name, string projectRoot, string tocFile)
        {
            Name = name;
            ProjectRoot = projectRoot;
            Toc = new TableOfContents(tocFile, this);
        }
    }
}
