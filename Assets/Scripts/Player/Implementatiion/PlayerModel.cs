using Lucky38.UnitReact.Core;
using UnityEngine;
namespace Sasha19.Entity.MyPlayer
{
    public class PlayerModel : IPlayerModel
    {
        public ReactiveProperty<PlayerData> reactivePlayerData { get; private set; } = new();
        public void UpdatePlayerData(PlayerData newPlayerData)
        {
            reactivePlayerData.Value = newPlayerData;
            Debug.Log(reactivePlayerData.Value.StatsState.CurrentHp);
        }
    }
}