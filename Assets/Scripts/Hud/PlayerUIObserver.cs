using Lucky38.UnitReact.Core;
using System;
using Zenject;

public class PlayerUIObserver : IInitializable, IDisposable
{
    public PlayerUIObserver(IPlayerFacedProvaiderGet facedProvaider, IStatDisplayWidget statDisplayWidget)
    {
        FacedProvaider = facedProvaider;
        this.statDisplayWidget = statDisplayWidget;
    }


    public IPlayerFacedProvaiderGet FacedProvaider;


    public IStatDisplayWidget statDisplayWidget;
    public CompositeDisposable compositeDisposable = new();

    public void Initialize()
    {
        var faced = FacedProvaider.PlayerFasedGet();
        if (faced == null) return;
        faced.reactivePlayerData.Select(playerData => playerData.StatsState)
            .DistinctUntilChanged(statsState => (statsState.CurrentHp, statsState.MaxHp))
            .Subscribe(statsState => statDisplayWidget.UpdateValue(statsState.CurrentHp,statsState.MaxHp))
            .AddTo(compositeDisposable);
    }
    public void Dispose()
    {
            compositeDisposable?.Dispose();
    }

}
