using System;
using System.Collections.Generic;
using System.Linq;

namespace Numeripack
{
    public class TreePermutator<T>
    {
        private readonly PermutationBranch<T> _root;
        private readonly PermutationsResults<T> _result;
        private readonly T[] _fromElements;
        private bool _skipped;
        private PermutationPath<T> _current;

        public TreePermutator(IEnumerable<T> fromElements)
        {
            if (fromElements == null)
            {
                throw new ArgumentNullException(nameof(fromElements));
            }

            _fromElements = fromElements as T[] ?? fromElements.ToArray();
            _root = new PermutationBranch<T>(_fromElements);
            _result = new PermutationsResults<T>();

            if (_fromElements.Length == 0)
            {
                throw new ArgumentException(
                    "must contain at least one element", nameof(fromElements));
            }
        }

        public IPermutationPath<T> Current => _current;

        public bool Finished { get; private set; }

        public bool MoveNext()
        {
            if (Finished)
            {
                throw new InvalidOperationException(
                    "tree traversal was finished");
            }
            
            if (_current != null)
            {
                if (!_skipped && _current.IsFull)
                {
                    _result.Add(_current.ElementsRw);
                }
                _skipped = false;

                Finished = _root.VisitNext();
            }

            if (!Finished)
            {
                _current = CreatePermutationPath();
            }

            return !Finished;
        }
        
        public IPermutationTreeResults<T> GetResult()
        {
            return _result;
        }

        private PermutationPath<T> CreatePermutationPath()
        {
            var path = new List<T>();
            _root.GetPath(path);

            var isFull = path.Count == _fromElements.Length;

            return new PermutationPath<T>(path, isFull, (depth) =>
            {
                _root.Skip(depth);
                _skipped = true;
            });
        }
    }
}
