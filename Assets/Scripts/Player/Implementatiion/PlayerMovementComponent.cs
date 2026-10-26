using Lucky38.UnitReact.Core;
using System;
using UnityEditor.PackageManager;
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
    private PlayerModel _playerModel = new PlayerModel();
    private IPlayerFacedProvaiderSet _facedProvaiderSet;
    private float _interval;
    public void Initialize()
    {
        _facedProvaiderSet.PlayerFacedSet(_playerModel);
        _myRb = GetComponent<Rigidbody>();
        MessageBroker.Receive<Vector2>().Subscribe(MoveAxis).AddTo(_subscribers);
        MessageBroker.Receive<Unit>().Gate(0.3f).Subscribe(Jump).AddTo(_subscribers);
        var newPlData = _playerModel.reactivePlayerData.Value.With(new StatsState(100,100),11,11,transform.position,transform.position,transform.rotation);
        _playerModel.UpdatePlayerData(newPlData);
    }
    public void Dispose()
    {
        _subscribers.Dispose();
    }
    [Inject]
    public void Construct(IPlayerFacedProvaiderSet facedProvaider)
    {
        _facedProvaiderSet = facedProvaider;
    }
    public void Jump(Unit unit)
    {
        if (_isInCollision)
        {
            _myRb.AddForce(new Vector3(0, _jumpForse, 0), ForceMode.Impulse);
        }
    }
    private void FixedUpdate()
    {
        Move(_moveAxis);
    }

    public void Move(Vector3 vector3)
    {
        _myRb.AddForce(vector3 * _speed, ForceMode.Force);
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
        if (collision.gameObject.tag == "ShtrafStena" )
        {
            var statsState = _playerModel.reactivePlayerData.Value.StatsState;
            var result = statsState.CurrentHp - 5;
            Debug.Log(result + "result");
            _playerModel.UpdatePlayerData(_playerModel.reactivePlayerData.Value.With(
                new StatsState(result,
                _playerModel.reactivePlayerData.Value.StatsState.MaxHp)));
  
        }
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