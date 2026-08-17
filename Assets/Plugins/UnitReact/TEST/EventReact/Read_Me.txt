================================================================================
  EventReact — демо FastEventHub + MessageBroker (без Subject/Property/EventBinding)
================================================================================
Namespace: Lucky38.TestReact.EventBusDemo
Сцена:     EventReact.unity

ЦЕЛЬ
----
Показать практичный случай, где ОБЫЧНЫЙ C# event / прямой вызов методов
ломается, а глобальные шины UnitReact решают задачу чисто:

  • FastEventHub  — gameplay-пульс (частый, pooled, с Gate)
  • MessageBroker — лёгкие UI/лог-сообщения между независимыми системами

Ни Zenject Subject, ни ReactiveProperty, ни EventBinding здесь не нужны.


СЦЕНАРИЙ (игровой, не «пример ради примера»)
--------------------------------------------
Одна глобальная кнопка = игрок кастует AoE / «удар по миру».

После клика должны отреагировать НЕСКОЛЬКО независимых систем:
  Combat  — посчитать урон
  VFX     — «заспавнить» волну (лог)
  Audio   — «проиграть» звук (лог)
  Quest   — обновить прогресс (не на каждый тик)

Системы НЕ знают друг о друге и НЕ знают о кнопке.
Кнопка НЕ держит список панелей.

Дополнительно системы шлют короткие UI-логи друг другу
(чтобы Quest/Combat видели чужие итоги) — через MessageBroker.


ПОЧЕМУ НЕ ХВАТИТ ОБЫЧНОГО C# event
----------------------------------
1) Publisher (кнопка) должен либо:
     - хранить ссылки на все 4 панели, либо
     - иметь static event, на который все подписываются.
   При появлении 5-й системы правишь кнопку / static список.
2) Нет Gate «из коробки» → дабл-клик = 2 урона / 2 VFX / 2 звука.
3) Нет пула → на частых пульсах аллокации (если тащить class-payload).
4) Lifetime: static event легко течёт, если панель забыла отписаться;
   здесь IDisposable + OnDisable.
5) Два разных канала (gameplay vs UI-log) на одном C# event =
   либо God-object, либо каша из типов.


РОЛИ ШИН
--------
FastEventHub  (WorldPulseEvent : IPoolableEvent)
  • источник правды gameplay
  • ConfigureGate против спама кнопки
  • Publish без ссылок на слушателей
  • Receive через FastEventHubRx

MessageBroker (UiLogMessage struct)
  • мягкие уведомления/логи после реакции системы
  • без IPoolableEvent / без пула
  • Where(channel != mine) — панель видит чужие логи, не свои эхо


СКРИПТЫ
-------
Events/BusMessages.cs
  WorldPulseEvent, UiLogMessage

Trigger/GlobalPulseTrigger.cs
  вешается рядом с ButtonTrigger
  onClick → FastEventHubSync.Publish<WorldPulseEvent>
  Gate настраивается в Awake

Panels/SystemLogPanel.cs
  вешается на каждое из 4 окон (Image + TMP)
  настройки:
    systemKind          Combat / Vfx / Audio / Quest
    channelName         имя канала для Broker
    logText             TMP
    listenWorldPulse    слушать Hub
    publishUiLog...     слать Broker после реакции
    listenUiLogFromOthers  слушать чужие Broker-логи

ButtonTrigger.cs
  просто ссылка на UnityEngine.UI.Button


СБОРКА НА СЦЕНЕ (без доп. ассетов)
---------------------------------
1. Canvas:
   - 4 окна: Image (фон) + дочерний TMP (лог)
   - 1 Button (глобальный триггер)

2. На Button:
   - ButtonTrigger  → bt = эта кнопка
   - GlobalPulseTrigger → buttonTrigger = тот же компонент

3. На каждое окно:
   - SystemLogPanel
     окно1: systemKind=Combat, channelName=Combat, logText=TMP
     окно2: Vfx / Vfx
     окно3: Audio / Audio
     окно4: Quest / Quest

4. Play → жми кнопку:
   • все 4 окна пишут свою реакцию на один и тот же Hub-пульс
   • ниже появляются строки «← OtherChannel: ...» из MessageBroker
   • быстрые клики режутся Gate (~0.35s)


ЧТО ДОКАЗЫВАЕТ ДЕМО
-------------------
• Новую систему = новый SystemLogPanel, кнопка не меняется.
• Gameplay (Hub) и UI-log (Broker) разделены.
• Без DI-графа Subject/Observer и без EventBinding.
• Без «примеров ради примера»: это тот же паттерн, что
  Combat/VFX/Audio/Quest реакция на skill cast в бою.
================================================================================
