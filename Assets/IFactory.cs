namespace Tanks
{
    public interface IFactory<out T>
    {
        T Create();
    }
}