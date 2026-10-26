using Lucky38.UnitReact.Core;
using UnityEngine;

public class PlayerModel : IPlayerFaced
{
    public ReactiveProperty<PlayerData> reactivePlayerData { get; private set; } = new();
    public void UpdatePlayerData(PlayerData newPlayerData)
    {
        reactivePlayerData.Value = newPlayerData;
        Debug.Log(reactivePlayerData.Value.StatsState.CurrentHp);
        // UpdatePlayerData( newPlayerData.With(new StatsState(curent-5,max)) )
    }
}