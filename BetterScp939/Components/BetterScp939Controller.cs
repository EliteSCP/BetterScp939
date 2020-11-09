namespace BetterScp939.Components
{
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using PlayerEvents = Exiled.Events.Handlers.Player;

    public class BetterScp939Controller : MonoBehaviour
    {
        private Player player;
        private Scp207 scp207;
        private SinkHole sinkHole;
        private List<DamageTypes.DamageType> excludedDamages;
        private CoroutineHandle forceSlowDownCoroutine;
        private CoroutineHandle angerMeterDecayCoroutine;
        private const float forceSlowDownInterval = 0.1f;

        public float AngerMeter { get; private set; }

        private void Awake()
        {
            RegisterEvents();

            player = Player.Get(gameObject);
            scp207 = player.ReferenceHub.playerEffectsController.GetEffect<Scp207>();
            sinkHole = player.ReferenceHub.playerEffectsController.GetEffect<SinkHole>();
            excludedDamages = new List<DamageTypes.DamageType>()
            {
                DamageTypes.Tesla,
                DamageTypes.Wall,
                DamageTypes.Nuke,
                DamageTypes.RagdollLess,
                DamageTypes.Contain,
                DamageTypes.Lure,
                DamageTypes.Recontainment,
                DamageTypes.Scp207,
                DamageTypes.None
            };
            AngerMeter = BetterScp939.Instance.Config.StartingAnger;
            sinkHole.slowAmount = BetterScp939.Instance.Config.SlowAmount;
        }

        private void Start()
        {
            player.Scale *= BetterScp939.Instance.Config.Size;

            if (BetterScp939.Instance.Config.ShowSpawnBroadcastMessage)
            {
                player.ClearBroadcasts();
                player.Broadcast(BetterScp939.Instance.Config.SpawnBroadcastMessageDuration, string.Format(BetterScp939.Instance.Config.SpawnBroadcastMessage, BetterScp939.Instance.Config.ForceSlowDownTime));
            }
        }

        private void Update()
        {
            if (player == null || !player.Role.Is939())
            {
                Destroy();
                return;
            }

            if (!scp207.Enabled && !sinkHole.Enabled && BetterScp939.Instance.Config.IsFasterThanHumans) player.ReferenceHub.playerEffectsController.EnableEffect<Scp207>();
        }

        public void OnPlayerHurt(HurtingEventArgs ev)
        {
            if (ev.Target == player)
            {
                if (ev.DamageType != DamageTypes.Scp207) player.Health += ev.Amount < 0 ? -9999999f : -ev.Amount;

                if (!excludedDamages.Contains(ev.DamageType)) AngerMeter += ev.Amount;
                else
                {
                    ev.Amount = 0;
                    return;
                }

                ev.Amount = 0;

                if (AngerMeter > BetterScp939.Instance.Config.AngerMeterMaximum) AngerMeter = BetterScp939.Instance.Config.AngerMeterMaximum;

                player.AdrenalineHealth = (byte)(AngerMeter / BetterScp939.Instance.Config.AngerMeterMaximum * player.MaxAdrenalineHealth);

                if (!angerMeterDecayCoroutine.IsRunning)
                {
                    angerMeterDecayCoroutine = Timing.RunCoroutine(AngerMeterDecay(BetterScp939.Instance.Config.AngerMeterDecayTime), Segment.FixedUpdate);
                }
            }
            else if (ev.Attacker == player && ev.Amount > 0)
            {
                ev.Amount = BetterScp939.Instance.Config.BaseDamage + (AngerMeter / BetterScp939.Instance.Config.AngerMeterMaximum) * BetterScp939.Instance.Config.BonusAttackMaximum;

                forceSlowDownCoroutine = Timing.RunCoroutine(ForceSlowDown(BetterScp939.Instance.Config.ForceSlowDownTime, forceSlowDownInterval), Segment.FixedUpdate);
            }
        }

        private void OnDestroy() => PartiallyDestroy();

        public void PartiallyDestroy()
        {
            UnregisterEvents();
            KillCoroutines();

            if (player == null) return;

            scp207.ServerDisable();
            sinkHole.ServerDisable();

            AngerMeter = 0;

            player.Scale = new Vector3(1, 1, 1);
            player.AdrenalineHealth = 0;
        }

        public void Destroy()
        {
            try
            {
                Destroy(this);
            }
            catch (Exception exception)
            {
                Log.Error($"Cannot destroy, IsReferenceHubNull: {player == null} Error: {exception}");
            }
        }

        private void RegisterEvents() => PlayerEvents.Hurting += OnPlayerHurt;

        private void UnregisterEvents() => PlayerEvents.Hurting -= OnPlayerHurt;

        private IEnumerator<float> ForceSlowDown(float totalWaitTime, float interval)
        {
            var waitedTime = 0f;

            scp207.ServerDisable();

            while (waitedTime < totalWaitTime)
            {
                if (!sinkHole.Enabled && BetterScp939.Instance.Config.ShouldGetSlowed) player.ReferenceHub.playerEffectsController.EnableEffect<SinkHole>();

                waitedTime += interval;

                yield return Timing.WaitForSeconds(interval);
            }

            sinkHole.ServerDisable();

            if (BetterScp939.Instance.Config.ResetAngerAfterHitSlowDown)
                AngerMeter = player.AdrenalineHealth = 0;
        }

        private IEnumerator<float> AngerMeterDecay(float waitTime)
        {
            while (AngerMeter > 0)
            {
                player.AdrenalineHealth = (byte)(AngerMeter / BetterScp939.Instance.Config.AngerMeterMaximum * player.MaxAdrenalineHealth);

                yield return Timing.WaitForSeconds(waitTime);

                AngerMeter -= BetterScp939.Instance.Config.AngerMeterDecayValue;

                if (AngerMeter < 0) AngerMeter = 0;
            }
        }

        private void KillCoroutines()
        {
            if (forceSlowDownCoroutine.IsRunning) Timing.KillCoroutines(forceSlowDownCoroutine);
            if (angerMeterDecayCoroutine.IsRunning) Timing.KillCoroutines(angerMeterDecayCoroutine);
        }
    }
}
