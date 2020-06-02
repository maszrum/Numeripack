using System.Collections.Generic;

namespace Numeripack
{
    public interface IPermutedCollection<out TElement, out TIdent> : IReadOnlyList<TElement>
    {
        TIdent Identifier { get; }
    }
}
