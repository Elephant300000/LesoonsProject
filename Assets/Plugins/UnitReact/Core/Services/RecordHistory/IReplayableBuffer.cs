using System;
using System.Threading;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Нетипизированный интерфейс для вызова истории опоздавшим подписчикам.
    /// </summary>
    public interface IReplayableBuffer
    {
        void Clear();
        void ReplaySync(ISyncObserverHandler observer, Type signature);
        void ReplayAsync(IAsyncObserverHandler observer, Type signature, CancellationToken token);
    }
    internal interface IReplayableBuffer<TContainer> : IReplayableBuffer where TContainer : class
    {
        void Add(TContainer item);
    }
}