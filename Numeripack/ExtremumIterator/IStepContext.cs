namespace Numeripack
{
    public interface IStepContext<out T, TArgument>
    {
        T Object { get; }
        TArgument Argument { get; }
        ExtremumValue<TArgument> CurrentExtremum { get; }
    }
}
