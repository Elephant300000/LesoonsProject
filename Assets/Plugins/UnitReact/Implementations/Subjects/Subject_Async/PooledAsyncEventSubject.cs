using Cysharp.Threading.Tasks;
using System;

namespace Lucky38.UnitReact.Core
{ 
    public class PooledAsyncEventSubject : AsyncEventSubject, IPooledAsyncEventSubject
    {
        public async UniTask InvokePooledActionAsync<TContainer>(Action<TContainer> fill)
             where TContainer : class, IPoolableEvent, new()
        {
            var container = EventPool<TContainer>.Get();
            try
            {
                fill?.Invoke(container);
                await InvokeActionAsync(container);
            }
            finally
            { 
                EventPool<TContainer>.Release(container);
            }
        }
    }
}
