using UnityEngine;

namespace Lucky38.MyCamera
{
    /// <summary>
    /// Camera wall collision only: raycast and shorten orbit zoom.
    /// </summary>
    public sealed class CinemaCameraCollision : MonoBehaviour, ICinemaCameraCollision
    {
        [Header("Collision")]
        [Tooltip("Obstacle layers (player layer should be excluded).")]
        [SerializeField] private LayerMask collisionLayers;

        [Tooltip("Padding from walls to avoid clipping.")]
        [Range(0.2f, 1f)]
        [SerializeField] private float collisionPadding = 0.5f;

        public float CollisionPadding => collisionPadding;

        public bool TryResolveZoom(
            Vector3 lookOrigin,
            Vector3 rayDirection,
            float desiredZoom,
            float maxRayDistance,
            out float resolvedZoom)
        {
            if (Physics.Raycast(lookOrigin, rayDirection, out var hit, maxRayDistance, collisionLayers) &&
                hit.distance < desiredZoom + collisionPadding)
            {
                resolvedZoom = Mathf.Max(0.1f, hit.distance - collisionPadding);
                return true;
            }

            resolvedZoom = desiredZoom;
            return false;
        }
    }
}
