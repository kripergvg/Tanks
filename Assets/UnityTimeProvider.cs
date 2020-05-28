using Tanks.Mobs;
using UnityEngine;
using Random = System.Random;

namespace Tanks
{
    public class UnityTimeProvider : ITimeProvider
    {
        public float Time => UnityEngine.Time.time;
    }

    public interface ITimeProvider
    {
        float Time { get; }
    }
    
    public interface IEntityFactory<out T>
    {
        T Create(in EntityDeps deps);
    }
    
    public interface IFactory<out T>
    {
        T Create();
    }

    public readonly struct ToParentFactory<T, TCast> : IFactory<TCast>
        where T : Object, TCast
    {
        private readonly T _target;
        private readonly Transform _parent;

        public ToParentFactory(T target, Transform parent)
        {
            _target = target;
            _parent = parent;
        }

        public TCast Create()
        {
            return (TCast) Object.Instantiate(_target, _parent);
        }
    }

    public interface IRandom
    {
        int Next(int min, int max);
        
        int Next(int max);
    }

    public class DefaultRandom : IRandom
    {
        private readonly Random _random;

        public DefaultRandom(Random random)
        {
            _random = random;
        }

        public int Next(int min, int max)
        {
            return _random.Next(min, max);
        }

        public int Next(int max)
        {
            return _random.Next(max);
        }
    }
}