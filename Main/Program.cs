using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// TODO Add a different kind of rule, based on a simple predicate, for enforcing the number of Topics in a TOC

namespace CourseValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Course from the provided or a local Table of Contents file (*.fltoc)
            Course course = new Course(args.Length == 1 ? args[0] : Properties.Resources.Lab_Guide_EN);

            // Some assertions about this specific Course
            //Debug.Assert(course.Toc.Topics.Count == 4, "TOC should have 4 top-level Topics.");
            //Debug.Assert(course.Toc.Books.Count == 11, "TOC should have 11 top-level Books (i.e. Labs)");
            //Debug.Assert(course.Toc.Books[0].Topics.Count == 11, "First Book (i.e. Lab) should have 11 Topics");
            //Debug.Assert(course.Toc.Books[0].Books.Count == 0, "First Book (i.e. Lab) should have no nested Books (i.e. no nested Labs)");

            // Reset Console textcolor to white
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("===============================================================================");
            Console.WriteLine("-= !! Bad TOCs !! =-");
            
            // Rules for Table of Contents
            List<PredicateSpecification<TableOfContents>> TocRules = new List<PredicateSpecification<TableOfContents>>();
            TocRules.Add(new PredicateSpecification<TableOfContents>(
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
            TocRules.Add(new PredicateSpecification<TableOfContents>(
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
            TocRules.Add(new PredicateSpecification<TableOfContents>(
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
            TocRules.Add(new PredicateSpecification<TableOfContents>(
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
                    DisplayDebugInformation(r, course.Toc);
                }
            }

            Console.WriteLine("===============================================================================");
            Console.WriteLine("-= !! Bad Books !! =-");

            // Rules for Books
            List<PredicateSpecification<Book>> BookRules = new List<PredicateSpecification<Book>>();
            BookRules.Add(new PredicateSpecification<Book>(
                (b) => { return b; },
                new TupleList<string, Func<string, string, bool>, string> {
                    { "Link", (string attribute, string value) => attribute.EndsWith(value), "/Description.htm" }
                },
                "A Book should link a to Description.htm Topic."));
            BookRules.Add(new PredicateSpecification<Book>(
                (b) => { return b.Topics.First(); },
                new TupleList<string, Func<string, string, bool>, string> {
                    { "Title", string.Equals, "Overview" },
                    { "Link", (string attribute, string value) => attribute.EndsWith(value), "/Overview.htm" }
                },
                "The first Topic of each Lab should be an Overview."));
            BookRules.Add(new PredicateSpecification<Book>(
                (b) => { return b.Topics.Last(); },
                new TupleList<string, Func<string, string, bool>, string> {
                    { "Title", string.Equals, "Wrap-Up" },
                    { "Link", (string attribute, string value) => attribute.EndsWith(value), "/Wrap-Up.htm" }
                },
               "The last Topic of each Lab should be a Wrap-Up."));

            // Apply each rule sequentially and display error message if necessary
            foreach (var r in BookRules)
            {
                var badBooksForCurrentRule = course.Toc.Books.FindAll(item => !r.IsSatisfiedBy(item)); // Use Specification.Not ?
                foreach (var b in badBooksForCurrentRule)
                {
                    DisplayDebugInformation(r, b);
                }
            }

            Console.ReadLine();
        }

        private static void DisplayDebugInformation<T>(PredicateSpecification<T> rule, T objectUnderTest)
        {
            Console.WriteLine("-------------------------------------------------------------------------------");

            Console.WriteLine(rule.ErrorMessage);

            // Only display the attributes values that differ
            var attributesThatDiffer = rule.ExpectedValues.FindAll(ev => rule.GetTocEntryUnderTest.Invoke(objectUnderTest)[ev.Item1] != ev.Item3);

            Console.WriteLine("Expected: ");
            Console.WriteLine("  " + string.Join(Environment.NewLine + "  ", attributesThatDiffer.Select(ev => ev.Item1 + " = " + ev.Item3)));

            Console.WriteLine("Got: ");
            Console.WriteLine("  " + string.Join(Environment.NewLine + "  ", attributesThatDiffer.Select(ev => ev.Item1 + " = " + rule.GetTocEntryUnderTest.Invoke(objectUnderTest)[ev.Item1])));
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