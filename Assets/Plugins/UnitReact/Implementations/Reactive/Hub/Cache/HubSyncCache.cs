using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
    internal static class HubSyncCache<TContainer> where TContainer : class
    {
        public static readonly List<Action<TContainer>> Handlers = new();

        public static void Add(Action<TContainer> handler)
        {
            if (!Handlers.Contains(handler))
                Handlers.Add(handler);
        }

        public static void Remove(Action<TContainer> handler)
        {
            Handlers.Remove(handler);
        }
    }
}
