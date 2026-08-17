using System.Threading;

namespace Lucky38.UnitReact.Core
{
    internal static class HubAsyncGateManager
    {
        private static class Cache<TContainer> where TContainer : class, IPoolableEvent, new()
        {
            public static HubAsyncGate Gate;
        }

        public static void Configure<TContainer>(float cooldownSeconds, bool skipIfRunning)
            where TContainer : class, IPoolableEvent, new()
        {
            Cache<TContainer>.Gate = new HubAsyncGate(cooldownSeconds, skipIfRunning);
        }

        public static bool TryPass<TContainer>(out HubAsyncGate gate, out CancellationToken token)
            where TContainer : class, IPoolableEvent, new()
        {
            token = CancellationToken.None;
            gate = Cache<TContainer>.Gate;
            if (gate != null)
                return gate.TryEnter(out token);
            return true;
        }
    }
}
