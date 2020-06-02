namespace Numeripack
{
    internal class ExtremumMaximum : IExtremumStrategy
    {
        public bool ShouldBeNewCurrent(double current, double candidate)
        {
            return candidate > current;
        }
    }
}
