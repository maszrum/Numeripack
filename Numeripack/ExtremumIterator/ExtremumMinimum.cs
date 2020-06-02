namespace Numeripack
{
    internal class ExtremumMinimum : IExtremumStrategy
    {
        public bool ShouldBeNewCurrent(double current, double candidate)
        {
            return candidate < current;
        }
    }
}
