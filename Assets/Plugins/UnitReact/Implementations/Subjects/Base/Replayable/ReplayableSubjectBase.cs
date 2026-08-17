using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
    public abstract class ReplayableSubjectBase : ValidatedSubject
    {
        protected readonly Dictionary<Type, IReplayableBuffer> _historyBuffers = new();
         
        public void EnableReplay<TContainer>(int capacity = 1) where TContainer : class
        {
            ReplaceBuffer(typeof(TContainer), new StandardReplayBuffer<TContainer>(capacity));
        }

        public void EnablePooledReplay<TContainer>(int capacity = 1)
            where TContainer : class, IPoolableEvent, new()
        {
            ReplaceBuffer(typeof(TContainer), new PooledReplayBuffer<TContainer>(capacity));
        }

        protected void RecordHistory<TContainer>(TContainer container) where TContainer : class
        {
            if (_historyBuffers.TryGetValue(typeof(TContainer), out var buffer) &&
                buffer is IReplayableBuffer<TContainer> bufferT)
            {
                bufferT.Add(container);
            }
        }

        private void ReplaceBuffer(Type eventType, IReplayableBuffer buffer)
        {
            if (_historyBuffers.TryGetValue(eventType, out var old))
                old.Clear();

            _historyBuffers[eventType] = buffer;
        }
    }
}
