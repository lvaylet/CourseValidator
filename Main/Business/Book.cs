﻿using System.Collections.Generic;
using System.Xml;

namespace CourseValidator
{
    public class Book : TocEntry
    {
        public readonly List<Book> Books; // Books can be nested in Table of Contents
        public readonly List<Topic> Topics;

        public Book(XmlNode xmlNode)
        {
            Attributes = xmlNode.Attributes;

            // Book-level Topics
            XmlNodeList topics = xmlNode.SelectNodes("./TocEntry[not(TocEntry)]");
            Topics = new List<Topic>();
            foreach (XmlNode t in topics)
            {
                Topics.Add(new Topic(t));
            }

            // Nested Books in addition to Topics?
            XmlNodeList books = xmlNode.SelectNodes("./TocEntry[TocEntry]");
            Books = new List<Book>();
            foreach (XmlNode b in books)
            {
                Books.Add(new Book(b));
            }
        }
    }
}