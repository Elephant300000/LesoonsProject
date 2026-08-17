using System;

namespace Lucky38.UnitReact.Core
{
    public static class EventBinding
    { 
        public static ISubscribable<T> FromEvent<T>(Action<Action<T>> add, Action<Action<T>> remove)
        {
            return new EventObservable<T>(add, remove);
        }  
        
        private class EventObservable<T> : ISubscribable<T>
        {
            private readonly Action<Action<T>> _add;
            private readonly Action<Action<T>> _remove;

            public EventObservable(Action<Action<T>> add, Action<Action<T>> remove)
            {
                _add = add;
                _remove = remove;
            }

            public IDisposable Subscribe(Action<T> action)
            {
                _add(action);
                var remove = _remove;
                return Subscription.Create(() => remove(action));
            }
        }
    }
}