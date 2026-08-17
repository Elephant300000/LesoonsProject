using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lucky38.UnitReact.Core
{
    public abstract class AsyncObserverContext : ObserverContextBase, IAsyncObserverHandler, IAsyncObserverRegistrar
    {
        protected AsyncObserverContext() : base()
        {
        }

        public AsyncObserverContext(IEventSubjectCore subject, IActionSignatureRegistry registry)
            : base(subject, registry)
        {
        }

        public void SubscribeAllAsync(CancellationToken replayToken)
        {
            router.SubscribeAllAsync(this, replayToken);
        }

        public void RegisterAsync<TContainer>(Func<TContainer, CancellationToken, UniTask> handler)
            where TContainer : class
        {
            var type = typeof(TContainer);
            if (registry != null)
                registry.RegisterSignature(type, typeof(Func<TContainer, CancellationToken, UniTask>));
            else
                HubSignatureValidator.RegisterSignature(type, typeof(Func<TContainer, CancellationToken, UniTask>));

            router.RegisterAsync(handler);
        }

        public async UniTask ReactAsync<TContainer>(TContainer evt, CancellationToken token) where TContainer : class
        {
            var type = typeof(TContainer);
            if (!router.TryGetHandlers(type, out var delegates))
                throw new InvalidOperationException($"No async handler registered for {type.Name}");

            for (int i = 0; i < delegates.Count; i++)
            {
                if (delegates[i] is not Func<TContainer, CancellationToken, UniTask> taskHandler)
                    continue;

                try
                {
                    await taskHandler.Invoke(evt, token);
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }
    }
}
