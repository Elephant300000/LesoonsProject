using UnityEngine;

namespace Lucky38.MyCamera
{
    /// <summary>
    /// Owns zoom / rotate / move for the cinema orbit camera.
    /// </summary>
    public sealed class CinemaCameraControlService : ICinemaCameraControl
    {
        private ICinemaCameraRig _rig;
        private Transform _lookTarget;

        private float _mouseX;
        private float _mouseY;
        private Vector3 _rotationInput;

        private float _currentZoom = 5f;
        private float _targetZoom = 5f;
        private float _originZoom = 5f;
        private float _zoomVelocity;
        private bool _collisionHit;
        private bool _lookEnabled = true;

        public void Bind(ICinemaCameraRig rig)
        {
            _rig = rig;
            _lookTarget = rig?.LookTarget;

            var settings = rig?.Settings;
            var initialZoom = settings != null ? settings.initialZoom : 5f;
            _currentZoom = initialZoom;
            _targetZoom = initialZoom;
            _originZoom = initialZoom;
            _zoomVelocity = 0f;

            SyncFromTransform();
        }

        public void SetLookTarget(Transform lookTarget)
        {
            _lookTarget = lookTarget;
        }

        public void SetLookEnabled(bool enabled)
        {
            _lookEnabled = enabled;
            Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !enabled;
        }

        public void SyncFromTransform()
        {
            if (_rig?.CameraTransform == null)
            {
                return;
            }

            var settings = _rig.Settings;
            var cameraTransform = _rig.CameraTransform;
            var euler = cameraTransform.eulerAngles;

            _mouseX = euler.y;
            _mouseY = Mathf.Clamp(
                NormalizePitch(euler.x),
                settings.minPitchAngle,
                settings.maxPitchAngle);
            _rotationInput = new Vector3(_mouseY, _mouseX, 0f);

            if (_lookTarget == null)
            {
                return;
            }

            var distance = Vector3.Distance(_lookTarget.position, cameraTransform.position);
            var zoom = Mathf.Clamp(distance, settings.minDistance, settings.maxDistance);
            _currentZoom = zoom;
            _targetZoom = zoom;
            _originZoom = zoom;
            _zoomVelocity = 0f;
            _collisionHit = false;
        }

        public void TickInput()
        {
            if (_rig?.CameraTransform == null)
            {
                return;
            }

            if (_lookEnabled)
            {
                TickRotationInput();
                TickZoomInput();
            }

            // Ray direction is derived in ResolveCollision / TickMotion from current rotation.
        }

        public void TickMotion(float deltaTime)
        {
            if (!_lookEnabled || _rig?.CameraTransform == null || _lookTarget == null)
            {
                return;
            }

            var settings = _rig.Settings;
            var cameraTransform = _rig.CameraTransform;

            var targetRotation = Quaternion.Euler(_rotationInput);
            cameraTransform.rotation = Quaternion.Slerp(
                cameraTransform.rotation,
                targetRotation,
                deltaTime * settings.rotationLerpSpeed);

            _currentZoom = Mathf.SmoothDamp(_currentZoom, _targetZoom, ref _zoomVelocity, 0.1f);
            cameraTransform.position = _lookTarget.position - cameraTransform.forward * _currentZoom;
        }

        public void ResolveCollision(ICinemaCameraCollision collision)
        {
            if (!_lookEnabled || _rig?.CameraTransform == null || _lookTarget == null || collision == null)
            {
                return;
            }

            var settings = _rig.Settings;
            var cameraTransform = _rig.CameraTransform;
            var rayDirection = cameraTransform.rotation * Vector3.back;
            var maxRayDistance = settings.maxDistance + _rig.CollisionPadding;

            if (collision.TryResolveZoom(
                    _lookTarget.position,
                    rayDirection,
                    _originZoom,
                    maxRayDistance,
                    out var resolvedZoom))
            {
                _targetZoom = resolvedZoom;
                _collisionHit = true;
                return;
            }

            _targetZoom = _originZoom;
            _collisionHit = false;
        }

        public float CheckRotateAngle()
        {
            if (_rig?.CameraTransform == null || _lookTarget == null)
            {
                return 0f;
            }

            var cameraZ = Vector3.ProjectOnPlane(_rig.CameraTransform.forward, Vector3.up).normalized;
            var characterZ = Vector3.ProjectOnPlane(_lookTarget.forward, Vector3.up).normalized;
            return Vector3.SignedAngle(cameraZ, characterZ, Vector3.up);
        }

        private void TickRotationInput()
        {
            var settings = _rig.Settings;
            _mouseX = 0;//Input.GetAxis("Mouse X") * settings.mouseSensitivity;
            _mouseY = 0;//  Input.GetAxis("Mouse Y") * settings.mouseSensitivity;
            _mouseY = Mathf.Clamp(_mouseY, settings.minPitchAngle, settings.maxPitchAngle);
            _rotationInput = new Vector3(_mouseY, _mouseX, 0f);
        }

        private void TickZoomInput()
        {
            var settings = _rig.Settings;
            var scroll = 0;//Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                _originZoom -= scroll * settings.zoomSpeed;
                _originZoom = Mathf.Clamp(_originZoom, settings.minDistance, settings.maxDistance);
            }

            if (!_collisionHit)
            {
                _targetZoom = _originZoom;
            }
        }

        private static float NormalizePitch(float eulerX)
        {
            if (eulerX > 180f)
            {
                eulerX -= 360f;
            }

            return eulerX;
        }
    }
}
