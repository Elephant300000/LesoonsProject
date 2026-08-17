using System.Collections.Concurrent;

namespace Lucky38.UnitReact.Core
{ 
    public static class EventPool<TContainer> where TContainer : class, IPoolableEvent, new()
    { 
        static readonly ConcurrentBag<TContainer> Bag = new();
 
        public static TContainer Get() => Bag.TryTake(out var e) ? e : new TContainer();
    
        public static void Release(TContainer e) { e.Release(); Bag.Add(e); }
    }
}
