using System;
using System.Collections.Generic;
using System.Threading;

namespace Lucky38.UnitReact.Core
{ 
    public abstract class AsyncGatedSubject : AsyncReplayableSubject
    {
        private readonly Dictionary<Type, IAsyncEventGate> _asyncGates = new();

        public void ConfigureAsyncGate<TContainer>(float cooldownSeconds, bool skipIfRunning)
            where TContainer : class
        {
            if (_asyncGates.TryGetValue(typeof(TContainer), out var oldGate))
                oldGate.Dispose();

            _asyncGates[typeof(TContainer)] = new AsyncEventGate(cooldownSeconds, skipIfRunning);
        }

        protected bool TryEnterAsyncGate(Type eventType, out CancellationToken token)
        {
            if (_asyncGates.TryGetValue(eventType, out IAsyncEventGate gate))
                return gate.TryEnter(out token);

            token = CancellationToken.None;
            return true;
        }

        protected void ExitAsyncGate(Type eventType)
        {
            if (_asyncGates.TryGetValue(eventType, out var gate))
                gate.Exit();
        }
    }
}
