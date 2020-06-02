namespace Numeripack
{
    public interface ITreePermutator<out T>
    {
        IPermutationPath<T> Current { get; }
        bool Finished { get; }

        bool MoveNext();
        IPermutationTreeResults<T> Finish();
    }
}
