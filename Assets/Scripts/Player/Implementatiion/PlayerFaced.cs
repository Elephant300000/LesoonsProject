using Lucky38.UnitReact.Core;

public class PlayerFaced : IPlayerFaced
{
    public ReactiveProperty<PlayerData> reactivePlayerData {  get; private set; } 
    public void UpdatePlayerData(PlayerData newPlayerData)
    {
        reactivePlayerData.Value = newPlayerData;
    }
}

