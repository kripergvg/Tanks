namespace Tanks.Pool
{
    public interface IPoolable
    {
        void PoolInit();
        
        void PoolClear();
    }
}