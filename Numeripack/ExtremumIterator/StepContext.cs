using System;

namespace Numeripack
{
    public class StepContext<T, TArgument> : IStepContext<T, TArgument>
    {
        private readonly Func<T, TArgument> _argumentSelector;

        public StepContext(Func<T, TArgument> argumentSelector, T obj)
        {
            _argumentSelector = argumentSelector;
            Object = obj;
        }

        public T Object { get; }

        public TArgument Argument => _argumentSelector(Object);

        public ExtremumValue<TArgument> CurrentExtremum { get; set; }
    }
}
