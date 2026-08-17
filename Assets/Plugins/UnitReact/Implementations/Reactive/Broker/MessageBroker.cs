using System;

namespace Lucky38.UnitReact.Core
{
    public static class MessageBroker
    { 
        private static class Cache<T>
        {
            public static readonly Subject<T> Instance = new Subject<T>();
        }
         
        public static void Publish<T>(T message) => Cache<T>.Instance.Publish(message);
        
        public static ISubscribable<T> Receive<T>() => Cache<T>.Instance;
         
        public static void ClearAll<T>() => Cache<T>.Instance.Clear();
    }
}