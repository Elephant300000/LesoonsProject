using UnityEngine;
using Zenject;

namespace Lucky38.MyCamera
{
    /// <summary>
    /// Thin scene facade: ticks control + collision. Serialized fields keep Unity inspector values.
    /// </summary>
    [RequireComponent(typeof(CinemaCameraCollision))]
    public sealed class CinemaCameraDriver : MonoBehaviour, IPlayerCinemaCamera, ICinemaCameraRig
    {
        [Header("References")]
        [Tooltip("Transform the camera follows (head / character center).")]
        [SerializeField] private Transform lookTarget;

        [Header("Rotation")]
        [Tooltip("Mouse look sensitivity.")]
        [Range(1f, 10f)]
        [SerializeField] private float mouseSensitivity = 5f;

        [Tooltip("Camera rotation slerp speed.")]
        [Range(1f, 10f)]
        [SerializeField] private float rotationLerpSpeed = 9f;

        [Tooltip("Min pitch (look down).")]
        [Range(-90f, 0f)]
        [SerializeField] private float minPitchAngle = -70f;

        [Tooltip("Max pitch (look up).")]
        [Range(0f, 90f)]
        [SerializeField] private float maxPitchAngle = 70f;

        [Header("Zoom")]
        [Tooltip("Scroll zoom speed.")]
        [Range(1f, 10f)]
        [SerializeField] private float zoomSpeed = 5f;

        [Tooltip("Min camera distance.")]
        [Range(0.5f, 2f)]
        [SerializeField] private float minDistance = 1.5f;

        [Tooltip("Max camera distance.")]
        [Range(2f, 10f)]
        [SerializeField] private float maxDistance = 10f;

        private readonly CinemaCameraControlSettings _settings = new CinemaCameraControlSettings();

        private ICinemaCameraControl _control;
        private ICinemaCameraCollision _collision;
        private CinemaCameraCollision _collisionComponent;

        public Transform CameraTransform => transform;

        public CinemaCameraControlSettings Settings
        {
            get
            {
                _settings.mouseSensitivity = mouseSensitivity;
                _settings.rotationLerpSpeed = rotationLerpSpeed;
                _settings.minPitchAngle = minPitchAngle;
                _settings.maxPitchAngle = maxPitchAngle;
                _settings.zoomSpeed = zoomSpeed;
                _settings.minDistance = minDistance;
                _settings.maxDistance = maxDistance;
                _settings.initialZoom = Mathf.Clamp(5f, minDistance, maxDistance);
                return _settings;
            }
        }

        public Transform LookTarget => lookTarget;

        public float CollisionPadding =>
            _collisionComponent != null ? _collisionComponent.CollisionPadding : 0.5f;

        [Inject]
        public void Construct(ICinemaCameraControl control, ICinemaCameraCollision collision)
        {
            _control = control;
            _collision = collision;
            _collisionComponent = GetComponent<CinemaCameraCollision>();
            _control.Bind(this);
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            _control?.TickInput();
        }

        private void LateUpdate()
        {
            _control?.TickMotion(Time.smoothDeltaTime);
        }

        private void FixedUpdate()
        {
            _control?.ResolveCollision(_collision);
        }

        public void SetLoocTarget(Transform trTarget)
        {
            lookTarget = trTarget;
            _control?.SetLookTarget(trTarget);
        }

        public void SetLookEnabled(bool enabled)
        {
            _control?.SetLookEnabled(enabled);
        }
    }
}
