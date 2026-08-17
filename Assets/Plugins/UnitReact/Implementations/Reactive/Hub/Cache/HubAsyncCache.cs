using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lucky38.UnitReact.Core
{
    internal static class HubAsyncCache<TContainer> where TContainer : class
    {
        public static readonly List<Func<TContainer, CancellationToken, UniTask>> Handlers = new();

        public static void Add(Func<TContainer, CancellationToken, UniTask> handler)
        {
            if (!Handlers.Contains(handler))
                Handlers.Add(handler);
        }

        public static void Remove(Func<TContainer, CancellationToken, UniTask> handler)
        {
            Handlers.Remove(handler);
        }
    }
}
