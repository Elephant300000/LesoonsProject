using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
    public class ReactiveProperty<T> : IReadOnlyReactiveProperty<T>
    {
        private T _value;
        private readonly List<Action<T>> _observers = new();

        public ReactiveProperty(T initialValue = default) => _value = initialValue;

        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                _value = value;
                
                for (int i = _observers.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        _observers[i](_value);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
        }

        public IDisposable Subscribe(Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            
            try
            {
                action(_value);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
            
            _observers.Add(action);
            
            return Subscription.Create(() => _observers.Remove(action));
        }

        public void Dispose() => _observers.Clear();
        public static implicit operator T(ReactiveProperty<T> property) => property.Value;
    }
}