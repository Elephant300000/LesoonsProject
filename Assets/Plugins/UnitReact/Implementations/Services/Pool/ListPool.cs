using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
     
    static class ListPool<T>
    {
        static readonly ConcurrentBag<List<T>> Bag = new(); 
        public static List<T> Rent() => Bag.TryTake(out var l) ? l : new List<T>(); 
        public static void Return(List<T> l) { l.Clear(); Bag.Add(l); }
    }
}