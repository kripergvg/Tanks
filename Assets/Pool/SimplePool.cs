using System;
using System.Collections.Generic;

namespace Tanks.Pool
{
    public class SimplePool<TObject> : IPool<TObject>
        where TObject : IPoolable
    {
        private readonly int _maxCount;
        private readonly Func<IPool<TObject>, TObject> _factory;
        private readonly Stack<TObject> _pooledObjects;

        public SimplePool(int maxCount, int startCount, Func<IPool<TObject>, TObject> factory)
        {
            _maxCount = maxCount;
            _factory = factory;
            _pooledObjects = new Stack<TObject>(startCount);
            for (int i = 0; i < startCount; i++)
            {
                var target = CreateTargetObject();
                _pooledObjects.Push(target);
            }
        }

        public TObject Get()
        {
            TObject targetObject = default;
            if (_pooledObjects.Count != 0)
            {
                targetObject= _pooledObjects.Pop();
            }
            else
            {
                // TODO Warning
                targetObject = CreateTargetObject();
            }

            targetObject.PoolInit();
            return targetObject;
        }

        public void Return(TObject target)
        {
            target.PoolClear();
            
            if (_pooledObjects.Count < _maxCount)
            {
                _pooledObjects.Push(target);
            }
            else
            {
                // TODO Warning
            }
        }

        private TObject CreateTargetObject()
        {
            var target = _factory(this);
            target.PoolClear();
            return target;
        }
    }
}