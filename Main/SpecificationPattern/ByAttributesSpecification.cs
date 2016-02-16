using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseValidator
{
    public sealed class ByAttributesSpecification<T> : Specification<T>
    {
        public ByAttributesSpecification(Func<T, TocEntry> getTestedTocEntry, List<Tuple<string, Func<string, string, bool>, string>> expectedValues, string errorMessage)
        {
            GetTocEntryUnderTest = getTestedTocEntry;
            ExpectedValues = expectedValues;
            ErrorMessage = errorMessage;
        }

        public override bool IsSatisfiedBy(T instance)
        {
            // Loop over each rule defined in ExpectedValues. Test will fail if any of the rule is not satisfied.
            var result = true;
            foreach (var tuple in ExpectedValues)
            {
                result &= tuple.Item2.Invoke(GetTocEntryUnderTest(instance)[tuple.Item1], tuple.Item3);
            }
            return result;
        }

        public override void DisplayDebugInformation(T instance)
        {
            Console.WriteLine(ErrorMessage);

            // Only display the attributes values that differ
            var attributesThatDiffer = ExpectedValues.FindAll(ev => GetTocEntryUnderTest.Invoke(instance)[ev.Item1] != ev.Item3);

            Console.WriteLine("Expected: ");
            Console.WriteLine("  " + string.Join(Environment.NewLine + "  ", attributesThatDiffer.Select(ev => ev.Item1 + " = " + ev.Item3)));

            Console.WriteLine("Got: ");
            Console.WriteLine("  " + string.Join(Environment.NewLine + "  ", attributesThatDiffer.Select(ev => ev.Item1 + " = " + GetTocEntryUnderTest.Invoke(instance)[ev.Item1])));
        }

        public Func<T, TocEntry> GetTocEntryUnderTest; // Takes T as only parameter, returns a TocEntry somewhere inside T (T itself, or T.Topics.First() or T.Books[0].Topics[0] or...)
        public readonly List<Tuple<string, Func<string, string, bool>, string>> ExpectedValues; // Item1 = attribute name, Item2 = test, Item3 = expected value
        public readonly string ErrorMessage;
    }
}
