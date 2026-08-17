using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Subscribes the observer to a local <see cref="IEventSubjectCore"/>.
    /// Handlers stay in multiMap and are invoked via React / ReactAsync.
    /// </summary>
    public sealed class LocalEventRouter : ISubscriptionRouter
    {
        private readonly Dictionary<Type, List<Delegate>> _multiMap = new();
        private readonly IEventSubjectCore _subject;

        public LocalEventRouter(IEventSubjectCore subject)
        {
            _subject = subject ?? throw new ArgumentNullException(nameof(subject));
        }

        public void RegisterSync<TContainer>(Action<TContainer> handler) where TContainer : class
        {
            GetOrAdd(typeof(TContainer)).Add(handler);
        }

        public void RegisterAsync<TContainer>(Func<TContainer, CancellationToken, UniTask> handler) where TContainer : class
        {
            GetOrAdd(typeof(TContainer)).Add(handler);
        }

        public void SubscribeAll(IObserver observer)
        {
            foreach (var kv in _multiMap)
            {
                try
                {
                    _subject.Subscribe(kv.Key, observer);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }

        public void SubscribeAllAsync(IObserver observer, CancellationToken replayToken)
        {
            if (_subject is AsyncReplayableSubject asyncSubject)
            {
                foreach (var kv in _multiMap)
                {
                    try
                    {
                        if(observer is IAsyncObserverHandler asyncObserver)
                        asyncSubject.SubscribeAsync(kv.Key, asyncObserver, replayToken);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
                return;
            }

            SubscribeAll(observer);
        }

        public void UnsubscribeAll(IObserver observer)
        {
            foreach (var kv in _multiMap)
            {
                try
                {
                    _subject.Unsubscribe(kv.Key, observer);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }

        public bool TryGetHandlers(Type eventType, out List<Delegate> handlers)
        {
            return _multiMap.TryGetValue(eventType, out handlers);
        }

        private List<Delegate> GetOrAdd(Type type)
        {
            if (!_multiMap.TryGetValue(type, out var list))
            {
                list = new List<Delegate>();
                _multiMap[type] = list;
            }   
            return list;
        }
    }
}
