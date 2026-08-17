using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
     
    public class ValueCache<TValue> : IValueCache<TValue>
    {
         
        public ValueCache(IEqualityComparer<TValue> comparer = null)
        {
            _comparer = comparer ?? EqualityComparer<TValue>.Default;
        }
        // Храним значения по типу события.
        private readonly Dictionary<Type, TValue> _values = new();
        readonly IEqualityComparer<TValue> _comparer;

        public bool HasChanged(Type eventType, TValue newValue)
        {
            if (_values.TryGetValue(eventType, out var oldValue))
            {
                if (_comparer.Equals(oldValue, newValue))
                    return false;
            }

            _values[eventType] = newValue;
            return true;
        }
        public TValue Get(Type eventType)
        {
            return _values.TryGetValue(eventType, out var val)
                ? val
                : default;
        }
        public void Clear(Type eventType)
        {
            _values.Remove(eventType);
        }
    }
}