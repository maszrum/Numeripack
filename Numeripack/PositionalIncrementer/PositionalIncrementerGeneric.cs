using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Numeripack
{
    public class PositionalIncrementer<T> : IPositionalIncrementer<T>
    {
        private readonly PositionalIncrementer _incrementer;
        private readonly T[] _elements;
        private readonly List<T> _values;

        public PositionalIncrementer(IEnumerable<T> fromElements, int positions)
        {
            _elements = fromElements.ToArray();

            if (_elements.Length < 2)
            {
                throw new ArgumentException(
                    "enumerable must contain at least two elements", nameof(fromElements));
            }

            _incrementer = new PositionalIncrementer(_elements.Length, positions);

            _values = Enumerable
                .Repeat(_elements[0], positions)
                .ToList();
        }

        public IReadOnlyList<T> Values => _values;

        public int Positions => _incrementer.Positions;

        public bool IsZero => _incrementer.IsZero;

        public bool IsMax => _incrementer.IsMax;

        public int Combinations => _incrementer.Combinations;

        public IReadOnlyList<T> Increment()
        {
            _incrementer.Increment();

            var incrementerValues = _incrementer.Values;
            for (var i = 0; i < incrementerValues.Count; i++)
            {
                _values[i] = _elements[incrementerValues[i]];
            }

            return Values;
        }

        public void Reset()
        {
            for (var i = 0; i < _values.Count; i++)
            {
                _values[i] = _elements[0];
            }
        }
        
        public IEnumerator<IReadOnlyList<T>> GetEnumerator()
        {
            do
            {
                yield return Values;
                Increment();
            }
            while (!IsZero);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
