using System;

namespace Lucky38.UnitReact.Core
{
    public static class FastEventHubRx
    { 
        public static ISubscribable<TContainer> Receive<TContainer>() 
            where TContainer : class, IPoolableEvent, new()
        {
            return new HubObservable<TContainer>();
        }

        private class HubObservable<TContainer> : ISubscribable<TContainer>
            where TContainer : class, IPoolableEvent, new()
        {
            public IDisposable Subscribe(Action<TContainer> action)
            {
                if (action == null) throw new ArgumentNullException(nameof(action));
                HubSyncCache<TContainer>.Add(action);
                return Subscription.Create(() => HubSyncCache<TContainer>.Remove(action));
            }
        }
    }
}