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

    void IStatDisplayWidget.UpdateValue(PlayerData playerData)
    {
        if(playerData.MaxHp <= 0) playerData.MaxHp = 1;
        _statBar.fillAmount = playerData.CurrentHp / playerData.MaxHp;
    }
}
public interface IStatDisplayWidget
{
    TypeOfStat typeOfStat { get; }
    void UpdateValue(PlayerData playerData);
}
public enum TypeOfStat
{
    Hp,
    Stamina,
    Mp
}