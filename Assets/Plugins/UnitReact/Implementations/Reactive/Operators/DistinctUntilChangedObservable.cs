using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
    internal class DistinctUntilChangedObservable<T, TKey> : ISubscribable<T>
    {
        private readonly ISubscribable<T> _source;
        private readonly Func<T, TKey> _keySelector;
        private readonly IEqualityComparer<TKey> _comparer;

        internal DistinctUntilChangedObservable(ISubscribable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            _source = source;
            _keySelector = keySelector;
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        public IDisposable Subscribe(Action<T> action)
        {
            bool isFirst = true;
            TKey prevKey = default;

            return _source.Subscribe(data =>
            {
                TKey currentKey = _keySelector(data);

                if (isFirst || !_comparer.Equals(prevKey, currentKey))
                {
                    isFirst = false;
                    prevKey = currentKey;

                    action(data);
                }
            });
        }
    }
}