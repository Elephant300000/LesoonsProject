using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Subscribes handlers directly into HubSyncCache / HubAsyncCache (FastEventHub).
    /// No multiMap / React path — hub invokes delegates itself.
    /// </summary>
    public sealed class GlobalEventRouter : ISubscriptionRouter
    {
        private readonly List<Action> _subscribers = new();
        private readonly List<Action> _unsubscribers = new();

        public void RegisterSync<TContainer>(Action<TContainer> handler) where TContainer : class
        {
            _subscribers.Add(() => HubSyncCache<TContainer>.Add(handler));
            _unsubscribers.Add(() => HubSyncCache<TContainer>.Remove(handler));
        }

        public void RegisterAsync<TContainer>(Func<TContainer, CancellationToken, UniTask> handler) where TContainer : class
        {
            _subscribers.Add(() => HubAsyncCache<TContainer>.Add(handler));
            _unsubscribers.Add(() => HubAsyncCache<TContainer>.Remove(handler));
        }

        public void SubscribeAll(IObserver observer)
        {
            for (int i = 0; i < _subscribers.Count; i++)
            {
                try
                {
                    _subscribers[i].Invoke();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }

        public void SubscribeAllAsync(IObserver observer, CancellationToken replayToken)
        {
            // Global hub has no replay buffer — same as SubscribeAll.
            SubscribeAll(observer);
        }

        public void UnsubscribeAll(IObserver observer)
        {
            for (int i = 0; i < _unsubscribers.Count; i++)
            {
                try
                {
                    _unsubscribers[i].Invoke();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }

        public bool TryGetHandlers(Type eventType, out List<Delegate> handlers)
        {
            handlers = null;
            return false;
        }
    }
}
