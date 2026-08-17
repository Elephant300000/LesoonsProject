using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
    public abstract class ValidatedSubject : EventSubjectBase, IActionSignatureRegistry
    {
        protected readonly Dictionary<Type, Type> signatures = new();

        public void RegisterSignature(Type eventType, Type delegateType)
        {
            if (signatures.TryGetValue(eventType, out var existing) && existing != delegateType)
                throw new InvalidOperationException($"Signature mismatch for {eventType.Name}");

            signatures[eventType] = delegateType;
        }

        public void Validate(Type eventType, Type delegateType)
        {
            if (!signatures.TryGetValue(eventType, out var expected))
            {
                signatures[eventType] = delegateType;
            }
            else if (expected != delegateType)
            {
                throw new InvalidOperationException(
                    $"Invoke signature mismatch for {eventType.Name}. Expected {expected}, got {delegateType}");
            }
        }
    }
}
