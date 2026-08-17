using System;
using System.Threading;

namespace Lucky38.UnitReact.Core
{
    public interface IAsyncEventGate : IDisposable
    {
        bool TryEnter(out CancellationToken token);

        void Exit();
    }
}