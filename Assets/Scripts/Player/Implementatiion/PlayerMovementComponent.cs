using Lucky38.UnitReact.Core;
using System;
using UnityEngine;
using Zenject;
[RequireComponent(typeof (Rigidbody))]
public class PlayerMovementComponent : MonoBehaviour, IPlayer, IInitializable, IDisposable
{
    private Rigidbody _myRb;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpForse;
    private CompositeDisposable _subscribers = new();
    private Vector3 _moveAxis;
    private bool _isInCollision;

    public void Dispose()
    {
        _subscribers.Dispose();
    }

    public void Initialize()
    {
        _myRb = GetComponent<Rigidbody>();
        MessageBroker.Receive<Vector2>().Subscribe(MoveAxis).AddTo(_subscribers);
        MessageBroker.Receive<Unit>().Gate(0.3f).Subscribe(Jump).AddTo(_subscribers);
    }

    public void Jump(Unit unit)
    {
        if (_isInCollision)
        {
            _myRb.AddForce(new Vector3(0, _jumpForse, 0), ForceMode.Impulse);
            Debug.Log("JUMP");
        }
    }
    private void FixedUpdate()
    {
        Move(_moveAxis);
    }

    public void Move(Vector3 vector3)
    {
        _myRb.AddForce(vector3 * _speed, ForceMode.Force);
        Debug.Log("Move");
    }
    public void MoveAxis(Vector2 axis)
    {
        _moveAxis = new(axis.x, 0, axis.y);
    }
    private void OnCollisionExit(Collision collision)
    {
        _isInCollision = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "ShtrafStena")
            _isInCollision = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "JumpOrb")
            _isInCollision = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "JumpOrb")
            _isInCollision = false;
    }
}