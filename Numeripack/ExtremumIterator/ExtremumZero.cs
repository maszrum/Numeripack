using System;

namespace Numeripack
{
    internal class ExtremumZero : IExtremumStrategy
    {
        public bool ShouldBeNewCurrent(double current, double candidate)
        {
            return Math.Abs(candidate) < Math.Abs(current);
        }
    }
}
