using UnityEngine;
using Zenject;

public class PlayerComponent : MonoBehaviour
{
    public PlayerFaced _playerFaced {  get; private set; }
    [Inject]
    public void Construct()
    {

    }
}
