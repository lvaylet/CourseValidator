using System.Collections.Generic;
using System.Xml.Serialization;

namespace CourseValidator
{
    [XmlRoot("Courses")]
    public class Courses : List<Course> { }
}
