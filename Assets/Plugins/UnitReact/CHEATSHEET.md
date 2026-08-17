# UnityReact — шпоргалка по синтаксису

Namespace: `Lucky38.UnitReact.Core`

Быстрый выбор:
| Нужно | Инструмент |
|-------|------------|
| Локальное значение + уведомление | `ReactiveProperty<T>` |
| Локальный сигнал без данных | `ReactiveTrigger` |
| Глобальный message bus (любой `T`) | `MessageBroker` |
| Глобальный pooled event (high-perf) | `FastEventHub*` |
| Обернуть C# `event` | `EventBinding` |
| DI-observer на Subject / Hub | `SyncObserverContext` / `AsyncObserverContext` |
| Локальный диспетчер событий | `*EventSubject` |

Отписка везде: `IDisposable.Dispose()`, `using`, или `.AddTo(CompositeDisposable)`.

---

## 1. ReactiveProperty&lt;T&gt;

Хранит значение. Подписка сразу получает текущее значение. Сеттер с тем же значением не стреляет.

```csharp
// создать
var hp = new ReactiveProperty<int>(100);

// читать / писать
int v = hp.Value;
hp.Value = 80;
int implicitCast = hp; // implicit operator T

// подписаться
using var sub = hp.Subscribe(val => Debug.Log(val));
// ↑ сразу вызовется с 100

// отписаться
sub.Dispose();
// или
hp.Dispose(); // очистить всех подписчиков
```

С операторами + lifecycle:

```csharp
readonly CompositeDisposable _bag = new();

hp
  .Skip(1)           // пропустить initial
  .Where(v => v > 0)
  .Take(5)
  .Subscribe(OnHp)
  .AddTo(_bag);

void OnDestroy() => _bag.Dispose();
```

Async-подписка:

```csharp
hp.SubscribeAsync(async (val, ct) =>
{
    await UniTask.Delay(100, cancellationToken: ct);
}, destroyCancellationToken)
.AddTo(_bag);
```

---

## 2. ReactiveTrigger

Сигнал без данных (`Unit`). Не хранит значение. Подписка НЕ вызывается сразу.

```csharp
// создать (+ опциональный gate от дабл-кликов)
var click = new ReactiveTrigger()
    .WithGate(cooldownSeconds: 0.2f, skipIfRunning: true);

// вызвать
click.Invoke();

// подписаться (Action без аргумента — через extension для Unit)
using var sub = click.Subscribe(() => DoSomething());
// или
using var sub2 = click.Subscribe(_ => DoSomething());

// отписаться
sub.Dispose();
click.Clear();   // всех
click.Dispose(); // = Clear
```

---

## 3. MessageBroker (глобальный bus)

Любой `T` (struct/class). Не pooled. Один Subject на тип.

```csharp
struct DamageMsg { public int Amount; }

// подписаться
using var sub = MessageBroker.Receive<DamageMsg>()
    .Where(m => m.Amount > 0)
    .Subscribe(m => Apply(m.Amount));

// вызвать
MessageBroker.Publish(new DamageMsg { Amount = 10 });

// отписаться
sub.Dispose();

// очистить всех подписчиков этого типа
MessageBroker.ClearAll<DamageMsg>();
```

Async:

```csharp
MessageBroker.Receive<DamageMsg>()
    .SubscribeAsync(async (m, ct) =>
    {
        await AnimateAsync(m, ct);
    }, token)
    .AddTo(_bag);
```

---

## 4. FastEventHub (pooled global)

Контейнер: `class` + `IPoolableEvent` + `new()`.

```csharp
sealed class HitEvt : IPoolableEvent
{
    public int Damage;
    public void Release() => Damage = 0; // обязательно сброс
}
```

### 4.1 Sync: Publish + Receive (Rx)

```csharp
// gate (опционально, один раз на тип)
FastEventHubSync.ConfigureGate<HitEvt>(0.1f, skipIfRunning: true);

// подписаться
using var sub = FastEventHubRx.Receive<HitEvt>()
    .Where(e => e.Damage > 0)
    .Subscribe(e => Apply(e.Damage));

// вызвать (setup заполняет pooled instance)
FastEventHubSync.Publish<HitEvt>(e => e.Damage = 25);
FastEventHubSync.Publish<HitEvt>(); // без setup

// отписаться
sub.Dispose();
```

### 4.2 Async: PublishAsync

