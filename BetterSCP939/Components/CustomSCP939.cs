using CustomPlayerEffects;
using EXILED;
using EXILED.Extensions;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace BetterSCP939.Components
{
	public class CustomSCP939 : MonoBehaviour
	{
		private ReferenceHub playerReferenceHub;
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

			playerReferenceHub = GetComponent<ReferenceHub>();
			scp207 = playerReferenceHub.plyMovementSync._scp207;
			sinkHole = playerReferenceHub.plyMovementSync._sinkhole;
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
			AngerMeter = Configs.startingAnger;
			sinkHole.slowAmount = Configs.slowAmount;
		}

		private void Start() => playerReferenceHub.SetScale(Configs.size);

		private void Update()
		{
			if (playerReferenceHub == null || !playerReferenceHub.GetRole().Is939())
			{
				Destroy();
				return;
			}

			if (!scp207.Enabled && !sinkHole.Enabled) scp207.ServerEnable();
		}
		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ev.Player == playerReferenceHub)
			{
				if (ev.DamageType != DamageTypes.Scp207) playerReferenceHub.AddHealth(ev.Amount < 0 ? -9999999f : -ev.Amount);

				if (!excludedDamages.Contains(ev.DamageType)) AngerMeter += ev.Amount;
				else
				{
					ev.Amount = 0;
					return;
				}

				ev.Amount = 0;

				if (AngerMeter > Configs.angerMeterMaximum) AngerMeter = Configs.angerMeterMaximum;

				playerReferenceHub.playerStats.unsyncedArtificialHealth = (byte)(AngerMeter / Configs.angerMeterMaximum * playerReferenceHub.GetMaxAdrenalineHealth());

				if (!angerMeterDecayCoroutine.IsRunning)
				{
					angerMeterDecayCoroutine = Timing.RunCoroutine(AngerMeterDecay(Configs.angerMeterDecayTime), Segment.FixedUpdate);
				}
			}
			else if (ev.Attacker == playerReferenceHub && ev.Amount > 0)
			{
				ev.Amount = Configs.baseDamage + (AngerMeter / Configs.angerMeterMaximum) * Configs.bonusAttackMaximum;

				forceSlowDownCoroutine = Timing.RunCoroutine(ForceSlowDown(Configs.forceSlowDownTime, forceSlowDownInterval), Segment.FixedUpdate);
			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player == playerReferenceHub) PartiallyDestroy();
		}

		public void OnRoundRestart() => PartiallyDestroy();

		public void PartiallyDestroy()
		{
			UnregisterEvents();
			KillCoroutines();
		}

		public void Destroy()
		{
			PartiallyDestroy();

			scp207.ServerDisable();
			sinkHole.ServerDisable();

			AngerMeter = 0;

			playerReferenceHub.SetScale(1);
			playerReferenceHub.playerStats.unsyncedArtificialHealth = 0;

			Destroy(this);
		}

		private void RegisterEvents()
		{
			EXILED.Events.PlayerHurtEvent += OnPlayerHurt;
			EXILED.Events.PlayerLeaveEvent += OnPlayerLeave;
			EXILED.Events.RoundRestartEvent += OnRoundRestart;
		}

		private void UnregisterEvents()
		{
			EXILED.Events.PlayerHurtEvent -= OnPlayerHurt;
			EXILED.Events.PlayerLeaveEvent -= OnPlayerLeave;
			EXILED.Events.RoundRestartEvent -= OnRoundRestart;
		}

		private IEnumerator<float> ForceSlowDown(float totalWaitTime, float interval)
		{
			var waitedTime = 0f;

			scp207.ServerDisable();

			while (waitedTime < totalWaitTime)
			{
				if (!sinkHole.Enabled) sinkHole.ServerEnable();

				waitedTime += interval;

				yield return Timing.WaitForSeconds(interval);
			}

			sinkHole.ServerDisable();
		}

		private IEnumerator<float> AngerMeterDecay(float waitTime)
		{
			while (AngerMeter > 0)
			{
				playerReferenceHub.playerStats.unsyncedArtificialHealth = (byte)(AngerMeter / Configs.angerMeterMaximum * playerReferenceHub.GetMaxAdrenalineHealth());

				yield return Timing.WaitForSeconds(waitTime);

				AngerMeter -= Configs.angerMeterDecayValue;

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
