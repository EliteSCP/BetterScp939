using static EXILED.Plugin;

namespace BetterScp939
{
	internal static class Configs
	{
		public static bool isEnabled;
		public static float size;
		public static float slowAmount;
		public static float baseDamage;
		public static float forceSlowDownTime;
		public static float bonusAttackMaximum;
		public static float angerMeterMaximum;
		public static float angerMeterDecayTime;
		public static float angerMeterDecayValue;
		public static float startingAnger;
		public static bool showSpawnBroadcastMessage;
		public static ushort spawnBroadcastMessageDuration;
		public static string spawnBroadcastMessage;
		public static bool resetAngerAfterHitSlowDown;

		/// <summary>
		/// Reloads plugin configs.
		/// </summary>
		public static void Reload()
		{
			isEnabled = Config.GetBool("b939_enabled", true);
			size = Config.GetFloat("b939_size", 0.75f);
			slowAmount = Config.GetFloat("b939_slow_amount", 10f);
			forceSlowDownTime = Config.GetFloat("b939_force_slow_down_time", 3f);
			baseDamage = Config.GetFloat("b939_base_damage", 40f);
			bonusAttackMaximum = Config.GetFloat("b939_bonus_attack_maximum", 150f);
			angerMeterMaximum = Config.GetFloat("b939_anger_meter_maximum", 500f);
			angerMeterDecayTime = Config.GetFloat("b939_anger_meter_decay_time", 1f);
			angerMeterDecayValue = Config.GetFloat("b939_anger_meter_decay_value", 3f);
			startingAnger = Config.GetFloat("b939_starting_anger", 0);
			showSpawnBroadcastMessage = Config.GetBool("b939_show_spawn_broadcast_message");
			spawnBroadcastMessageDuration = Config.GetUShort("b939_spawn_broadcast_message_duration", 15);
			spawnBroadcastMessage = Config.GetString("b939_spawn_broadcast_message", "<size=20><color=#00FFFF>You've spawned as an upgraded version of <color=#FF0000>SCP-939</color>!\nYou're faster than humans, your <color=#FF0000>anger</color> will increase after taking damage from them.\nMore anger means more damage inflicted to humans.\nAfter <color=#FF0000>hurting</color> someone, you'll get slowed down for <color=#FF0000>{0}</color> seconds</color></size>");
			resetAngerAfterHitSlowDown = Config.GetBool("b939_reset_anger_after_hit_slow_down");
		}
	}
}
