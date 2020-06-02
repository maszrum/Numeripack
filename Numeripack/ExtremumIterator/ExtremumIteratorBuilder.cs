using System;

namespace Numeripack
{
    public class ExtremumIteratorBuilder
    {
        private Extremum? _extremumType;

        public ExtremumIteratorBuilder Find(Extremum extremum)
        {
            if (_extremumType.HasValue)
            {
                throw new InvalidOperationException(
                    "method can be called only once");
            }

            _extremumType = extremum;
            return this;
        }

        public ExtremumIteratorBuilderSelector<T> ForObject<T>(T obj)
            where T : class
        {
            if (!_extremumType.HasValue)
            {
                throw new InvalidOperationException(
                    $"specify extremum type before using {nameof(Find)} method");
            }

            return new ExtremumIteratorBuilderSelector<T>(_extremumType.Value, obj);
        }
    }
}
