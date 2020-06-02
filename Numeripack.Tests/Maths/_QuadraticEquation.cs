namespace Numeripack.Tests.Maths
{
    internal class QuadraticEquation
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;

        public double Argument { get; set; } = 0.0;

        public QuadraticEquation(double a, double b, double c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        public double GetValue()
        {
            return (_a * Argument * Argument) + (_b * Argument) + _c;
        }

        public double GetDerivative()
        {
            return (2 * _a * Argument) + _b;
        }
    }
}
