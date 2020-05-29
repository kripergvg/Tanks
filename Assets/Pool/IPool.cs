namespace Tanks.Pool
{
    public interface IPool<TObject>
        where TObject : IPoolable
    {
        TObject Get();

        void Return(TObject target);
    }
}