using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Course from a TOC filename provided as an argument (release mode) or a local Table of Contents file (debug mode)
#if DEBUG
            Course course = new Course(@"C:\Users\lvaylet\Documents\My Projects\di-basics", Properties.Resources.Lab_Guide_EN);
#else
            Course course = new Course(args[0], args[1]);
#endif

            // Some assertions about this specific Course
            //Debug.Assert(course.Toc.Topics.Count == 4, "TOC should have 4 top-level Topics.");
            //Debug.Assert(course.Toc.Books.Count == 11, "TOC should have 11 top-level Books (i.e. Labs)");
            //Debug.Assert(course.Toc.Books[0].Topics.Count == 11, "First Book (i.e. Lab) should have 11 Topics");
            //Debug.Assert(course.Toc.Books[0].Books.Count == 0, "First Book (i.e. Lab) should have no nested Books (i.e. no nested Labs)");

            // Reset Console textcolor to white
            //Console.ForegroundColor = ConsoleColor.White;

            // Rules for Table of Contents
            List<Specification<TableOfContents>> TocRules = new List<Specification<TableOfContents>>();
            TocRules.Add(new PredicateSpecification<TableOfContents>(
                (t) => { return t.Topics.Count == 4; }, 
                "A TOC should have only 4 Topics: Title, Copyright, Welcome, Contents"));
            TocRules.Add(new ByAttributesSpecification<TableOfContents>(
                (t) => { return t.Topics[0]; },
                new TupleList<string, Func<string, string, bool>, string> {
                    { "Title", string.Equals, "Title" },
                    { "Link", string.Equals, "/Content/Print Only Topics/Title.htm" },
                    { "BreakType", string.Equals, "pageLayout" },
                    { "StartSection", string.Equals, "false" },
                    { "PageLayout", string.Equals, "/Content/Resources/PageLayouts/8_5X11/Title.flpgl" },
                    { "PageNumberReset", string.Equals, "reset" },
                    { "PageNumber", string.Equals, "1" },
                    { "SectionNumberReset", string.Equals, "continue" },
                    { "VolumeNumberReset", string.Equals, "same" },
                    { "ChapterNumberReset", string.Equals, "continue" },
                    { "PageType", string.Equals, "normal" }
                },
                "The first Topic of a TOC should be a properly configured Title page."));
            TocRules.Add(new ByAttributesSpecification<TableOfContents>(
                (t) => { return t.Topics[1]; },
                new TupleList<string, Func<string, string, bool>, string> {
                    { "Title", string.Equals, "Copyright" },
                    { "Link", string.Equals, "/Content/Print Only Topics/Copyright.htm" },
                    { "BreakType", string.Equals, "pageLayout" },
                    { "StartSection", string.Equals, "false" },
                    { "PageLayout", string.Equals, "/Content/Resources/PageLayouts/8_5X11/Copyright.flpgl" },
                    { "PageNumberReset", string.Equals, "continue" },
                    { "SectionNumberReset", string.Equals, "continue" },
                    { "VolumeNumberReset", string.Equals, "same" },
                    { "ChapterNumberReset", string.Equals, "continue" },
                },
                "The second Topic of a TOC should be a properly configured Copyright page."));
            TocRules.Add(new ByAttributesSpecification<TableOfContents>(
                (t) => { return t.Topics[2]; },
                new TupleList<string, Func<string, string, bool>, string> {
                    { "Title", string.Equals, "Welcome" },
                    { "Link", string.Equals, "/Content/Print Only Topics/Welcome.htm" },
                    { "BreakType", string.Equals, "pageLayout" },
                    { "StartSection", string.Equals, "false" },
                    { "PageLayout", string.Equals, "/Content/Resources/PageLayouts/8_5X11/Welcome.flpgl" },
                    { "SectionNumberReset", string.Equals, "continue" },
                    { "VolumeNumberReset", string.Equals, "same" },
                    { "ChapterNumberReset", string.Equals, "continue" },
                },
                "The third Topic of a TOC should be a properly configured Welcome page."));
            TocRules.Add(new ByAttributesSpecification<TableOfContents>(
                (t) => { return t.Topics[3]; },
                new TupleList<string, Func<string, string, bool>, string> {
                    { "Title", string.Equals, "TOC" },
                    { "Link", string.Equals, "/Content/Print Only Topics/TOC.htm" },
                    { "BreakType", string.Equals, "pageLayout" },
                    { "StartSection", string.Equals, "false" },
                    { "PageLayout", string.Equals, "/Content/Resources/PageLayouts/8_5X11/TOC.flpgl" },
                    { "PageNumberReset", string.Equals, "continue" },
                    { "SectionNumberReset", string.Equals, "continue" },
                    { "VolumeNumberReset", string.Equals, "same" },
                    { "ChapterNumberReset", string.Equals, "continue" },
                    { "PageType", string.Equals, "firstright" }
                },
                "The fourth Topic of a TOC should be a properly configured Contents page."));

            // Apply each rule sequentially and display error message if necessary
            foreach (var r in TocRules)
            {
                if (!r.IsSatisfiedBy(course.Toc))
                {
                    Console.WriteLine("-------------------------------------------------------------------------------");
                    r.DisplayDebugInformation(course.Toc);
                }
            }

            // Rules for Books
            List<Specification<Book>> BookRules = new List<Specification<Book>>();
            BookRules.Add(new ByAttributesSpecification<Book>(
                (b) => { return b; },
                new TupleList<string, Func<string, string, bool>, string> {
                    { "Link", (string attribute, string value) => attribute.EndsWith(value), "/Description.htm" }
                },
                "A Book should link a to properly configured Description.htm Topic."));
            BookRules.Add(new ByAttributesSpecification<Book>(
                (b) => { return b.Topics.First(); },
                new TupleList<string, Func<string, string, bool>, string> {
                    { "Title", string.Equals, "Overview" },
                    { "Link", (string attribute, string value) => attribute.EndsWith(value), "/Overview.htm" }
                },
                "The first Topic of each Lab should be a properly configured Overview."));
            BookRules.Add(new ByAttributesSpecification<Book>(
                (b) => { return b.Topics.Last(); },
                new TupleList<string, Func<string, string, bool>, string> {
                    { "Title", string.Equals, "Wrap-Up" },
                    { "Link", (string attribute, string value) => attribute.EndsWith(value), "/Wrap-Up.htm" }
                },
               "The last Topic of each Lab should be a properly configured Wrap-Up."));
            BookRules.Add(new PredicateSpecification<Book>(
                (b) => { return b.Topics.First().XmlDocumentHasThisNode("//MadCap:snippetBlock[@src='../../Resources/Snippets/TinCanWrapperForOverview.flsnp']"); },
               "The Overview should contain a link to the TinCanWrapperForOverview snippet."));

            // Apply each rule sequentially and display error message if necessary
            foreach (var r in BookRules)
            {
                var badBooksForCurrentRule = course.Toc.Books.FindAll(item => !r.IsSatisfiedBy(item)); // Use Specification.Not ?
                foreach (var b in badBooksForCurrentRule)
                {
                    Console.WriteLine("-------------------------------------------------------------------------------");
                    Console.WriteLine(b.GetTitle());
                    r.DisplayDebugInformation(b);
                }
            }

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}

//Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
//Debug.AutoFlush = true;
//Debug.Indent();
//Debug.WriteLine("Entering Main");
//Console.WriteLine("Hello World.");
//Debug.WriteLine("Exiting Main");
//Debug.Unindent();

//var BadBookSpecification = Specification.Not(Specification.And(LinksToDescriptionSpecification, FirstTopicIsOverviewSpecification, LastTopicIsWrapUpSpecification));

// According to De Morgan's laws, "(not A) or (not B)" is the same as "not (A and B)"