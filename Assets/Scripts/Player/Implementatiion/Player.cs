using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Lucky38.UnitReact;
using Lucky38.UnitReact.Core;
using System;

public class Player : MonoBehaviour, IPlayer, IInitializable, IDisposable
{
    private Rigidbody _myRb2d;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpForse;
    private CompositeDisposable _subscribers = new();

    public void Dispose()
    {
        _subscribers.Dispose();
    }

    public void Initialize()
    {
        MessageBroker.Receive<Vector2>().Subscribe(Move).AddTo(_subscribers);
        MessageBroker.Receive<Unit>().Gate(1).Subscribe(Jump).AddTo(_subscribers);
    }

    public void Jump()
    {
        _myRb2d.AddForce(new Vector3(0, _jumpForse,0), ForceMode.Impulse);
        Debug.Log("JUMP");

    }

    public void Move(Vector2 vector2)
    {
        _myRb2d.AddForce(vector2 * _speed, ForceMode.Force);
        Debug.Log("Move");

    }

}