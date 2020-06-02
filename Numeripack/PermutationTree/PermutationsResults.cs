using System.Collections;
using System.Collections.Generic;

namespace Numeripack
{
    internal class PermutationsResults<T> : IPermutationTreeResults<T>
    {
        private readonly List<List<T>> _paths = new List<List<T>>();

        public IReadOnlyList<T> this[int index] => _paths[index];

        public int Count => _paths.Count;

        public void Add(List<T> path)
        {
            _paths.Add(path);
        }

        public IEnumerator<IReadOnlyList<T>> GetEnumerator()
        {
            return _paths.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _paths.GetEnumerator();
        }
    }
}
