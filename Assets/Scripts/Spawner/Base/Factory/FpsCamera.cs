using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class FPSCamera : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 0.05f;
    [SerializeField]
    private float minPitch = -85f;
    [SerializeField]
    private float maxPitch = 85f;

    [Inject]
    private PlayerInput playerInput;
    [SerializeField]
    
    private string actionName = "Look";  

    private InputAction _lookAction;
    private float _pitch;
    private Transform _playerBody;

    private void Start()
    {
        _playerBody = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerInput != null)
        {
            _lookAction = playerInput.actions.FindAction(actionName);
        }
    }

    private void Update()
    {
        if (_lookAction == null) return;

        Vector2 lookInput = _lookAction.ReadValue<Vector2>() * sensitivity;

        _playerBody.Rotate(Vector3.up * lookInput.x);

        _pitch -= lookInput.y;
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
}