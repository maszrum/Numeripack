using System;

namespace Numeripack
{
    public class ExtremumIteratorParams<T, TArgument>
    {
        public Extremum ExtremumType { get; set; }
        public T Object { get; set; }
        public Func<T, TArgument> ArgumentSelector { get; set; }
        public Func<IStepContext<T, TArgument>, double> Step { get; set; }
        public Predicate<TArgument> ArgumentDomain { get; set; }
        public Predicate<IStepContext<T, TArgument>> EndsIf { get; set; }
        public int? MaximumSteps { get; set; }
    }
}
