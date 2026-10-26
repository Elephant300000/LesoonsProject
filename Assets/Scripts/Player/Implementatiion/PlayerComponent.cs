using UnityEngine;
using Zenject;

public class PlayerComponent : MonoBehaviour
{
    public PlayerModel _playerFaced {  get; private set; }
    [Inject]
    public void Construct()
    {

    }
}
