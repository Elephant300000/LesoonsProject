using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Lucky38.UnitReact.Core
{
    public static class FastEventHubAsync
    {
        public static void ConfigureGate<TContainer>(float cooldownSeconds, bool skipIfRunning) 
            where TContainer : class, IPoolableEvent, new()
        {
            HubAsyncGateManager.Configure<TContainer>(cooldownSeconds, skipIfRunning);
        }

        public static IDisposable Subscribe<TContainer>(Func<TContainer, CancellationToken, UniTask> handler)
            where TContainer : class, IPoolableEvent, new()
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            
            HubSignatureValidator.RegisterSignature(typeof(TContainer), typeof(Func<TContainer, CancellationToken, UniTask>));
            HubAsyncCache<TContainer>.Add(handler);
            return Subscription.Create(() => HubAsyncCache<TContainer>.Remove(handler));
        }

        public static async UniTask PublishAsync<TContainer>(Action<TContainer> setup = null) 
            where TContainer : class, IPoolableEvent, new()
        {
            HubSignatureValidator.Validate(typeof(TContainer), typeof(Func<TContainer, CancellationToken, UniTask>));

            if (!HubAsyncGateManager.TryPass<TContainer>(out var gate, out var token)) 
                return;

            var evt = EventPool<TContainer>.Get();
            setup?.Invoke(evt);

            try
            {
                var handlers = HubAsyncCache<TContainer>.Handlers;
                int count = handlers.Count;
                
                if (count == 1)
                {
                    await SafeInvokeAsync(handlers[0], evt, token);
                }
                else if (count > 1)
                {
                    var tasks = ListPool<UniTask>.Rent();
                    for (int i = count - 1; i >= 0; i--)
                    {
                        tasks.Add(SafeInvokeAsync(handlers[i], evt, token));
                    }
                    await UniTask.WhenAll(tasks);
                    ListPool<UniTask>.Return(tasks);
                }
            }
            finally
            {
                EventPool<TContainer>.Release(evt);
                gate?.Complete(); 
            }
        }

        private static async UniTask SafeInvokeAsync<TContainer>(
            Func<TContainer, CancellationToken, UniTask> handler,
            TContainer evt,
            CancellationToken token) where TContainer : class
        {
            try
            {
                await handler.Invoke(evt, token);
            }
            catch (OperationCanceledException)
            {
                // �������� ��� ������ ����� ����� ��� Gate
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }
    }
}