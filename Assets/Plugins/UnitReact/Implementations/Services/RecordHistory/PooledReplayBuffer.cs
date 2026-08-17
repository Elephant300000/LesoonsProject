namespace Lucky38.UnitReact.Core
{ 
    internal class PooledReplayBuffer<TContainer> : StandardReplayBuffer<TContainer>
        where TContainer : class, IPoolableEvent, new()
    {
        public PooledReplayBuffer(int capacity) : base(capacity) { }

        protected override TContainer Capture(TContainer item)
        {
            return EventCloner<TContainer>.Clone(item);
        }

        protected override void OnDiscard(TContainer item)
        {
            if (item == null) return;
            EventPool<TContainer>.Release(item);
        }
    }
}
