using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Strategy: routes observer registration/subscribe to local Subject or global hub caches.
    /// </summary>
    public interface ISubscriptionRouter
    {
        void RegisterSync<TContainer>(Action<TContainer> handler) where TContainer : class;
        void RegisterAsync<TContainer>(Func<TContainer, CancellationToken, UniTask> handler) where TContainer : class;

        void SubscribeAll(IObserver observer);
        void SubscribeAllAsync(IObserver observer, CancellationToken replayToken);
        void UnsubscribeAll(IObserver observer);

        /// <summary>
        /// Local router only: handlers for React / ReactAsync. Global returns false.
        /// </summary>
        bool TryGetHandlers(Type eventType, out List<Delegate> handlers);
    }
}
