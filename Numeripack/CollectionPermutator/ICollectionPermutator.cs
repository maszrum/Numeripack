using System.Collections.Generic;

namespace Numeripack
{
    internal interface ICollectionPermutator<out TElement, out TIdent>
    {
        int Combinations { get; }
        IReadOnlyList<IPermutedCollection<TElement, TIdent>> Current { get; }

        bool Permute();
    }
}
