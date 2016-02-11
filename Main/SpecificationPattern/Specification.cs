using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseValidator
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T item);

        public static Specification<T> operator &(Specification<T> left, Specification<T> right)
        {
            return new AndSpecification<T>(left, right);
        }

        public static Specification<T> operator |(Specification<T> left, Specification<T> right)
        {
            return new OrSpecification<T>(left, right);
        }

        public static Specification<T> operator !(Specification<T> specification)
        {
            return new NotSpecification<T>(specification);
        }

        public static bool operator true(Specification<T> specification)
        {
            return true;
        }

        public static bool operator false(Specification<T> specification)
        {
            return false;
        }
    }

    public static class Specification
    {
        public static Specification<T> And<T>(params ISpecification<T>[] specifications)
        {
            return new AndSpecification<T>(specifications);
        }

        public static Specification<T> Or<T>(params ISpecification<T>[] specifications)
        {
            return new OrSpecification<T>(specifications);
        }

        public static Specification<T> Not<T>(ISpecification<T> specification)
        {
            return new NotSpecification<T>(specification);
        }

        public static Specification<T> Empty<T>()
        {
            return new EmptySpecification<T>();
        }
    }

    public class AndSpecification<T> : CompositeSpecification<T>
    {
        public AndSpecification(params ISpecification<T>[] specifications)
            : base(specifications)
        {
        }

        public override bool IsSatisfiedBy(T item)
        {
            return Specifications.All((s) => s.IsSatisfiedBy(item));
        }
    }

    public class OrSpecification<T> : CompositeSpecification<T>
    {
        public OrSpecification(params ISpecification<T>[] specifications)
            : base(specifications)
        {
        }

        public override bool IsSatisfiedBy(T item)
        {
            return Specifications.Any((s) => s.IsSatisfiedBy(item));
        }
    }

    public class NotSpecification<T> : Specification<T>
    {
        public NotSpecification(ISpecification<T> specification)
        {
            if (specification == null) { throw new ArgumentNullException("specification"); }
            this.specification = specification;
        }

        public override bool IsSatisfiedBy(T item)
        {
            return !specification.IsSatisfiedBy(item);
        }

        private readonly ISpecification<T> specification;
    }

    public abstract class CompositeSpecification<T> : Specification<T>
    {
        public IEnumerable<ISpecification<T>> Specifications
        {
            get { return specifications; }
        }

        protected CompositeSpecification(ICollection<ISpecification<T>> specifications)
        {
            this.specifications = specifications;
        }

        private readonly ICollection<ISpecification<T>> specifications = new HashSet<ISpecification<T>>();
    }
}
