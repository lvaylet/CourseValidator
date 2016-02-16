using System;

namespace CourseValidator
{
    public sealed class PredicateSpecification<T> : Specification<T>
    {
        public PredicateSpecification(Func<T, bool> predicate, string errorMessage)
        {
            Predicate = predicate;
            ErrorMessage = errorMessage;
        }

        public override bool IsSatisfiedBy(T instance)
        {
            return Predicate.Invoke(instance);
        }

        public override void DisplayDebugInformation(T instance)
        {
            Console.WriteLine(ErrorMessage);
        }

        public readonly Func<T, bool> Predicate;
        public readonly string ErrorMessage;
    }
}
