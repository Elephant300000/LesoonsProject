using Lucky38.UnitReact.Core;
using UnityEngine;

public class Character : MonoBehaviour
{
    private readonly ReactiveProperty<float> _health = new(100f);
     
    public IReadOnlyReactiveProperty<float> Health => _health;

 
    public void TakeDamage(float amount)
    {
        _health.Value -= amount;
    }

    private void OnDestroy()
    {
        _health.Dispose();
    }
}
