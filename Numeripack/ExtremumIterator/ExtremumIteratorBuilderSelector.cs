using System;

namespace Numeripack
{
    public class ExtremumIteratorBuilderSelector<T>
        where T : class
    {
        private readonly Extremum _extremumType;
        private readonly T _object;

        public ExtremumIteratorBuilderSelector(Extremum extremumType, T obj)
        {
            _extremumType = extremumType;
            _object = obj ?? throw new ArgumentNullException(nameof(obj));
        }

        public ExtremumIteratorBuilderArgument<T, TArgument> SelectArgument<TArgument>(
            Func<T, TArgument> selector)
        {
            var parameters = new ExtremumIteratorParams<T, TArgument>()
            {
                ArgumentSelector = selector,
                ExtremumType = _extremumType,
                Object = _object
            };

            return new ExtremumIteratorBuilderArgument<T, TArgument>(parameters);
        }
    }
}
