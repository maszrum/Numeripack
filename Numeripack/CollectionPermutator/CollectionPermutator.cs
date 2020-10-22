using System;
using System.Collections.Generic;
using System.Linq;

namespace Numeripack
{
    public class CollectionPermutator<TElement, TIdent>
    {
        private readonly PositionalIncrementer _incrementer;
        private readonly TElement[] _elements;
        private readonly TIdent[] _identifiers;

        private List<PermutedCollection<TElement, TIdent>> _current;

        private bool _finishOnNext;

        public CollectionPermutator(
            IEnumerable<TElement> elements,
            IEnumerable<TIdent> collectionIdentifiers)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }
            if (collectionIdentifiers == null)
            {
                throw new ArgumentNullException(nameof(collectionIdentifiers));
            }

            _elements = elements.ToArray();
            _identifiers = collectionIdentifiers.ToArray();

            if (_elements.Length  == 0)
            {
                throw new ArgumentException(
                    "must contain at least one element", nameof(elements));
            }
            if (_identifiers.Length < 2)
            {
                throw new ArgumentException(
                    "must contain at least two identifiers", nameof(collectionIdentifiers));
            }

            _incrementer = new PositionalIncrementer(_identifiers.Length, _elements.Length);
        }

        public int Combinations => _incrementer.Combinations;

        public IReadOnlyList<IPermutedCollection<TElement, TIdent>> Current => _current;

        public bool Permute()
        {
            var incrementerValues = _incrementer.Values;

            var pc = _identifiers.Select(i => new PermutedCollection<TElement, TIdent>(i));
            _current = new List<PermutedCollection<TElement, TIdent>>(pc);

            for (var elementIndex = 0; elementIndex < incrementerValues.Count; elementIndex++)
            {
                var collectionIndex = incrementerValues[elementIndex];
                _current[collectionIndex].Add(_elements[elementIndex]);
            }

            _incrementer.Increment();

            var finish = _finishOnNext;
            _finishOnNext = _incrementer.IsZero;
            return !finish;
        }
    }
}
