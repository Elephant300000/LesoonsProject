using UnityEngine;

namespace Lucky38.MyCamera
{
    /// <summary>
    /// Save/load pose only: reads/writes the camera Transform. No orbit/collision logic.
    /// </summary>
    public sealed class MainCameraTransformPose : IMainCameraPose
    {
        public MainCameraTransformPose(ICinemaCameraRig rig)
        {
            _cameraTransform = rig != null ? rig.CameraTransform : null;
        }

        private readonly Transform _cameraTransform;

        public Vector3 Position =>
            _cameraTransform != null ? _cameraTransform.position : Vector3.zero;

        public Quaternion Rotation =>
            _cameraTransform != null ? _cameraTransform.rotation : Quaternion.identity;

        public void ApplyPose(Vector3 position, Quaternion rotation)
        {
            if (_cameraTransform == null)
            {
                return;
            }

            _cameraTransform.SetPositionAndRotation(position, rotation);
        }
    }
}
