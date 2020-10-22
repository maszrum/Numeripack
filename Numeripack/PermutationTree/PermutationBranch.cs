using System;
using System.Collections.Generic;
using System.Linq;

namespace Numeripack
{
    internal class PermutationBranch<T>
    {
        private readonly List<T> _toVisit;
        private readonly List<T> _visited = new List<T>();
        private readonly bool _isLast;

        private PermutationBranch<T> _inner;
        private bool _skipNext;

        public PermutationBranch(IEnumerable<T> toVisit)
        {
            if (toVisit == null)
            {
                throw new ArgumentNullException(nameof(toVisit));
            }

            _toVisit = toVisit.ToList();
            _isLast = _toVisit.Count == 1;

            if (_toVisit.Count == 0)
            {
                throw new ArgumentException(
                    "must contain at least one element", nameof(toVisit));
            }
        }

        public IReadOnlyList<T> ToVisit => _toVisit;

        public bool AllVisited => _toVisit.Count == 0;

        public T Current => _toVisit[0];

        private void MarkCurrentAsVisited()
        {
            _visited.Add(Current);
            _toVisit.RemoveAt(0);
        }
        
        public bool VisitNext()
        {
            if (_isLast)
            {
                return true;
            }

            if (_skipNext)
            {
                MarkCurrentAsVisited();
                _inner = null;
                _skipNext = false;
                return AllVisited;
            }

            if (_inner == null)
            {
                _inner = GetNewPermutationNode();
                return false;
            }

            var innerFinished = _inner.VisitNext();
            if (innerFinished)
            {
                MarkCurrentAsVisited();
                _inner = null;
                return AllVisited;
            }

            return false;
        }

        public void Skip(int depth)
        {
            if (depth == 0)
            {
                _skipNext = true;
            }
            else
            {
                if (_inner == null)
                {
                    throw new InvalidOperationException(
                        "cannot skip not existing path");
                }
                depth--;
                _inner.Skip(depth);
            }
        }

        private PermutationBranch<T> GetNewPermutationNode()
        {
            var innerCollection = _toVisit
                .Skip(1)
                .Concat(_visited);
            return new PermutationBranch<T>(innerCollection);
        }

        public void GetPath(List<T> path)
        {
            path.Add(Current);

            _inner?.GetPath(path);
        }
    }
}
