using System;
using System.Collections.Generic;
using System.Threading;

namespace Lucky38.UnitReact.Core
{ 
    public abstract class EventSubjectBase : IEventSubjectCore
    {
        protected readonly Dictionary<Type, List<WeakObserverLink>> observers = new();

        public virtual void Subscribe(Type eventType, IObserver observer)
        {
            if (!TryAddObserver(eventType, observer))
                return;

            if (observer is ISyncObserverHandler syncObserver)
                OnAfterSubscribe(eventType, syncObserver);

            if (observer is IAsyncObserverHandler asyncObserver)
                OnAfterSubscribeAsync(eventType, asyncObserver, CancellationTokenForReplay);
        }
         
        protected virtual CancellationToken CancellationTokenForReplay => CancellationToken.None;
         
        protected bool TryAddObserver(Type eventType, IObserver observer)
        {
            if (!observers.TryGetValue(eventType, out var list))
                observers[eventType] = list = new List<WeakObserverLink>();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].IsDead)
                    list.RemoveAt(i);
                else if (list[i].TryGetTarget(out var o) && ReferenceEquals(o, observer))
                    return false;
            }

            list.Add(new WeakObserverLink(observer));
            return true;
        }

        protected virtual void OnAfterSubscribe(Type eventType, ISyncObserverHandler observer) { }

        protected virtual void OnAfterSubscribeAsync(
            Type eventType,
            IAsyncObserverHandler observer,
            CancellationToken token) { }

        public virtual void Unsubscribe(Type eventType, IObserver observer)
        {
            if (!observers.TryGetValue(eventType, out var list)) return;
            list.RemoveAll(l => l.IsDead || (l.TryGetTarget(out var o) && ReferenceEquals(o, observer)));
            if (list.Count == 0) observers.Remove(eventType);
        }

        public List<ISyncObserverHandler> SnapshotSyncObserver(Type t)
        {
            var result = ListPool<ISyncObserverHandler>.Rent();
            if (!observers.TryGetValue(t, out var list)) return result;
             
            for (int i = 0; i < list.Count;)
            {
                if (!list[i].TryGetTarget(out var o))
                {
                    list.RemoveAt(i);
                    continue;
                }

                if (o is ISyncObserverHandler syncObserver)
                    result.Add(syncObserver);

                i++;
            }

            if (list.Count == 0) observers.Remove(t);
            return result;
        }

        public List<IAsyncObserverHandler> SnapshotAsyncObserver(Type t)
        {
            var result = ListPool<IAsyncObserverHandler>.Rent();
            if (!observers.TryGetValue(t, out var list)) return result;

            for (int i = 0; i < list.Count;)
            {
                if (!list[i].TryGetTarget(out var o))
                {
                    list.RemoveAt(i);
                    continue;
                }

                if (o is IAsyncObserverHandler asyncObserver)
                    result.Add(asyncObserver);

                i++;
            }

            if (list.Count == 0) observers.Remove(t);
            return result;
        }
    }
}
