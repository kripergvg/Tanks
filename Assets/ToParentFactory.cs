using UnityEngine;

namespace Tanks
{
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
}