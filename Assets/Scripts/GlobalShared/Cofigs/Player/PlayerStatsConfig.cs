using System;
using UnityEngine;
namespace Sasha19.MainShared.Configs
{
    [CreateAssetMenu(fileName = "MovePropety", menuName = "PlayerSettings/MovePropety")]
    public class PlayerStatsConfig : ScriptableObject, IPlayerStatsConfig
    { 
        [field:SerializeField]
        public PlayerStats playerStats {  get; private set; }
    }
    [Serializable]
    public struct PlayerStats
    {
        public float StatrtHp;
        public float MaxHp;
    }
    public interface IPlayerStatsConfig
    {
        PlayerStats playerStats { get; }
    }
}
