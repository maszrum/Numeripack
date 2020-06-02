using System.Collections.Generic;

namespace Numeripack
{
    public interface IPositionalIncrementer<out T> : IEnumerable<IReadOnlyList<T>>
    {
        IReadOnlyList<T> Values { get; }
        int Positions { get; }
        bool IsZero { get; }
        bool IsMax { get; }
        int Combinations { get; }

        IReadOnlyList<T> Increment();
        void Reset();
    }
}
