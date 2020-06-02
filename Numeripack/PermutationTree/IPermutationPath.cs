using System.Collections.Generic;

namespace Numeripack
{
    public interface IPermutationPath<out T>
    {
        bool IsFull { get; }
        IReadOnlyList<T> Elements { get; }

        void SkipBranch();
    }
}
