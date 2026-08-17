using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
    public class CompositeDisposable : IDisposable
    {
        private readonly List<IDisposable> _disposables = new();
        public void Add(IDisposable d) => _disposables.Add(d);
        public void Dispose()
        {
            foreach (var d in _disposables)
            {
                if (d != null)
                {
                    try
                    {
                        d.Dispose();
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
            _disposables.Clear();
        }
    }
}