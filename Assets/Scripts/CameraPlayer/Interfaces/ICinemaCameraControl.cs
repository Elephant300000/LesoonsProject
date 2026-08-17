using UnityEngine;

namespace Lucky38.MyCamera
{
    /// <summary>
    /// Orbit camera control: look input, scroll zoom, rotate/move application, orbit sync after load.
    /// </summary>
    public interface ICinemaCameraControl
    {
        void Bind(ICinemaCameraRig rig);
        void SetLookTarget(Transform lookTarget);
        void SetLookEnabled(bool enabled);
        void SyncFromTransform();
        void TickInput();
        void TickMotion(float deltaTime);
        void ResolveCollision(ICinemaCameraCollision collision);
        float CheckRotateAngle();
    }
}
