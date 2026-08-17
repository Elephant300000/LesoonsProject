using System;
using System.Threading;

namespace Lucky38.UnitReact.Core
{
    public abstract class AsyncReplayableSubject : ReplayableSubjectBase
    { 
        public void SubscribeAsync(Type eventType, IAsyncObserverHandler observer, CancellationToken replayToken)
        {
            if (!TryAddObserver(eventType, observer))
                return;

            OnAfterSubscribeAsync(eventType, observer, replayToken);
        }

        protected override void OnAfterSubscribeAsync(
            Type eventType,
            IAsyncObserverHandler observer,
            CancellationToken token)
        {
            if (signatures.TryGetValue(eventType, out var sig) &&
                _historyBuffers.TryGetValue(eventType, out var buffer))
            {
                buffer.ReplayAsync(observer, sig, token);
            }
        }
    }
}
