using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseValidator
{
    public class EmptySpecification<T> : Specification<T>
    {
        public EmptySpecification()
        {
        }

        public override void DisplayDebugInformation(T instance)
        {
            throw new NotImplementedException();
        }

        public override bool IsSatisfiedBy(T item)
        {
            return true;
        }
    }
}
