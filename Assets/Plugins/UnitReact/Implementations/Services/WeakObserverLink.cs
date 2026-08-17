using System;

namespace Lucky38.UnitReact.Core
{ 
    public sealed class WeakObserverLink
    {
        private readonly WeakReference<IObserver> _weak; 
        public WeakObserverLink(IObserver ctx) => _weak = new WeakReference<IObserver>(ctx);
         
        public bool TryGetTarget(out IObserver target) => _weak.TryGetTarget(out target);
 
        public bool IsDead => !_weak.TryGetTarget(out _);
    }
}
