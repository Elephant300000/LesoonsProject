using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Lucky38.UnitReact.Core
{ 
    public class AsyncEventSubject : AsyncGatedSubject, IAsyncEventSubject
    {
        public async UniTask InvokeActionAsync<TContainer>(TContainer container) where TContainer : class
        {
            Type eventType = typeof(TContainer);
            Type signatureType = typeof(Func<TContainer, CancellationToken, UniTask>);
            Validate(eventType, signatureType);

            if (!TryEnterAsyncGate(eventType, out CancellationToken token))
                return;

            try
            {
                RecordHistory(container);
                var live = SnapshotAsyncObserver(eventType);
                var tasks = ListPool<UniTask>.Rent();
                try
                {
                    foreach (var o in live)
                        tasks.Add(SafeReactAsync(o, container, token));

                    await UniTask.WhenAll(tasks).AttachExternalCancellation(token);
                }
                finally
                {
                    ListPool<UniTask>.Return(tasks);
                    ListPool<IAsyncObserverHandler>.Return(live);
                }
            }
            finally
            {
                ExitAsyncGate(eventType);
            }
        }
        private static async UniTask SafeReactAsync<TContainer>(
            IAsyncObserverHandler observer,
            TContainer container,
            CancellationToken token) where TContainer : class
        {
            try
            {
                await observer.ReactAsync(container, token);
            }
            catch (OperationCanceledException)
            {
                // Expected when gate/token cancels.
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
