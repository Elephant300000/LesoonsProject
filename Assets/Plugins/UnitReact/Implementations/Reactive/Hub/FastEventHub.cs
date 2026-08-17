using System;

namespace Lucky38.UnitReact.Core
{
    public static class FastEventHub
    {
        public static void RegisterSignature(Type eventType, Type delegateType)
        {
            HubSignatureValidator.RegisterSignature(eventType, delegateType);
        }
    }
}