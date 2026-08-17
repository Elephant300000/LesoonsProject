using Lucky38.UnitReact.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MovementPlayerInput : IInitializable, IDisposable
{
    private PlayerInput _playerInput;
    private InputAction _onMove;
    private InputAction _onJump;
    public MovementPlayerInput(PlayerInput playerInput)
    {
        _playerInput = playerInput;
    }

    public void Dispose()
    {
        _onMove.canceled -= OnStopedMove;
        _onMove.started -= OnStartedMove;
        _onMove.performed -= OnJump;
    }

    public void Initialize()
    {
        _onMove = _playerInput.actions["Move"];
        _onJump = _playerInput.actions["Jump"];
        _onMove.started += OnStartedMove;
        _onMove.canceled += OnStopedMove;
        _onJump.performed += OnJump;
    }
    private void OnJump(InputAction.CallbackContext ctx)
    {
        MessageBroker.Publish(new Unit());

    }

    private void OnStartedMove(InputAction.CallbackContext ctx)
    {
        MessageBroker.Publish(ctx.ReadValue<Vector2>());

    }
    private void OnStopedMove(InputAction.CallbackContext ctx)
    {
        MessageBroker.Publish(Vector2.zero);
    }
}
