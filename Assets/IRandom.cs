namespace Tanks
{
    public interface IRandom
    {
        int Next(int min, int max);
        
        int Next(int max);
    }
}