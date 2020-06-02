namespace Numeripack
{
    public interface IExtremumIterator<TArgument>
    {
        ExtremumValue<TArgument> Current { get; }
        bool Finished { get; }
        int StepNumber { get; }

        ExtremumValue<TArgument> FindResult();
        bool Next();
    }
}