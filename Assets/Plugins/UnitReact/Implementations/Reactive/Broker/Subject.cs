using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
    public class Subject<T> : ISubscribable<T>, IDisposable
    {
        private readonly List<Action<T>> _observers = new();

        public void Publish(T eventData)
        {
            for (int i = _observers.Count - 1; i >= 0; i--)
            {
                try
                {
                    _observers[i](eventData);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }

        public IDisposable Subscribe(Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            _observers.Add(action);
             
            return Subscription.Create(() => _observers.Remove(action));
        }

        public void Clear() => _observers.Clear();
        public void Dispose() => Clear();
    }
}