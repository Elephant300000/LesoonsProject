 
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lucky38.UnitReact.Core
{
    public static class ReactiveAsyncExtensions
    {
        // Поддержка UniTask. Позволяет вешать асинхронные методы на подписку.
        public static IDisposable SubscribeAsync<T>(this ISubscribable<T> source, Func<T, CancellationToken, UniTask> asyncAction, CancellationToken token = default)
        {
            return source.Subscribe(data => 
            {
                UniTask.Void(async () => 
                {
                    try
                    {
                        await asyncAction(data, token);
                    }
                    catch (OperationCanceledException)
                    {
                        // Игнорируем отмену
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                });
            });
        }
    }
}