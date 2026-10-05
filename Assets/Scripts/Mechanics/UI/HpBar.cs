using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HpBar : MonoBehaviour, IInitializable, IDisposable
{
    private Image hpBarImage;

    private void Awake()
    {
        hpBarImage = GetComponent<Image>();
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Initialize()
    {
        throw new NotImplementedException();
    }

    private void UpdateHp()//event data
    {
        
    }
}
