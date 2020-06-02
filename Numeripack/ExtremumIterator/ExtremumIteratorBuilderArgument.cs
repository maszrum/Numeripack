using System;

namespace Numeripack
{
    public class ExtremumIteratorBuilderArgument<T, TArgument>
    {
        private readonly ExtremumIteratorParams<T, TArgument> _parameters;

        public ExtremumIteratorBuilderArgument(ExtremumIteratorParams<T, TArgument> parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));

            if (parameters.Object == null)
            {
                throw new ArgumentNullException(
                    nameof(parameters.Object));
            }
            if (parameters.ArgumentSelector == null)
            {
                throw new ArgumentNullException(
                    nameof(parameters.ArgumentSelector));
            }
        }

        public ExtremumIteratorBuilderArgument<T, TArgument> WithStep(
            Func<IStepContext<T, TArgument>, double> step)
        {
            if (_parameters.Step != null)
            {
                throw new InvalidOperationException(
                    "method can be called only once");
            }

            _parameters.Step = step ?? throw new ArgumentNullException(nameof(step));

            return this;
        }

        public ExtremumIteratorBuilderArgument<T, TArgument> WithArgumentDomain(
            Predicate<TArgument> predicate)
        {
            if (_parameters.ArgumentDomain != null)
            {
                throw new InvalidOperationException(
                    "method can be called only once");
            }

            _parameters.ArgumentDomain = predicate ?? throw new ArgumentNullException(nameof(predicate));

            return this;
        }

        public ExtremumIteratorBuilderArgument<T, TArgument> EndsIf(
            Predicate<IStepContext<T, TArgument>> predicate)
        {
            if (_parameters.EndsIf != null)
            {
                throw new InvalidOperationException(
                    "method can be called only once");
            }

            _parameters.EndsIf = predicate;

            return this;
        }

        public ExtremumIteratorBuilderArgument<T, TArgument> WithMaximumSteps(int steps)
        {
            if (_parameters.MaximumSteps.HasValue)
            {
                throw new InvalidOperationException(
                    "method can be called only once");
            }

            _parameters.MaximumSteps = steps;

            return this;
        }

        public ExtremumIterator<T, TArgument> Build()
        {
            if (_parameters.EndsIf == null && !_parameters.MaximumSteps.HasValue)
            {
                throw new InvalidOperationException(
                    "ending condition and maximum steps was not specified");
            }
            if (_parameters.Step == null)
            {
                throw new InvalidOperationException(
                    "step was not defined");
            }

            return new ExtremumIterator<T, TArgument>(_parameters);
        }
    }
}
