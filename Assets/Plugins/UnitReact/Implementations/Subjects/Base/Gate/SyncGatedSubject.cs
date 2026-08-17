using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{ 
    public abstract class SyncGatedSubject : SyncReplayableSubject
    {
        private readonly Dictionary<Type, ISyncEventGate> _gates = new();

        public void ConfigureGate<TContainer>(float cooldownSeconds, bool skipIfRunning = false)
            where TContainer : class
        {
            _gates[typeof(TContainer)] = new SyncEventGate(cooldownSeconds, skipIfRunning);
        }

        protected bool TryEnterGate(Type eventType)
        {
            if (_gates.TryGetValue(eventType, out var gate))
                return gate.TryEnter();

            return true;
        }

        protected void ExitGate(Type eventType)
        {
            if (_gates.TryGetValue(eventType, out var gate))
                gate.Exit();
        }
    }
}
