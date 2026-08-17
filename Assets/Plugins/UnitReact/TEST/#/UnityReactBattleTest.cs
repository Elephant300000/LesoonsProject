using Cysharp.Threading.Tasks;
using Lucky38.UnitReact.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Lucky38.UnitReact.Core.Tests
{
    /// <summary>
    /// Self-contained battle suite — creates subjects/observers via <c>new</c>, no DI required.
    /// Hang on any active GameObject (or bind via installer) and enter Play Mode.
    /// </summary>
    public sealed class UnityReactBattleTest : MonoBehaviour, IInitializable
    {
        [SerializeField] private bool runOnStart = true;

        private int _passed;
        private int _failed;
        private bool _started;
        private readonly StringBuilder _log = new();

        private void Awake()
        {
            Debug.Log("[UnitReactBattle] Awake — component is alive on " + gameObject.name);
        }

        private void Start()
        {
            Debug.Log("[UnitReactBattle] Start — runOnStart=" + runOnStart);
            if (runOnStart)
                KickOff("Start");
        }

        public void Initialize()
        {
            Debug.Log("[UnitReactBattle] Initialize (Zenject)");
            if (runOnStart)
                KickOff("Initialize");
        }

        [ContextMenu("Run UnitReact Battle Tests")]
        public void RunFromContextMenu() => KickOff("ContextMenu");

        private void KickOff(string source)
        {
            if (_started)
            {
                Debug.Log("[UnitReactBattle] Already running/ran — skip (" + source + ")");
                return;
            }

            _started = true;
            Debug.Log("[UnitReactBattle] Running suite via " + source + "...");
            RunAll().Forget();
        }

        private async UniTaskVoid RunAll()
        {
            _passed = 0;
            _failed = 0;
            _log.Clear();
            Line("=== UnitReact Battle Test ===");

            try
            {
                // 1. Old Subject/Observer tests
                await Test_SyncDispatch();
                await Test_SyncMultiObserverOrder();
                await Test_SyncGateCooldown();
                await Test_SyncGateSkipIfRunning();
                await Test_SyncPooledReplayLateSubscribe();
                await Test_AsyncDispatch();
                await Test_AsyncGateSkipIfRunning();
                await Test_AsyncPooledReplayLateSubscribe();
                await Test_CloneableReplay();

                // 2. New Reactive 5 Types tests
                await Test_ReactiveProperty();
                await Test_ReactiveTrigger();
                await Test_MessageBroker();
                await Test_FastEventHub();
                await Test_EventBinding();
                await Test_RxExtensions();
            }
            catch (Exception e)
            {
                Fail("Unhandled", e.ToString());
                Debug.LogException(e);
            }

            Line($"=== DONE: {_passed} passed, {_failed} failed ===");
            var summary = _log.ToString();
            if (_failed == 0)
                Debug.Log(summary);
            else
                Debug.LogError(summary);
        }

        private void Line(string msg) => _log.AppendLine(msg);

        private void Pass(string name)
        {
            _passed++;
            Line($"[PASS] {name}");
            Debug.Log($"[UnitReactBattle] PASS {name}");
        }

        private void Fail(string name, string reason)
        {
            _failed++;
            Line($"[FAIL] {name}: {reason}");
            Debug.LogError($"[UnitReactBattle] FAIL {name}: {reason}");
        }

        private void Assert(string name, bool condition, string reason = "assertion failed")
        {
            if (condition) Pass(name);
            else Fail(name, reason);
        }

        // =================================================================================
        // EVENTS & PROBES
        // =================================================================================

        private sealed class SyncEvt : IPoolableEvent
        {
            public int Value;
            public void Release() => Value = 0;
        }

        private sealed class AsyncEvt : IPoolableEvent
        {
            public int Value;
            public void Release() => Value = 0;
        }

        private sealed class CloneEvt : ICloneableEvent<CloneEvt>
        {
            public int Value;
            public CloneEvt Clone() => new CloneEvt { Value = Value };
        }

        private sealed class GlobalSyncEvt : IPoolableEvent
        {
            public int Value;
            public void Release() => Value = 0;
        }

        private sealed class GlobalAsyncEvt : IPoolableEvent
        {
            public int Value;
            public void Release() => Value = 0;
        }

        private sealed class SyncProbe : SyncObserverContext
        {
            public readonly List<int> Received = new();
            public SyncProbe(ISyncEventSubject subject) : base(subject, subject) { }
            public void Bind()
            {
                Register<SyncEvt>(e => Received.Add(e.Value));
                SubscribeAll();
            }
        }

        private sealed class AsyncProbe : AsyncObserverContext
        {
            public readonly List<int> Received = new();
            public int ActiveCount;
            public int MaxActive;

            public AsyncProbe(IAsyncEventSubject subject) : base(subject, subject) { }

            public void Bind(int delayMs = 0)
            {
                RegisterAsync<AsyncEvt>(async (e, ct) =>
                {
                    ActiveCount++;
                    MaxActive = Math.Max(MaxActive, ActiveCount);
                    if (delayMs > 0)
                        await UniTask.Delay(delayMs, cancellationToken: ct);
                    Received.Add(e.Value);
                    ActiveCount--;
                });
                SubscribeAll();
            }
        }

        private sealed class CloneProbe : SyncObserverContext
        {
            public readonly List<int> Received = new();
            public CloneProbe(ISyncEventSubject subject) : base(subject, subject) { }
            public void Bind()
            {
                Register<CloneEvt>(e => Received.Add(e.Value));
                SubscribeAll();
            }
        }

        private sealed class OrderObserver : SyncObserverContext
        {
            private readonly string _name;
            private readonly List<string> _order;
            public OrderObserver(ISyncEventSubject subject, string name, List<string> order) : base(subject, subject)
            {
                _name = name;
                _order = order;
            }
            public void Bind()
            {
                Register<SyncEvt>(_ => _order.Add(_name));
                SubscribeAll();
            }
        }

        private sealed class ReentrantObserver : SyncObserverContext
        {
            private readonly Action _onReact;
            public ReentrantObserver(ISyncEventSubject subject, Action onReact) : base(subject, subject) => _onReact = onReact;
            public void Bind()
            {
                Register<SyncEvt>(_ => _onReact());
                SubscribeAll();
            }
        }

        // =================================================================================
        // OLD SUBJECT/OBSERVER TESTS
        // =================================================================================

        private async UniTask Test_SyncDispatch()
        {
            var subject = new PooledSyncEventSubject();
            var probe = new SyncProbe(subject);
            probe.Bind();

            subject.InvokePooledAction<SyncEvt>(e => e.Value = 42);
            Assert("SyncDispatch", probe.Received.Count == 1 && probe.Received[0] == 42, $"got count={probe.Received.Count}");
            await UniTask.Yield();
        }

        private async UniTask Test_SyncMultiObserverOrder()
        {
            var subject = new SyncEventSubject();
            var order = new List<string>();

            var a = new OrderObserver(subject, "A", order);
            var b = new OrderObserver(subject, "B", order);
            a.Bind();
            b.Bind();

            subject.InvokeAction(new SyncEvt { Value = 1 });
            Assert("SyncObserverOrder_FIFO", order.Count == 2 && order[0] == "A" && order[1] == "B", string.Join(",", order));
            await UniTask.Yield();
        }

        private async UniTask Test_SyncGateCooldown()
        {
            var subject = new SyncEventSubject();
            subject.ConfigureGate<SyncEvt>(0.2f, skipIfRunning: false);
            var probe = new SyncProbe(subject);
            probe.Bind();

            subject.InvokeAction(new SyncEvt { Value = 1 });
            subject.InvokeAction(new SyncEvt { Value = 2 });

            Assert("SyncGateCooldown", probe.Received.Count == 1 && probe.Received[0] == 1, $"count={probe.Received.Count}");

            await UniTask.Delay(250);
            subject.InvokeAction(new SyncEvt { Value = 3 });
            Assert("SyncGateCooldown_AfterWait", probe.Received.Count == 2 && probe.Received[1] == 3, $"count={probe.Received.Count}");
        }

        private async UniTask Test_SyncGateSkipIfRunning()
        {
            var subject = new SyncEventSubject();
            subject.ConfigureGate<SyncEvt>(0f, skipIfRunning: true);
            var hits = 0;
            var reentrantBlocked = false;

            var obs = new ReentrantObserver(subject, () =>
            {
                hits++;
                subject.InvokeAction(new SyncEvt { Value = 99 });
                reentrantBlocked = hits == 1;
            });
            obs.Bind();

            subject.InvokeAction(new SyncEvt { Value = 1 });
            Assert("SyncGateSkipIfRunning", hits == 1 && reentrantBlocked, $"hits={hits}, blocked={reentrantBlocked}");
            await UniTask.Yield();
        }

        private async UniTask Test_SyncPooledReplayLateSubscribe()
        {
            var subject = new PooledSyncEventSubject();
            subject.EnablePooledReplay<SyncEvt>(1);

            subject.InvokePooledAction<SyncEvt>(e => e.Value = 77);

            var late = new SyncProbe(subject);
            late.Bind();

            Assert("SyncPooledReplay_LateSubscribe", late.Received.Count == 1 && late.Received[0] == 77, $"count={late.Received.Count}");
            await UniTask.Yield();
        }

        private async UniTask Test_AsyncDispatch()
        {
            var subject = new PooledAsyncEventSubject();
            var probe = new AsyncProbe(subject);
            probe.Bind();

            await subject.InvokePooledActionAsync<AsyncEvt>(e => e.Value = 5);
            Assert("AsyncDispatch", probe.Received.Count == 1 && probe.Received[0] == 5, $"count={probe.Received.Count}");
        }

        private async UniTask Test_AsyncGateSkipIfRunning()
        {
            var subject = new PooledAsyncEventSubject();
            subject.ConfigureAsyncGate<AsyncEvt>(0f, skipIfRunning: true);

            var probe = new AsyncProbe(subject);
            probe.Bind(delayMs: 100);

            var t1 = subject.InvokePooledActionAsync<AsyncEvt>(e => e.Value = 1);
            var t2 = subject.InvokePooledActionAsync<AsyncEvt>(e => e.Value = 2);

            await UniTask.WhenAll(t1, t2);
            await UniTask.Delay(50);

            Assert("AsyncGateSkipIfRunning", probe.Received.Count == 1 && probe.Received[0] == 1 && probe.MaxActive == 1, $"count={probe.Received.Count}, maxActive={probe.MaxActive}");
        }

        private async UniTask Test_AsyncPooledReplayLateSubscribe()
        {
            var subject = new PooledAsyncEventSubject();
            subject.EnablePooledReplay<AsyncEvt>(1);

            await subject.InvokePooledActionAsync<AsyncEvt>(e => e.Value = 88);

            var late = new AsyncProbe(subject);
            late.Bind();
            await UniTask.Delay(50);

            Assert("AsyncPooledReplay_LateSubscribe", late.Received.Count == 1 && late.Received[0] == 88, $"count={late.Received.Count}");
        }

        private async UniTask Test_CloneableReplay()
        {
            var subject = new SyncEventSubject();
            subject.EnableReplay<CloneEvt>(1);

            var original = new CloneEvt { Value = 9 };
            subject.InvokeAction(original);
            original.Value = 0;

            var late = new CloneProbe(subject);
            late.Bind();

            Assert("CloneableReplay", late.Received.Count == 1 && late.Received[0] == 9, $"count={late.Received.Count}");
            await UniTask.Yield();
        }

        // =================================================================================
        // NEW REACTIVE TESTS (5 Types + Rx)
        // =================================================================================

        private async UniTask Test_ReactiveProperty()
        {
            var hp = new ReactiveProperty<int>(100);
            int lastHp = 0;
            int calls = 0;

            using var sub = hp.Subscribe(val =>
            {
                lastHp = val;
                calls++;
            });

            hp.Value = 80;
            hp.Value = 80; // Should not trigger
            hp.Value = 50;

            Assert("ReactiveProperty", lastHp == 50 && calls == 3, $"lastHp={lastHp}, calls={calls}"); // 1 for initial, 2 for changes
            await UniTask.Yield();
        }

        private async UniTask Test_ReactiveTrigger()
        {
            using var trigger = new ReactiveTrigger().WithGate(0.1f);
            int calls = 0;

            using var sub = trigger.Subscribe(_ => calls++);

            trigger.Invoke();
            trigger.Invoke(); // Blocked by gate

            Assert("ReactiveTrigger_Gate", calls == 1, $"calls={calls}");

            await UniTask.Delay(150);
            trigger.Invoke();

            Assert("ReactiveTrigger_Gate_Wait", calls == 2, $"calls={calls}");
        }

        private struct TestMessage { public string Text; }

        private async UniTask Test_MessageBroker()
        {
            MessageBroker.ClearAll<TestMessage>();
            
            string received = "";
            using var sub = MessageBroker.Receive<TestMessage>().Subscribe(m => received = m.Text);

            MessageBroker.Publish(new TestMessage { Text = "Hello" });

            Assert("MessageBroker", received == "Hello", $"received={received}");
            await UniTask.Yield();
        }

        private async UniTask Test_FastEventHub()
        {
            int syncVal = 0;
            int asyncVal = 0;

            FastEventHubSync.ConfigureGate<GlobalSyncEvt>(0.1f, skipIfRunning: false);

            using var sub1 = FastEventHubRx.Receive<GlobalSyncEvt>().Subscribe(e => syncVal = e.Value);
            
            FastEventHubSync.Publish<GlobalSyncEvt>(e => e.Value = 42);
            FastEventHubSync.Publish<GlobalSyncEvt>(e => e.Value = 43); // Blocked

            // Async manual subscribe via ObserverContext is one way.
            // But we can also subscribe directly via FastEventHubAsync.Subscribe!
            using var sub2 = FastEventHubAsync.Subscribe<GlobalAsyncEvt>(async (e, ct) =>
            {
                await UniTask.Delay(10, cancellationToken: ct);
                asyncVal = e.Value;
            });
            
            await FastEventHubAsync.PublishAsync<GlobalAsyncEvt>(e => e.Value = 99);

            Assert("FastEventHub", syncVal == 42 && asyncVal == 99, $"sync={syncVal}, async={asyncVal}");
        }

        private event Action<int> OnTestEvent;

        private async UniTask Test_EventBinding()
        {
            int received = 0;
            using var sub = EventBinding.FromEvent<int>(h => OnTestEvent += h, h => OnTestEvent -= h)
                .Subscribe(v => received = v);

            OnTestEvent?.Invoke(123);

            Assert("EventBinding", received == 123, $"received={received}");
            await UniTask.Yield();
        }

        private async UniTask Test_RxExtensions()
        {
            var prop = new ReactiveProperty<int>(0);
            var results = new List<int>();

            using var sub = prop
                .Skip(1) // Skip initial 0
                .Where(v => v > 10)
                .Take(2)
                .Subscribe(v => results.Add(v));

            prop.Value = 5;  // Filtered by Where
            prop.Value = 15; // Passed (1)
            prop.Value = 20; // Passed (2)
            prop.Value = 25; // Blocked by Take(2)

            Assert("RxExtensions", results.Count == 2 && results[0] == 15 && results[1] == 20, $"count={results.Count}");
            await UniTask.Yield();
        }
    }
}
