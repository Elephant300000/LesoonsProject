using Lucky38.UnitReact.Core;
using Sasha19.Enums;
using Sasha19.MainShared.Configs;
using System;
using UnityEngine;
using Zenject;
namespace Sasha19.Entity.MyPlayer
{
    internal sealed class PlayerMovement : IFixedTickable, IPlayerMovement, IInitializable, IDisposable
    {
        private float _speed;
        private float _jumpForse;
        private bool _isInCollision;
        private Vector3 _moveAxis;
        private CompositeDisposable _subscribers = new();
        private readonly IPlayerMoveProperty _moveProperty;

        private Rigidbody _myRb;
        public PlayerMovement([Inject(Id = ZenjectsId.PlayeRb)] Rigidbody myRb, IPlayerMoveProperty moveProperty)
        {
            _myRb = myRb;
            _moveProperty = moveProperty;
            _speed = _moveProperty.Speed;
            _jumpForse = _moveProperty.JumpForse;
        }

        public void Initialize()
        {
            MessageBroker.Receive<Vector2>().Subscribe(MoveAxis).AddTo(_subscribers);
            MessageBroker.Receive<Unit>().Gate(0.3f).Subscribe(Jump).AddTo(_subscribers);
            Debug.Log("INit");
        }

        public void FixedTick()
        {
            Move(_moveAxis);
            Debug.Log("Tick");
        }

        public void Dispose()
        {
            _subscribers.Dispose();
        }

        private void Jump(Unit unit)
        {
           // if (_isInCollision)
           // {
                _myRb.AddForce(new Vector3(0, _jumpForse, 0), ForceMode.Impulse);
           // }
            Debug.Log("jump");

        }

        private void Move(Vector3 vector3)
        {
            _myRb.AddForce(vector3 * _speed, ForceMode.Force);
        }
        private void MoveAxis(Vector2 axis)
        {
            _moveAxis = new(axis.x, 0, axis.y);
            Debug.Log("Move");

        }

    }
    public interface IPlayerMovement
    {

    }
}