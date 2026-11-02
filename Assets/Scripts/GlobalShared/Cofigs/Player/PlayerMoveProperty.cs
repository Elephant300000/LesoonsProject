using UnityEngine;
namespace Sasha19.MainShared.Configs
{
    [CreateAssetMenu (fileName = "MovePropety", menuName = "PlayerSettings/MovePropety")]
    public class PlayerMoveProperty : ScriptableObject, IPlayerMoveProperty
    {
        [Header("Movement")]
        [field:SerializeField]
        public float Speed{ get; private set; } = 20;
        [field: SerializeField]
        public float JumpForse { get; private set; } = 20;

        [Header("Attack")]
        [field: SerializeField]
        public float DamagePerHit { get; private set; }
        
    }
    public interface IPlayerMoveProperty
    {
        float Speed { get; }
        float JumpForse { get; }
        float DamagePerHit { get; }
    }
}
