================================================================================
  SubjectObserver — демо-сцена паттерна Subject + Observer (UnitReact)
================================================================================
Namespace: Lucky38.TestReact.EntityScene
Сцена:     Subject_Observer.unity
Стек:      Zenject + Lucky38.UnitReact.Core

Цель папки
----------
Показать на минимальном игровом примере, как правильно использовать
локальный Subject/Observer из UnitReact:

  • у каждой сущности — свой уникальный SubjectEntity (не глобальная шина);
  • данные живут в Entity, Subject только меняет их и рассылает событие;
  • глобальный UI (кнопки / полоска) не держит ссылку на Entity напрямую —
    ищет сущность в RepositoryEntity по Id из EntitySelectable;
  • HUD обновляется через ObserverContext, подписанный на локальный Subject.

Смысл демо
----------
На сцене несколько кубиков (Entity). Клик по кубику:
  1) визуально выделяет его (scale);
  2) записывает его Id в глобальный ReactiveProperty<EntitySelectable>.

Один общий Scrollbar (HudEntity) показывает FildAmount выбранного кубика.
Кнопки Increment / Decrement меняют FildAmount только у текущего выбранного
Entity — через его локальный SubjectEntity.

================================================================================
  КАКИЕ ЧАСТИ UNITYREACT ИСПОЛЬЗУЮТСЯ
================================================================================

  Subject / Observer
    SubjectEntity : PooledSyncEventSubject
    HudEntityObserver : SyncObserverContext
    Событие EntityUpdatedEvent : IPoolableEvent
    → локальная изоляция, pooled dispatch, Register + SubscribeAll

  ReactiveProperty
    Глобальный «текущий выбранный Id» (EntitySelectable)
    → подписка в EntityHandler

  EventBinding + Gate-оператор
    Кнопки UI → ISubscribable<Unit>
    .Gate(0.2f) — защита от дабл-клика
    .AddTo(CompositeDisposable) — корректная отписка

Не используются в этом демо (намеренно):
  FastEventHub, MessageBroker — здесь нужна локальность на сущность, не глобальная шина.

  ================================================================================
  РАЗДЕЛЕНИЕ ОТВЕТСТВЕННОСТИ (кратко)
================================================================================

  Entity              — View + хранение Data + клик мыши + регистрация в repo
  SubjectEntity       — мутация Data + рассылка EntityUpdatedEvent
  HudEntityObserver   — реакция на событие Subject → отрисовка HUD
  EntityHandler       — глобальный glue: selection + кнопки → repo → Subject
  RepositoryEntity    — lookup Entity по Id
  ButtonTrigger/Hud   — чистые View без бизнес-логики


================================================================================
  КАК ЗАПУСТИТЬ / ПРОВЕРИТЬ НА СЦЕНЕ
================================================================================

  1. Открыть Subject_Observer.unity.
  2. На SceneContext должен висеть SceneEntityInstaller.
  3. На каждом кубике: Entity + GameObjectContext + LocalEntityInstaller.
  4. На UI: HudEntity (Scrollbar), ButtonTrigger (две кнопки).
  5. Play:
       • клик по кубику → он увеличивается, полоска = его FildAmount;
       • Increment / Decrement → полоска меняется только у выбранного;
       • быстрые клики по кнопкам режутся Gate (0.2s).




================================================================================
  АРХИТЕКТУРА: два контекста Zenject
================================================================================

1) Scene Context  — SceneEntityInstaller
   Глобальные синглтоны на всю сцену:
     • HudEntity              — View полоски (Scrollbar)
     • ButtonTrigger          — View кнопок Increment/Decrement
     • RepositoryEntity       — словарь Id → Entity
     • ReactiveProperty
       <EntitySelectable>     — «кто сейчас выбран» (Id)
     • EntityHandler          — глобальный контроллер UI/выбора

2) GameObject Context (на каждом кубике) — LocalEntityInstaller
   Локальные зависимости ЭТОГО кубика:
     • Entity                 — MonoBehaviour-фасад (View + хранение Data)
     • SubjectEntity          — уникальный локальный Subject / бизнес-мутации
     • HudEntityObserver      — ObserverContext: Subject → глобальный HUD

Важно: SubjectEntity создаётся в LocalEntityInstaller.AsSingle() внутри
GameObjectContext. У каждого кубика свой экземпляр. Это НЕ FastEventHub
и НЕ MessageBroker.


================================================================================
  СТРУКТУРА ПАПОК И РОЛИ КЛАССОВ
================================================================================

