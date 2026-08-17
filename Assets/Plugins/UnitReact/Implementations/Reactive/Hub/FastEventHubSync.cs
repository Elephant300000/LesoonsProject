using System;

namespace Lucky38.UnitReact.Core
{
    public static class FastEventHubSync
    {
        public static void ConfigureGate<TContainer>(float cooldownSeconds, bool skipIfRunning) 
            where TContainer : class, IPoolableEvent, new()
        {
            HubSyncGateManager.Configure<TContainer>(cooldownSeconds, skipIfRunning);
        }

        public static void Publish<TContainer>(Action<TContainer> setup = null) 
            where TContainer : class, IPoolableEvent, new()
        {
            HubSignatureValidator.Validate(typeof(TContainer), typeof(Action<TContainer>));

            if (!HubSyncGateManager.TryPass<TContainer>(out var gate)) 
                return;

            var evt = EventPool<TContainer>.Get();
            setup?.Invoke(evt);

            try
            {
                var handlers = HubSyncCache<TContainer>.Handlers;
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        handlers[i].Invoke(evt);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
            finally
            {
                EventPool<TContainer>.Release(evt);
                gate?.Exit(); 
            }
        }
    }
}