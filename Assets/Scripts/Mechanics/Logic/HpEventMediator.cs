using Lucky38.UnitReact.Core;
using UnityEngine;

public class HpEventMediator : PooledSyncEventSubject
{
    private PlayerMovementComponent player;// ссылка на компонент игрока PlayerFaced
    //PlayerFaced.playerdata.hpProperty
    private void OnUpdateHp(float newHp)// float
    {
        // Обновляем UI или выполняем другие действия при изменении HP
        //вызываем subject(InvokePooledAction) для observer хп new берём eventData из shared
        //
        Debug.Log($"HP updated to: {newHp}");
    }
}
