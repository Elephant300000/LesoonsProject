using System;
using UnityEngine;

namespace Lucky38.UnitReact.Core
{ 
    public class SyncEventSubject : SyncGatedSubject, ISyncEventSubject
    {
        public void InvokeAction<TContainer>(TContainer container) where TContainer : class
        {
            Validate(typeof(TContainer), typeof(Action<TContainer>));

            if (!TryEnterGate(typeof(TContainer)))
                return;

            try
            {
                RecordHistory(container);
                var live = SnapshotSyncObserver(typeof(TContainer));
                try
                {
                    foreach (var o in live)
                    {
                        try
                        {
                            o.React(container);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }
                finally
                {
                    ListPool<ISyncObserverHandler>.Return(live);
                }
            }
            finally
            {
                ExitGate(typeof(TContainer));
            }
        }
    }
}
