using UnityEngine;

namespace Lucky38.MyCamera
{
    /// <summary>
    /// Rig data for the control service: camera transform + settings + look target.
    /// </summary>
    public interface ICinemaCameraRig
    {
        Transform CameraTransform { get; }
        CinemaCameraControlSettings Settings { get; }
        Transform LookTarget { get; }
        float CollisionPadding { get; }
    }
}
