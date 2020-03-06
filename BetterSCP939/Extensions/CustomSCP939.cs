using CustomPlayerEffects;
using EXILED;
using MEC;
using Mirror;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterSCP939.Extensions
{
    public class CustomSCP939 : NetworkBehaviour
    {
        private ReferenceHub playerReferenceHub;
        private Scp207 scp207;
        private List<DamageTypes.DamageType> excludedDamages;
        private CoroutineHandle forcePositionCoroutine;
        private CoroutineHandle angerMeterDecayCoroutine;
        private const float forcePositionInterval = 0.1f;

        public float AngerMeter { get; private set; }

        private void Awake()
        {
            EXILED.Events.PlayerHurtEvent += OnPlayerHurt;
            EXILED.Events.PlayerLeaveEvent += OnPlayerLeave;

            playerReferenceHub = GetComponent<ReferenceHub>();
            scp207 = (Scp207)(typeof(PlyMovementSync).GetField("_scp207", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(playerReferenceHub.plyMovementSync));
            excludedDamages = new List<DamageTypes.DamageType>()
            {
                DamageTypes.Tesla,
                DamageTypes.Wall,
                DamageTypes.RagdollLess,
                DamageTypes.Contain,
                DamageTypes.Lure,
                DamageTypes.Recontainment,
                DamageTypes.Scp207,
                DamageTypes.None
            };
            AngerMeter = 0;
        }

        private void Start() => Scale(BetterSCP939.size);

        private void Update()
        {
            if (!scp207.Enabled) scp207.ServerEnable();
        }

        public void Destroy()
        {
            EXILED.Events.PlayerHurtEvent -= OnPlayerHurt;
            EXILED.Events.PlayerLeaveEvent -= OnPlayerLeave;

            KillCoroutines();

            Destroy(this);
        }

        public void OnPlayerHurt(ref PlayerHurtEvent ev)
        {
            if (ev.Player == playerReferenceHub && ev.DamageType == DamageTypes.Scp207) ev.Amount = 0;

            if (excludedDamages.Contains(ev.DamageType)) return;

            if (ev.Attacker == playerReferenceHub)
            {
                ev.Amount += (AngerMeter / BetterSCP939.angerMeterMaximum) * BetterSCP939.bonusAttackMaximum;

                forcePositionCoroutine = Timing.RunCoroutine(ForcePosition(BetterSCP939.forcePositionTime, forcePositionInterval), Segment.FixedUpdate);
            }
            else if (ev.Player == playerReferenceHub)
            {
                AngerMeter += ev.Amount;

                if (AngerMeter > BetterSCP939.angerMeterMaximum) AngerMeter = BetterSCP939.angerMeterMaximum;
                
                playerReferenceHub.playerStats.unsyncedArtificialHealth = (AngerMeter / BetterSCP939.angerMeterMaximum) * playerReferenceHub.playerStats.maxArtificialHealth;

                if (!angerMeterDecayCoroutine.IsRunning)
                {
                    angerMeterDecayCoroutine = Timing.RunCoroutine(AngerMeterDecay(BetterSCP939.angerMeterDecayTime), Segment.FixedUpdate);
                }
            }

        }

        public void OnPlayerLeave(PlayerLeaveEvent ev)
        {
            if (ev.Player == playerReferenceHub) Destroy();
        }

        private IEnumerator<float> ForcePosition(float totalWaitTime, float interval)
        {
            var oldPosition = playerReferenceHub.plyMovementSync.RealModelPosition;
            var waitedTime = 0f;

            while (waitedTime < totalWaitTime)
            {
                playerReferenceHub.plyMovementSync.TargetForcePosition(playerReferenceHub.nicknameSync.connectionToClient, oldPosition);

                waitedTime += interval;

                yield return Timing.WaitForSeconds(interval);
            }
        }

        private IEnumerator<float> AngerMeterDecay(float waitTime)
        {
            while (AngerMeter > 0)
            {
                playerReferenceHub.playerStats.unsyncedArtificialHealth = (AngerMeter / BetterSCP939.angerMeterMaximum) * playerReferenceHub.playerStats.maxArtificialHealth;

                yield return Timing.WaitForSeconds(waitTime);

                AngerMeter -= BetterSCP939.angerMeterDecayValue;

                if (AngerMeter < 0) AngerMeter = 0;
            }
        }

        private void KillCoroutines()
        {
            if (forcePositionCoroutine.IsRunning) Timing.KillCoroutines(forcePositionCoroutine);
            if (angerMeterDecayCoroutine.IsRunning) Timing.KillCoroutines(angerMeterDecayCoroutine);
        }

        private void Scale(float size)
        {
            var identity = GetComponent<NetworkIdentity>();

            gameObject.transform.localScale = Vector3.one * size;

            var destroyMessage = new ObjectDestroyMessage
            {
                netId = identity.netId
            };

            foreach (var player in PlayerManager.players)
            {
                if (player == gameObject) continue;

                var playerCon = player.GetComponent<NetworkIdentity>().connectionToClient;

                playerCon.Send(destroyMessage, 0);

                var sendSpawnMessageMethod = typeof(NetworkServer).GetMethod("SendSpawnMessage", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);

                sendSpawnMessageMethod?.Invoke(null, new object[] { identity, playerCon });
            }
        }
    }
}
