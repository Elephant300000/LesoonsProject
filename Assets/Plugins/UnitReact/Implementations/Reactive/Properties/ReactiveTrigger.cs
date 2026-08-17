using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
    // Используется для ЛОКАЛЬНЫХ СИГНАЛОВ (Клики UI, выстрел). Не хранит значение.
    public class ReactiveTrigger : IReadOnlyReactiveTrigger
    {
        private readonly global::System.Collections.Generic.List<Action<Unit>> _observers = new();
        private SyncEventGate _gate;

        // Добавляем локальный Gate для защиты от дабл-кликов
        public ReactiveTrigger WithGate(float cooldownSeconds, bool skipIfRunning = true)
        {
            _gate = new SyncEventGate(cooldownSeconds, skipIfRunning);
            return this;
        }

        public void Invoke()
        {
            if (_gate != null && !_gate.TryEnter()) 
                return;

            try
            {
                for (int i = _observers.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        _observers[i](Unit.Default);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
            finally
            {
                _gate?.Exit();
            }
        }

        public IDisposable Subscribe(Action<Unit> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            
            _observers.Add(action);
            return Subscription.Create(() => _observers.Remove(action));
        }

        public void Clear() => _observers.Clear();
        public void Dispose() => Clear();
    }
}