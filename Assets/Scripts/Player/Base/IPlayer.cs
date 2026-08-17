using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayer 
{
    void Move(Vector2 vector2);
    void Jump();
}
