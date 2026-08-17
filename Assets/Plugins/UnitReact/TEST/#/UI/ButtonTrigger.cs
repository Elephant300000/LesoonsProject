using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Unity UI buttons → plain C# events for Reactive EventBinding.
/// </summary>
public class ButtonTrigger : MonoBehaviour
{
    public Button btIncrement;
    public Button btDecrement;

    public event Action<float> OnIncrementClicked;
    public event Action<float> OnDecrementClicked;

    public float heal;
    public float damage;

    private void Awake()
    {
        if (btIncrement != null)
            btIncrement.onClick.AddListener(HandleIncrement);
        if (btDecrement != null)
            btDecrement.onClick.AddListener(HandleDecrement);
    }

    private void OnDestroy()
    {
        if (btIncrement != null)
            btIncrement.onClick.RemoveListener(HandleIncrement);
        if (btDecrement != null)
            btDecrement.onClick.RemoveListener(HandleDecrement);
    }

    private void HandleIncrement() => OnIncrementClicked?.Invoke(heal);
    private void HandleDecrement() => OnDecrementClicked?.Invoke(damage);
}
