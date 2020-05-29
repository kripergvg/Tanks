using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Tanks.Pool
{
    public class SimplePool<TObject> : IPool<TObject>
        where TObject : IPoolable
    {
        private readonly ILogger _logger;
        private readonly int _maxCount;
        private readonly Func<IPool<TObject>, TObject> _factory;
        private readonly Stack<TObject> _pooledObjects;

        public SimplePool(ILogger logger,
            int maxCount, 
            int startCount,
            Func<IPool<TObject>, TObject> factory)
        {
            _logger = logger;
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
            TObject targetObject;
            if (_pooledObjects.Count != 0)
            {
                targetObject= _pooledObjects.Pop();
            }
            else
            {
                _logger.LogInformation("Creating new object in pool {type}",typeof(TObject).ToString());
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
                _logger.LogWarning("Returned object doesn't fit in pool {type}",typeof(TObject).ToString());
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