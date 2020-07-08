using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Numeripack
{
    public class PositionalIncrementer : IPositionalIncrementer<int>
    {
        private readonly int _baseNumber;
        private readonly int _maxValue;
        private readonly List<int> _values;
        private readonly Lazy<int> _combinations;

        public PositionalIncrementer(int baseNumber, int positions)
        {
            if (baseNumber < 2)
                throw new ArgumentOutOfRangeException(nameof(baseNumber), "must be greater than or equal to 2");
            if (positions <= 0)
                throw new ArgumentOutOfRangeException(nameof(positions), "must be positive");

            _baseNumber = baseNumber;
            _maxValue = baseNumber - 1;

            _values = Enumerable
                .Repeat(0, positions)
                .ToList();

            _combinations = new Lazy<int>(() => _baseNumber.Pow(Positions));
        }

        public IReadOnlyList<int> Values => _values;

        public int Positions => _values.Count;

        public bool IsZero => _values.All(v => v == 0);

        public bool IsMax => _values.All(v => v == _maxValue);

        public int Combinations => _combinations.Value;

        public IReadOnlyList<int> Increment()
        {
            _values[0]++;

            for (var i = 0; i < _values.Count; i++)
            {
                if (_values[i] == _baseNumber)
                {
                    _values[i] = 0;
                    
                    if (i < _values.Count - 1)
                    {
                        _values[i + 1]++;
                    }
                }
                else
                {
                    break;
                }
            }

            return Values;
        }

        public void Reset()
        {
            for (var i = 0; i < _values.Count; i++)
            {
                _values[i] = 0;
            }
        }

        public IEnumerator<IReadOnlyList<int>> GetEnumerator()
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
