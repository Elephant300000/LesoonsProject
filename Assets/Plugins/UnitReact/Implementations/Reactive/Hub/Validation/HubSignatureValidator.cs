using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
    internal static class HubSignatureValidator
    {
        private static readonly Dictionary<Type, Type> _signatures = new();

        public static void RegisterSignature(Type eventType, Type delegateType)
        {
            if (_signatures.TryGetValue(eventType, out var existing) && existing != delegateType)
                throw new InvalidOperationException($"Global signature mismatch for {eventType.Name}");
            _signatures[eventType] = delegateType;
        }

        public static void Validate(Type eventType, Type delegateType)
        {
            if (!_signatures.TryGetValue(eventType, out var expected))
                _signatures[eventType] = delegateType;
            else if (expected != delegateType)
                throw new InvalidOperationException($"Global invoke signature mismatch for {eventType.Name}. Expected {expected}, got {delegateType}");
        }
    }
}