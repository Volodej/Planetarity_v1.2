using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Planetarity.Utils
{
    public class ObjectsPool<T> where T : Component
    {
        private readonly Transform _orphansRoot;
        
        private readonly Stack<T> _objects = new Stack<T>();
        private readonly Func<T> _createFunc;
        private readonly Action<T> _resetFunc;

        public ObjectsPool(Func<T> createFunc, Action<T> resetFunc, Transform orphansRoot)
        {
            _createFunc = createFunc;
            _resetFunc = resetFunc;
            _orphansRoot = orphansRoot;
        }

        public T Borrow()
        {
            return _objects.Count > 0 ? _objects.Pop() : _createFunc();
        }

        public void Release(T value)
        {
            _resetFunc(value);
            _objects.Push(value);
            value.transform.SetParent(_orphansRoot);
        }
    }
}