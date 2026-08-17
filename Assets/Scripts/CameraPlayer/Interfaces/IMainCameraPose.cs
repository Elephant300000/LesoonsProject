using UnityEngine;

namespace Lucky38.MyCamera
{
    /// <summary>
    /// Durable camera pose for save/load — Transform get/set only.
    /// Orbit sync after load is handled by <see cref="ICinemaCameraControl.SyncFromTransform"/>.
    /// </summary>
    public interface IMainCameraPose
    {
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        void ApplyPose(Vector3 position, Quaternion rotation);
    }
}
