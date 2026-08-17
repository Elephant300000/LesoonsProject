using System;
using System.Collections.Generic;
using UnityEngine;
namespace Sasha19.Pool.MonoBeh
{


    public partial class PoolMonoBehBase<T> : IDisposable where T : class
    {
        private readonly Stack<T> _pool = new();
        private readonly HashSet<T> _inPoolTracker = new();

        protected Transform PoolRoot { get; private set; }

        public void InitializePoolGameObject()
        {
            var go = new GameObject($"[{typeof(T).Name}_Pool_Root]");
            go.SetActive(false);
            PoolRoot = go.transform;
        }
        protected T GetFromPool()
        {
            if (_pool.Count > 0)
            {
                var element = _pool.Pop();
                _inPoolTracker.Remove(element);

                if (element is not MonoBehaviour mono || mono == null)
                {
                    return null;
                }

                return element;
            }
            return null;
        }
        protected bool TryReleaseToPool(T element)
        {
            if (element == null || _inPoolTracker.Contains(element))
                return false;

            if (element is IReleasable releasable)
            {
                releasable.ResetForPool();
            }

            _pool.Push(element);
            _inPoolTracker.Add(element);
            return true;
        }
        public virtual void Dispose()
        {
            _inPoolTracker.Clear();
            while (_pool.Count > 0)
            {
                var element = _pool.Pop();
                if (element is MonoBehaviour mono && mono != null)
                {
                    GameObject.Destroy(mono.gameObject);
                }
            }

            if (PoolRoot != null)
                GameObject.Destroy(PoolRoot.gameObject);
        }
    }
}