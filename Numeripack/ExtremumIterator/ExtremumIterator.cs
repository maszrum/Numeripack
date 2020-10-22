using System;

namespace Numeripack
{
    public class ExtremumIterator<T, TArgument>
    {
        private readonly ExtremumIteratorParams<T, TArgument> _parameters;
        private readonly StepContext<T, TArgument> _context;

        public ExtremumIterator(ExtremumIteratorParams<T, TArgument> parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));

            _context = new StepContext<T, TArgument>(parameters.ArgumentSelector, parameters.Object);

            ExtremumComparer = GetStrategy(_parameters.ExtremumType);
        }

        public bool Finished { get; private set; }

        public int StepNumber { get; private set; }

        public ExtremumValue<TArgument> Current => _context.CurrentExtremum;

        public IExtremumStrategy ExtremumComparer { get; }

        public ExtremumValue<TArgument> FindResult()
        {
            while (Next())
            {
            }
            return _context.CurrentExtremum;
        }

        public bool Next()
        {
            if (Finished)
            {
                return false;
            }

            var objectiveValue = _parameters.Step(_context);
            if (IsInDomain(_context.Argument) && ShouldBeNewCurrent(objectiveValue))
            {
                _context.CurrentExtremum = new ExtremumValue<TArgument>(_context.Argument, objectiveValue);
            }

            StepNumber++;

            Finished = CheckEndingConditions();
            return !Finished;
        }

        private bool ShouldBeNewCurrent(double objectiveValue)
        {
            return Current == null 
                   || ExtremumComparer.ShouldBeNewCurrent(Current.Value, objectiveValue);
        }

        private bool IsInDomain(TArgument argument)
        {
            return _parameters.ArgumentDomain == null 
                   || _parameters.ArgumentDomain(argument);
        }

        private bool CheckEndingConditions()
        {
            if (_parameters.EndsIf != null && _parameters.EndsIf(_context))
            {
                return true;
            }

            return _parameters.MaximumSteps.HasValue && 
                   StepNumber >= _parameters.MaximumSteps.Value;
        }

        private static IExtremumStrategy GetStrategy(Extremum extremumType)
            => extremumType switch
            {
                Extremum.Minimum => new ExtremumMinimum(),
                Extremum.Maximum => new ExtremumMaximum(),
                Extremum.Zero => new ExtremumZero(),
                _ => throw new ArgumentOutOfRangeException(nameof(extremumType), "invalid value")

            };
    }
}
