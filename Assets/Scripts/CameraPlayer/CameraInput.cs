using Lucky38.UnitReact.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CameraInput : IInitializable, IDisposable
{
    private PlayerInput _playerInput;
    private InputAction _onRotate;
    private InputAction _onScroll;
    public CameraInput(PlayerInput playerInput)
    {
        _playerInput = playerInput;
    }


    public void Initialize()
    {
        _onRotate = _playerInput.actions["Look"];
        _onScroll = _playerInput.actions["Scroll"];
        _onRotate.started += OnStartOfRotatte;
        _onRotate.canceled += OnStopOfRotatte;
        _onScroll.started += OnStartOfScroll;
        _onScroll.canceled += OnStopScroll;
    }

    public void Dispose()
    {
        _onRotate.started -= OnStartOfRotatte;
        _onRotate.canceled -= OnStopOfRotatte;
        _onScroll.started -= OnStartOfScroll;
        _onScroll.canceled -= OnStopScroll;

    }

    private void OnStartOfRotatte(InputAction.CallbackContext ctx)
    {
        MessageBroker.Publish(ctx.ReadValue<Vector2>());
    }
    private void OnStopOfRotatte(InputAction.CallbackContext ctx)
    {
        MessageBroker.Publish(Vector2.zero);
    }
    private void OnStartOfScroll(InputAction.CallbackContext ctx)
    {
        MessageBroker.Publish(ctx.ReadValue<Vector2>());
    }
    private void OnStopScroll(InputAction.CallbackContext ctx)
    {
        MessageBroker.Publish(Vector2.zero);
    }

}
