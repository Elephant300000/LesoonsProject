using System;

namespace Lucky38.UnitReact.Core
{
    public abstract class SyncObserverContext : ObserverContextBase, ISyncObserverHandler, ISyncObserverRegistrar
    {
        protected SyncObserverContext() : base()
        {
        }

        protected SyncObserverContext(IEventSubjectCore subject, IActionSignatureRegistry registry)
            : base(subject, registry)
        {
        }

        public void Register<TContainer>(Action<TContainer> handler) where TContainer : class
        {
            var type = typeof(TContainer);
            if (registry != null)
                registry.RegisterSignature(type, typeof(Action<TContainer>));
            else
                HubSignatureValidator.RegisterSignature(type, typeof(Action<TContainer>));

            router.RegisterSync(handler);
        }

        public void React<TContainer>(TContainer container) where TContainer : class
        {
            var type = typeof(TContainer);
            if (!router.TryGetHandlers(type, out var delegates))
                throw new InvalidOperationException($"No Action<{type.Name}> handler registered.");

            for (int i = 0; i < delegates.Count; i++)
            {
                if (delegates[i] is not Action<TContainer> action)
                    continue;

                try
                {
                    action.Invoke(container);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }
    }
}
