using System.Collections.Generic;

namespace Numeripack
{
    public interface IPermutator<out T> : IEnumerable<T[]>
    {
        int Combinations { get; }

        T[] Permute();
        void Reset();
    }
}