using System;
using System.Collections.Generic;

namespace com.ethnicthv.util.pool
{
    public class Pool<T> where T : IPoolable
    {
        private readonly Stack<T> _pool = new();
        private readonly Func<T> _factory;

        public Pool(Func<T> factory)
        {
            _factory = factory;
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                return _pool.Pop();
            }
            return _factory();
        }

        public void Return(T obj)
        {
            obj.ResetInstance();
            _pool.Push(obj);
        }
    }
}