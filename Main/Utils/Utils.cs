using System;
using System.Collections.Generic;

namespace CourseValidator
{
    internal static class Utils
    {
        // TODO Refactor these XPath requests for detecting Topics and Books so they also work with nested Books
        public const string XPATH_REQUEST_FOR_BOOKS = "/CatapultToc/TocEntry[TocEntry]";      // Root-level TocEntry nodes with at least one TocEntry child
        public const string XPATH_REQUEST_FOR_TOPICS = "/CatapultToc/TocEntry[not(TocEntry)]"; // Root-level TocEntry nodes with no TocEntry children
    }

    // Helper class to initialize a List of Tuples 
    public class TupleList<T1, T2, T3> : List<Tuple<T1, T2, T3>>
    {
        public void Add(T1 item, T2 item2, T3 item3)
        {
            Add(new Tuple<T1, T2, T3>(item, item2, item3));
        }
    }
}
