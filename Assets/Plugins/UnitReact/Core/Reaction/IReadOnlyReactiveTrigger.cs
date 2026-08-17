using System;

namespace Lucky38.UnitReact.Core
{
    public interface IReadOnlyReactiveTrigger : ISubscribable<Unit>, IDisposable
    { 
    }
}