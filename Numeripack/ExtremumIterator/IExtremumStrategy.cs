namespace Numeripack
{
    public interface IExtremumStrategy
    {
        bool ShouldBeNewCurrent(double current, double candidate);
    }
}
