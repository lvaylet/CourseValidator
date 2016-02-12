using System.Collections.Generic;
using System.Xml;

namespace CourseValidator
{
    public class TableOfContents
    {
        private readonly XmlDocument XmlDoc;

        public readonly List<Topic> Topics;
        public readonly List<Book> Books;

        private readonly Course _course; // Store a reference to the Couse this TableOfContents is attached to

        public TableOfContents(string Filename, Course c)
        {
            _course = c; // Store a reference to the Couse this Topic is attached to

            XmlDoc = new XmlDocument();
            // Load TOC from filename provided as an argument (release mode) or stored locally as a Resource (debug mode)
#if DEBUG
            XmlDoc.LoadXml(Filename); // for an XML string
#else
            XmlDoc.Load(Filename); // for a file argument
#endif

            XmlNodeList topics = XmlDoc.SelectNodes(Utils.XPATH_REQUEST_FOR_TOPICS);
            Topics = new List<Topic>();
            foreach (XmlNode t in topics)
            {
                Topics.Add(new Topic(t, c));
            }

            XmlNodeList books = XmlDoc.SelectNodes(Utils.XPATH_REQUEST_FOR_BOOKS);
            Books = new List<Book>();
            foreach (XmlNode b in books)
            {
                Books.Add(new Book(b, c));
            }
        }
    }
}
