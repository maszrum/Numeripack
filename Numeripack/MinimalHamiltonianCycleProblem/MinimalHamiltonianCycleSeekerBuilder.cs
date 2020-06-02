using System;
using System.Collections.Generic;

namespace Numeripack
{
    public class MinimalHamiltonianCycleSeekerBuilder<T>
    {
        private IEnumerable<T> _elements;
        private Func<T, T, int, double> _costFunction;

        public MinimalHamiltonianCycleSeekerBuilder<T> ForElements(IEnumerable<T> elements)
        {
            if (_elements != null)
            {
                throw new InvalidOperationException(
                    "method can be called once");
            }

            _elements = elements;

            return this;
        }

        public MinimalHamiltonianCycleSeekerBuilder<T> WithCostFunction(Func<T, T, int, double> function)
        {
            if (_costFunction != null)
            {
                throw new InvalidOperationException(
                    "method can be called once");
            }

            _costFunction = function;

            return this;
        }

        public MinimalHamiltonianCycleSeeker<T> Build()
        {
            if (_elements == null)
            {
                throw new ArgumentNullException(
                    nameof(_elements), $"specify elements with {nameof(ForElements)} method");
            }
            if (_costFunction == null)
            {
                throw new ArgumentNullException(
                    nameof(_costFunction), $"specify cost function with {nameof(WithCostFunction)} method");
            }

            return new MinimalHamiltonianCycleSeeker<T>(_elements, _costFunction);
        }
    }
}
