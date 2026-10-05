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
        _onScroll.started += OnScroll;
        _onScroll.canceled += OnStopScroll;
        _onRotate.performed += OnRotate;
    }

    public void Dispose()
    {
        _onRotate.performed -= OnRotate;
        _onScroll.started -= OnScroll;
        _onScroll.canceled -= OnStopScroll;
    }

    private void OnStopScroll(InputAction.CallbackContext ctx)
    {
        MessageBroker.Publish(new ZoomEventData(Vector2.zero));
    }
    private void OnScroll(InputAction.CallbackContext ctx)
    {
        MessageBroker.Publish(new ZoomEventData(ctx.ReadValue<Vector2>()));
    }
    private void OnRotate(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            MessageBroker.Publish(new DeltaEventData(ctx.ReadValue<Vector2>()));
        }
        else MessageBroker.Publish(new DeltaEventData(Vector2.zero));
    }
    
}
public struct ZoomEventData
{
    public ZoomEventData(Vector2 zoom)
    {
        Zoom = zoom;
    }

    public Vector2 Zoom { get;}
}
public struct DeltaEventData
{
    public DeltaEventData(Vector2 scroll)
    {
        DeltaMouse = scroll;
    }

    public Vector2 DeltaMouse { get;}
}