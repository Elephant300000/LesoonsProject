using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatDisplayWidget : MonoBehaviour, IStatDisplayWidget
{
    private Image _statBar;
    [SerializeField]
    private TypeOfStat typeOfStat;

    TypeOfStat IStatDisplayWidget.typeOfStat => typeOfStat;

    private void Awake()
    {
        _statBar = GetComponent<Image>();
    }

    void IStatDisplayWidget.UpdateValue(float value,float maxValue)
    {
        if(maxValue <= 0) maxValue = 1;
        _statBar.fillAmount = value / maxValue;
    }
}
public interface IStatDisplayWidget
{
    TypeOfStat typeOfStat { get; }
    void UpdateValue(float value, float maxValue);
}
public enum TypeOfStat
{
    Hp,
    Stamina,
    Mp
}