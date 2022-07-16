﻿namespace BetterScp939.Components
{
    using CustomPlayerEffects;
    using Exiled.API.Enums;
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
        private List<DamageType> excludedDamages;
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
            excludedDamages = new List<DamageType>()
            {
                DamageType.Tesla,
                DamageType.Crushed,
                DamageType.Warhead,
                DamageType.Custom,
                DamageType.FemurBreaker,
                DamageType.Recontainment,
                DamageType.Scp207,
                DamageType.Unknown
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
            if (player == null || !player.Role.Type.Is939())
            {
                Destroy();
                return;
            }

            if (!scp207.IsEnabled && !sinkHole.IsEnabled && BetterScp939.Instance.Config.IsFasterThanHumans)
                player.EnableEffect<Scp207>();
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.IsAllowed && ev.Target == player)
            {
                if (ev.Handler.Type != DamageType.Scp207)
                    player.Health += ev.Amount < 0 ? -9999999f : -ev.Amount;

                if (!excludedDamages.Contains(ev.Handler.Type))
                {
                    AngerMeter += ev.Amount;
                }
                else
                {
                    ev.Amount = 0;
                    return;
                }

                ev.Amount = 0;

                if (AngerMeter > BetterScp939.Instance.Config.AngerMeterMaximum)
                    AngerMeter = BetterScp939.Instance.Config.AngerMeterMaximum;

                player.ArtificialHealth = (byte)(AngerMeter / BetterScp939.Instance.Config.AngerMeterMaximum * player.MaxArtificialHealth);

                if (!angerMeterDecayCoroutine.IsRunning)
                    angerMeterDecayCoroutine = Timing.RunCoroutine(AngerMeterDecay(BetterScp939.Instance.Config.AngerMeterDecayTime), Segment.FixedUpdate);
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

            if (player == null)
                return;

            scp207.IsEnabled = false;
            sinkHole.IsEnabled = false;

            AngerMeter = 0;

            player.Scale = new Vector3(1, 1, 1);
            player.ArtificialHealth = 0;
        }

        public void Destroy()
        {
            try
            {
                Destroy(this);
            }
            catch (Exception exception)
            {
                Log.Error($"Error, cannot destroy: {exception}");
            }
        }

        private void RegisterEvents() => PlayerEvents.Hurting += OnHurting;

        private void UnregisterEvents() => PlayerEvents.Hurting -= OnHurting;

        private IEnumerator<float> ForceSlowDown(float totalWaitTime, float interval)
        {
            var waitedTime = 0f;

            scp207.IsEnabled = false;

            while (waitedTime < totalWaitTime)
            {
                if (!sinkHole.IsEnabled && BetterScp939.Instance.Config.ShouldGetSlowed)
                    player.EnableEffect<SinkHole>();

                waitedTime += interval;

                yield return Timing.WaitForSeconds(interval);
            }

            sinkHole.IsEnabled = false;

            if (BetterScp939.Instance.Config.ResetAngerAfterHitSlowDown)
                AngerMeter = player.ArtificialHealth = 0;
        }

        private IEnumerator<float> AngerMeterDecay(float waitTime)
        {
            while (AngerMeter > 0)
            {
                player.ArtificialHealth = (byte)(AngerMeter / BetterScp939.Instance.Config.AngerMeterMaximum * player.MaxArtificialHealth);

                yield return Timing.WaitForSeconds(waitTime);

                AngerMeter -= BetterScp939.Instance.Config.AngerMeterDecayValue;

                if (AngerMeter < 0)
                    AngerMeter = 0;
            }
        }

        private void KillCoroutines()
        {
            if (forceSlowDownCoroutine.IsRunning)
                Timing.KillCoroutines(forceSlowDownCoroutine);

            if (angerMeterDecayCoroutine.IsRunning)
                Timing.KillCoroutines(angerMeterDecayCoroutine);
        }
    }
}
