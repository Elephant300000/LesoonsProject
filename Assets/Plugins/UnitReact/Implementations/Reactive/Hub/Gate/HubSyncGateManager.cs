using System;

namespace Lucky38.UnitReact.Core
{
    internal static class HubSyncGateManager
    {
        private static class Cache<TContainer> where TContainer : class, IPoolableEvent, new()
        {
            public static SyncEventGate Gate;
        }

        public static void Configure<TContainer>(float cooldownSeconds, bool skipIfRunning)
            where TContainer : class, IPoolableEvent, new()
        {
            Cache<TContainer>.Gate = new SyncEventGate(cooldownSeconds, skipIfRunning);
        }

        public static bool TryPass<TContainer>(out SyncEventGate gate)
            where TContainer : class, IPoolableEvent, new()
        {
            gate = Cache<TContainer>.Gate;
            if (gate != null)
                return gate.TryEnter();
            return true;
        }
    }
}
