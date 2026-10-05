using System;
using Zenject;

public class PlayerUIObserver : IInitializable, IDisposable
{
    public IPlayerFacedProvaiderGet FacedProvaider;
    public IStatDisplayWidget statDisplayWidget;

    public void Initialize()
    {
        var faced = FacedProvaider.PlayerFasedGet();
        if (faced == null) return;
        faced.reactivePlayerData.Subscribe(playerData => statDisplayWidget.UpdateValue(playerData.CurrentHp, playerData.MaxHp));
    }
    public void Dispose()
    {
        
    }

}
