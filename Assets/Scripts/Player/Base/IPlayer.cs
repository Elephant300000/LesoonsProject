using Lucky38.UnitReact.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayer 
{
    void MoveAxis(Vector2 vector2);
    void Jump(Unit unit);
}