Entity/
  EntitySelectable.cs
    readonly struct { int Id }
    Значение глобального выбора. Пишет Entity.OnMouseDown, читает EntityHandler.

  Model/
    Entity.cs
      MonoBehaviour-фасад кубика.
      • Хранит Id (стабильный readonly), EntityData, ссылку на свой Subject.
      • Start: инициализирует Data (0.5f), регистрируется в RepositoryEntity.
      • OnDestroy: Unregister из репозитория.
      • OnMouseDown: scale = 1.5, пишет selected.Value = EntitySelectable(Id).
      • OnMouseExit: сбрасывает scale.
      • SetData: единственная точка записи Data (вызывается из Subject).

    EntityData.cs
      EntityData — immutable struct (Id, FildAmount).
      EntityUpdatedEvent : IPoolableEvent — pooled-событие для Subject
      (поле Data, Release сбрасывает в default).

    SubjectEntity.cs
      Наследник PooledSyncEventSubject.
      Уникален для одного Entity (инжектится Entity в ctor).
      • ChangeAmount(delta) — читает Data из Entity, Clamp01, SetData, NotifyState.
      • NotifyState() / NotifyState(data) — InvokePooledAction<EntityUpdatedEvent>.
      Subject НЕ дублирует EntityData — источник правды всегда Entity.Data.

RepositoryEntity/
  RepositoryEntity.cs
    Dictionary<int, Entity>. Register / Unregister / Get(id).
    Нужен глобальному EntityHandler, чтобы по Id достать Entity и его Subject.

Handler/
  EntityHandler.cs
    Глобальный IInitializable / IDisposable (один на сцену).
    НЕ хранит Entity. Работает только через Repository + Selectable.

    Подписки в Initialize:
      1) _selectedEntity.Subscribe(OnSelectionChanged)
         → при смене выбора: repo.Get(id).Subject.NotifyState()
           (HUD сразу показывает данные выбранного кубика)

      2) EventBinding.FromEvent (кнопки ButtonTrigger)
           .Gate(0.2f)
           .Subscribe(OnIncrement / OnDecrement)
           .AddTo(_disposables)
         → по Id из selected: repo.Get(id).Subject.ChangeAmount(±0.1f)

    Dispose → CompositeDisposable отписывает всё.

UI/
  Buttons/ButtonTrigger.cs
    MonoBehaviour: ссылки на Unity Button.
    Пробрасывает onClick в C# events OnIncrementClicked / OnDecrementClicked.
    EventBinding в Handler оборачивает эти events в ISubscribable.

  HUD/HudEntity.cs
    MonoBehaviour: публичный Scrollbar (одна полоска на всю сцену).

  HUD/HudEntityObserver.cs
    SyncObserverContext + IInitializable.
    ctor(SubjectEntity, HudEntity) : base(subject, subject)
      → локальный Subject + LocalEventRouter.
    Initialize:
      Register<EntityUpdatedEvent>(OnEntityUpdated);
      SubscribeAll();
    OnEntityUpdated: scrollbar.size = evt.Data.FildAmount.

Zenject_Installer/
  SceneEntityInstaller.cs     — глобальные биндинги (см. выше).
  Local_Entity_Installer/
    LocalEntityInstaller.cs   — биндинги на каждом кубике (см. выше).


================================================================================
  ПОТОК СОБЫТИЙ (runtime)
================================================================================

A) Клик по кубику
   Entity.OnMouseDown
     → scale 1.5
     → ReactiveProperty<EntitySelectable>.Value = { Id }
     → EntityHandler.OnSelectionChanged
         → RepositoryEntity.Get(Id)
         → entity.Subject.NotifyState()
             → InvokePooledAction<EntityUpdatedEvent>
                 → HudEntityObserver.OnEntityUpdated
                     → scrollbar.size = FildAmount этого кубика

B) Клик Increment / Decrement
   ButtonTrigger → C# event
     → EventBinding (+ Gate 0.2s анти-спам)
       → EntityHandler.OnIncrement / OnDecrement
           → RepositoryEntity.Get(selected.Id)
           → entity.Subject.ChangeAmount(±0.1)
               → Entity.SetData(new EntityData(...))
               → NotifyState → снова HudEntityObserver → полоска

C) Уничтожение кубика
   Entity.OnDestroy → Repository.Unregister
   (локальный GameObjectContext и Subject уходят вместе с объектом)


   ================================================================================
  ЗАЧЕМ ЭТОТ ПРИМЕР В ТЕСТАХ
================================================================================

Это учебный/интеграционный пример ответа на вопрос:
  «когда брать Subject/Observer, а не FastEventHub / MessageBroker?»

Ответ в этом демо:
  несколько одинаковых сущностей, у каждой своё состояние и свой поток
  событий; глобальный UI работает с ними через Id + Repository, а обновление
  HUD идёт через локальный Subject → ObserverContext, без глобального шума
  и без проверок EntityId в каждом подписчике.
================================================================================


