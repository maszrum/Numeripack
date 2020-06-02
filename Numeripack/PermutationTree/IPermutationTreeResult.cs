using System.Collections.Generic;

namespace Numeripack
{
    public interface IPermutationTreeResults<out T> : IReadOnlyList<IReadOnlyList<T>>
    {
    }
}