Для асинхронных подписок можно использовать прямую подписку `FastEventHubAsync.Subscribe`, либо через `AsyncObserverContext` (если нужен DI/жизненный цикл).

**Вариант A — Прямая подписка (одна строка):**

```csharp
// подписаться напрямую
using var sub = FastEventHubAsync.Subscribe<HitEvt>(async (e, ct) => 
{
    await UniTask.Delay(50, cancellationToken: ct);
    Apply(e.Damage);
});

// gate
FastEventHubAsync.ConfigureGate<HitEvt>(0.1f, skipIfRunning: true);

// вызвать
await FastEventHubAsync.PublishAsync<HitEvt>(e => e.Damage = 25);
```

**Вариант B — Через ObserverContext (для сложной логики и DI):**

```csharp
sealed class HitListener : AsyncObserverContext
{
    public void Bind()
    {
        RegisterAsync<HitEvt>(OnHit);
        SubscribeAll(); // добавляет в глобальный HubAsyncCache
    }

    async UniTask OnHit(HitEvt e, CancellationToken ct) { ... }

    public void Unbind() => UnsubscribeAll();
}
```

### 4.3 Сигнатуры (sync vs async не смешивать)

```csharp
// явно (редко нужно — обычно Register сам регистрирует)
FastEventHub.RegisterSignature(typeof(HitEvt), typeof(Action<HitEvt>));
// или
FastEventHub.RegisterSignature(
    typeof(HitEvt),
    typeof(Func<HitEvt, CancellationToken, UniTask>));
```

Один тип события = одна сигнатура. Sync Publish и Async PublishAsync на один `T` — конфликт.

---

## 5. EventBinding (C# event → ISubscribable)

```csharp
public event Action<float> OnHeal;

using var sub = EventBinding.FromEvent<float>(
        add:    h => OnHeal += h,
        remove: h => OnHeal -= h)
    .Where(v => v > 0)
    .Take(10)
    .Subscribe(OnHealReceived);

// вызвать исходный event
OnHeal?.Invoke(15f);

// отписаться → снимает handler с event
sub.Dispose();
```

С `CompositeDisposable`:

```csharp
EventBinding.FromEvent<float>(
        h => buttons.OnIncrementClicked += h,
        h => buttons.OnIncrementClicked -= h)
    .Subscribe(OnIncrement)
    .AddTo(_subscriptions);
```

---

## 6. Операторы и extensions (на любом ISubscribable&lt;T&gt;)

```csharp
source.Where(x => predicate)   // WhereObservable<T>
source.Take(n)                 // первые n, потом авто-отписка
source.Skip(n)                 // пропустить первые n

// Unit → Action без аргумента
ISubscribable<Unit> triggerSource;
triggerSource.Subscribe(() => ...);

// async handler
source.SubscribeAsync(async (data, ct) => { ... }, token);

// lifecycle bag
sub.AddTo(compositeDisposable);
```

Цепочка:

```csharp
MessageBroker.Receive<Msg>()
    .Skip(1)
    .Where(m => m.Ok)
    .Take(3)
    .SubscribeAsync(async (m, ct) => await Handle(m, ct), ct)
    .AddTo(_bag);
```

---

## 7. CompositeDisposable / отписка

```csharp
readonly CompositeDisposable _bag = new();

some.Subscribe(...).AddTo(_bag);
other.Subscribe(...).AddTo(_bag);

void Dispose() => _bag.Dispose(); // отпишет всё
```

Или `using var sub = ...;` / `sub.Dispose()`.

---

## 8. SyncObserverContext / AsyncObserverContext

### Local Subject (DI)

```csharp
// SYNC
class MySyncObs : SyncObserverContext, IInitializable
{
    public MySyncObs(ISyncEventSubject subject) : base(subject, subject) { }

    public void Initialize()
    {
        Register<MyEvt>(OnMyEvt);
        SubscribeAll();
    }

    void OnMyEvt(MyEvt e) { /* ... */ }

    // отписка (если нужно вручную):
    // UnsubscribeAll();
}

// вызвать с subject:
subject.InvokeAction(new MyEvt { ... });
// или pooled:
((IPooledSyncEventSubject)subject).InvokePooledAction<MyEvt>(e => e.Value = 1);
```

