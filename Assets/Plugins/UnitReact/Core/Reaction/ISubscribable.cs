using System;

namespace Lucky38.UnitReact.Core
{
    public interface ISubscribable<out T>
    {
        IDisposable Subscribe(Action<T> action);
    }
     
   
}