using Exiled.API.Interfaces;

namespace BetterScp939
{
	public class Configs : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		public float Size { get; set; }
		public float SlowAmount { get; set; }
		public float BaseDamage { get; set; }
		public float ForceSlowDownTime { get; set; }
		public float BonusAttackMaximum { get; set; }
		public float AngerMeterMaximum { get; set; }
		public float AngerMeterDecayTime { get; set; }
		public float AngerMeterDecayValue { get; set; }
		public float StartingAnger { get; set; }
		public bool ShowSpawnBroadcastMessage { get; set; }
		public ushort SpawnBroadcastMessageDuration { get; set; }
		public string SpawnBroadcastMessage { get; set; }
		public bool ResetAngerAfterHitSlowDown { get; set; }
	}
}
