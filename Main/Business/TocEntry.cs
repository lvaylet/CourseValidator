using System;
using System.Xml;

namespace CourseValidator
{
    public class TocEntry
    {
        protected XmlAttributeCollection Attributes;
        protected XmlDocument XmlDoc;
        protected Course _course; // Reference to the Course the TopicEntry is attached to. Makes it easier to get the value of ProjectRoot.

        // Overload index operator to enable Topic["Title"] instead of Topic.Attributes["Title"].Value
        public string this[string key]
        {
            get
            {
                try
                {
                    return Attributes[key].Value;
                }
                catch (NullReferenceException e)
                {
                    // The requested Attribute does not exist
                    return "<Missing>";
                }
            }
        }
    }
}
