using System;

namespace Lucky38.UnitReact.Core
{ 
    public class PooledSyncEventSubject : SyncEventSubject, IPooledSyncEventSubject
    {
        public void InvokePooledAction<TContainer>(Action<TContainer> fill)
            where TContainer : class, IPoolableEvent, new()
        {
            var container = EventPool<TContainer>.Get();
            try
            {
                fill?.Invoke(container);
                InvokeAction(container);
            }
            finally
            {
                EventPool<TContainer>.Release(container);
            }
        }
    }
}