```csharp
// ASYNC
class MyAsyncObs : AsyncObserverContext, IInitializable
{
    public MyAsyncObs(IAsyncEventSubject subject) : base(subject, subject) { }

    public void Initialize()
    {
        RegisterAsync<MyEvt>(OnMyEvt);
        SubscribeAll();
        // с replay token:
        // SubscribeAllAsync(destroyCancellationToken);
    }

    async UniTask OnMyEvt(MyEvt e, CancellationToken ct)
    {
        await UniTask.Delay(10, cancellationToken: ct);
    }
}

await subject.InvokeActionAsync(evt);
await pooled.InvokePooledActionAsync<MyEvt>(e => e.Value = 1);
```

### Global Hub (без subject)

```csharp
class GlobalSyncObs : SyncObserverContext // base() → GlobalEventRouter
{
    public void Bind()
    {
        Register<HitEvt>(e => ...);
        SubscribeAll(); // → HubSyncCache
    }
    public void Unbind() => UnsubscribeAll();
}

// publish снаружи:
FastEventHubSync.Publish<HitEvt>(e => ...);
```

```csharp
class GlobalAsyncObs : AsyncObserverContext
{
    public void Bind()
    {
        RegisterAsync<HitEvt>(async (e, ct) => ...);
        SubscribeAll(); // → HubAsyncCache
    }
    public void Unbind() => UnsubscribeAll();
}

await FastEventHubAsync.PublishAsync<HitEvt>(e => ...);
```

### External C# event через Observer

```csharp
SubscribeExternal<Action<float>>(
    add:    h => button.OnClick += h,
    remove: h => button.OnClick -= h,
    handler: OnClick);

// снимет при UnsubscribeAll()
```

---

## 9. Subjects (локальный диспетчер)

```csharp
// sync
var sync = new SyncEventSubject();
sync.ConfigureGate<MyEvt>(0.2f, skipIfRunning: true);
sync.EnableReplay<MyEvt>(capacity: 1);
sync.EnablePooledReplay<MyEvt>(capacity: 1); // только IPoolableEvent

sync.InvokeAction(container);

var pooledSync = new PooledSyncEventSubject();
pooledSync.InvokePooledAction<MyEvt>(e => e.Value = 42);

// async
var async = new AsyncEventSubject();
async.ConfigureAsyncGate<MyEvt>(0.2f, skipIfRunning: true);
async.EnableReplay<MyEvt>(1);
async.EnablePooledReplay<MyEvt>(1);

await async.InvokeActionAsync(container);

var pooledAsync = new PooledAsyncEventSubject();
await pooledAsync.InvokePooledActionAsync<MyEvt>(e => e.Value = 42);
```

Подписка observer’ов — только через `*ObserverContext.Register* + SubscribeAll`, не через Rx `Subscribe` на subject.

---

## 10. IPoolableEvent (контракт)

```csharp
sealed class MyEvt : IPoolableEvent
{
    public int Value;
    public void Release()
    {
        Value = 0; // сброс ВСЕХ полей
    }
}
```

После `Publish` / `InvokePooled*` контейнер уходит в пул — **не кэшируй ссылку** в handler’е.

---

## 11. Карта «кто с кем»

| Подписка | Вызов |
|----------|--------|
| `prop.Subscribe` | `prop.Value = x` |
| `trigger.Subscribe` | `trigger.Invoke()` |
| `MessageBroker.Receive&lt;T&gt;().Subscribe` | `MessageBroker.Publish(msg)` |
| `FastEventHubRx.Receive&lt;T&gt;().Subscribe` | `FastEventHubSync.Publish&lt;T&gt;(setup)` |
| `AsyncObserverContext.RegisterAsync` + `SubscribeAll` (global) | `FastEventHubAsync.PublishAsync&lt;T&gt;(setup)` |
| `EventBinding.FromEvent(...).Subscribe` | исходный `event?.Invoke(...)` |
| `SyncObserverContext.Register` + `SubscribeAll` (local) | `subject.InvokeAction` / `InvokePooledAction` |
| `AsyncObserverContext.RegisterAsync` + `SubscribeAll` (local) | `subject.InvokeActionAsync` / `InvokePooledActionAsync` |

---

## 12. Мини-шпаргалка отписки

```csharp
// 1) using
using var d = source.Subscribe(...);

// 2) Dispose
var d = source.Subscribe(...);
d.Dispose();

// 3) bag
source.Subscribe(...).AddTo(_bag);
_bag.Dispose();

// 4) Observer
SubscribeAll();
UnsubscribeAll(); // + external subscriptions

// 5) MessageBroker nuke
MessageBroker.ClearAll<T>();

// 6) ReactiveProperty / Trigger nuke
prop.Dispose();
trigger.Clear();
```
