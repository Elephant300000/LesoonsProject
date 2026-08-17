using System;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Hybrid observer facade: local Subject or global FastEventHub via <see cref="ISubscriptionRouter"/>.
    /// </summary>
    public abstract class ObserverContextBase : IObserver
    {
        protected readonly ISubscriptionRouter router;
        protected readonly IActionSignatureRegistry registry;

        private readonly CompositeDisposable _externalSubscriptions = new();

        protected ObserverContextBase()
        {
            router = new GlobalEventRouter();
        }

        protected ObserverContextBase(IEventSubjectCore subject, IActionSignatureRegistry registry)
        {
            this.registry = registry;
            router = subject != null
                ? new LocalEventRouter(subject)
                : new GlobalEventRouter();
        }

        public IDisposable SubscribeExternal<TDelegate>(
            Action<TDelegate> addHandler,
            Action<TDelegate> removeHandler,
            TDelegate handler)
            where TDelegate : Delegate
        {
            if (addHandler == null) throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null) throw new ArgumentNullException(nameof(removeHandler));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            try
            {
                addHandler(handler);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }

            var subscription = Subscription.Create(() =>
            {
                try
                {
                    removeHandler(handler);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            });

            _externalSubscriptions.Add(subscription);
            return subscription;
        }

        public void SubscribeAll() => router.SubscribeAll(this);

        public void UnsubscribeAll()
        {
            router.UnsubscribeAll(this);
            _externalSubscriptions.Dispose();
        }
    }
}
