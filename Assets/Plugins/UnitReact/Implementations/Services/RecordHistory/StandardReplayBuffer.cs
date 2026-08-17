using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace Lucky38.UnitReact.Core
{
    internal class StandardReplayBuffer<TContainer> : IReplayableBuffer<TContainer> where TContainer : class
    {
        protected readonly Queue<TContainer> _queue = new();
        protected readonly int _capacity;

        public StandardReplayBuffer(int capacity) => _capacity = Math.Max(0, capacity);

        public virtual void Add(TContainer item)
        {
            if (_capacity <= 0 || item == null) return;

            if (_queue.Count >= _capacity)
                OnDiscard(_queue.Dequeue());

            _queue.Enqueue(Capture(item));
        }
         
        protected virtual TContainer Capture(TContainer item)
        {
            if (item is ICloneableEvent<TContainer> cloneable)
                return cloneable.Clone();

            return item;
        }

        protected virtual void OnDiscard(TContainer item)
        {
            if (item is IDisposable disposable)
                disposable.Dispose();
        }

        public void ReplaySync(ISyncObserverHandler observer, Type signature)
        {
            if (signature != typeof(Action<TContainer>)) return;

            foreach (var item in _queue)
                observer.React(item);
        }

        public void ReplayAsync(IAsyncObserverHandler observer, Type signature, CancellationToken token)
        {
            if (signature != typeof(Func<TContainer, CancellationToken, UniTask>)) return;

            foreach (var item in _queue)
            { 
                observer.ReactAsync(item, token).Forget();
            }
        }

        public virtual void Clear()
        {
            while (_queue.Count > 0)
                OnDiscard(_queue.Dequeue());
        }
    }
}
