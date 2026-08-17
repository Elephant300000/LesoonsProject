using UnityEngine;

namespace Lucky38.MyCamera
{
    /// <summary>
    /// Resolves orbit zoom against world obstacles.
    /// </summary>
    public interface ICinemaCameraCollision
    {
        bool TryResolveZoom(
            Vector3 lookOrigin,
            Vector3 rayDirection,
            float desiredZoom,
            float maxRayDistance,
            out float resolvedZoom);
    }
}
