using System.Collections;
using System.Collections.Generic;

namespace Numeripack
{
    internal class PermutedCollection<TElement, TIdent> : IPermutedCollection<TElement, TIdent>
    {
        private readonly List<TElement> _list = new List<TElement>();

        public PermutedCollection(TIdent identifier)
        {
            Identifier = identifier;
        }

        public TElement this[int index] => _list[index];

        public int Count => _list.Count;

        public TIdent Identifier { get; }

        public void Add(TElement item)
        {
            _list.Add(item);
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
