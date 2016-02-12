using System.Collections.Generic;
using System.Xml;

namespace CourseValidator
{
    public class TableOfContents
    {
        private readonly XmlDocument XmlDoc;

        public readonly List<Topic> Topics;
        public readonly List<Book> Books;

        public TableOfContents(string Filename)
        {
            XmlDoc = new XmlDocument();
            // Load TOC from filename provided as an argument (release mode) or from local Table of Contents file (debug mode)
#if DEBUG
            XmlDoc.LoadXml(Filename); // for an XML string
#else
            XmlDoc.Load(Filename); // for a file argument
#endif

            XmlNodeList topics = XmlDoc.SelectNodes(Utils.XPATH_REQUEST_FOR_TOPICS);
            Topics = new List<Topic>();
            foreach (XmlNode t in topics)
            {
                Topics.Add(new Topic(t));
            }

            XmlNodeList books = XmlDoc.SelectNodes(Utils.XPATH_REQUEST_FOR_BOOKS);
            Books = new List<Book>();
            foreach (XmlNode b in books)
            {
                Books.Add(new Book(b));
            }
        }
    }
}
