using Lucky38.UnitReact.Core;
using System;
using Zenject;

/// <summary>
/// Wires ButtonTrigger C# events to HudBur via Reactive EventBinding.
/// </summary>
public class HudBarObserver : IInitializable, IDisposable
{
    private readonly Character _character;
    private readonly ButtonTrigger _buttons;
    private readonly HudBur _bur;
    private readonly CompositeDisposable _subscriptions = new();

    public HudBarObserver(Character character,ButtonTrigger buttons, HudBur bur)
    {
        _character = character;
        _buttons = buttons;
        _bur = bur;
    }

    public void Initialize()
    {
        EventBinding.FromEvent<float>(
                h => _buttons.OnIncrementClicked += h,
                h => _buttons.OnIncrementClicked -= h)
            .Where(amount => amount > 0)
            .Subscribe(OnIncrement)
            .AddTo(_subscriptions);

        EventBinding.FromEvent<float>(
                h => _buttons.OnDecrementClicked += h,
                h => _buttons.OnDecrementClicked -= h)
            .Take(6)
            .Skip(3)
            .Gate(1)
            .Where(amount => amount > 1)
            .Subscribe(OnDecrement)
            .AddTo(_subscriptions);

        _character.Health
            .Subscribe(_bur.UpdateHealthBur)
            .AddTo(_subscriptions);
    }

    private void OnIncrement(float heal) => _bur.Increment(heal);
    private void OnDecrement(float damage) => _bur.Decrement(damage);

    public void Dispose() => _subscriptions.Dispose();
}
