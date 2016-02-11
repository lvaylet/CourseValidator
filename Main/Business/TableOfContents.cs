using System.Collections.Generic;
using System.Xml;

namespace CourseValidator
{
    public class TableOfContents
    {
        private readonly XmlDocument _XmlDoc;

        public readonly List<Topic> Topics;
        public readonly List<Book> Books;

        public TableOfContents(string Filename)
        {
            _XmlDoc = new XmlDocument();
            //_XmlDoc.Load(Filename); // for a file argument
            _XmlDoc.LoadXml(Filename); // for an XML string

            XmlNodeList topics = _XmlDoc.SelectNodes(Utils.XPATH_REQUEST_FOR_TOPICS);
            Topics = new List<Topic>();
            foreach (XmlNode t in topics)
            {
                Topics.Add(new Topic(t));
            }

            XmlNodeList books = _XmlDoc.SelectNodes(Utils.XPATH_REQUEST_FOR_BOOKS);
            Books = new List<Book>();
            foreach (XmlNode b in books)
            {
                Books.Add(new Book(b));
            }
        }
    }
}
