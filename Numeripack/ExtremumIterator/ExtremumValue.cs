namespace Numeripack
{
    public class ExtremumValue<TArgument>
    {
        public ExtremumValue(TArgument argument, double value)
        {
            Argument = argument;
            Value = value;
        }

        public TArgument Argument { get; }
        public double Value { get; }
    }
}
