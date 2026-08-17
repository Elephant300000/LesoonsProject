using System;

namespace Lucky38.UnitReact.Core
{
    public abstract class SyncReplayableSubject : ReplayableSubjectBase
    {
        protected override void OnAfterSubscribe(Type eventType, ISyncObserverHandler observer)
        {
            if (signatures.TryGetValue(eventType, out var sig) &&
                _historyBuffers.TryGetValue(eventType, out var buffer))
            {
                buffer.ReplaySync(observer, sig);
            }
        }
    }
}
