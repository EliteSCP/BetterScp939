using EXILED;

namespace BetterSCP939
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
		public static uint spawnBroadcastMessageDuration;
		public static string spawnBroadcastMessage;

		/// <summary>
		/// Reloads plugin configs.
		/// </summary>
		public static void Reload()
		{
			isEnabled = Plugin.Config.GetBool("b939_enabled", true);
			size = Plugin.Config.GetFloat("b939_size", 0.75f);
			slowAmount = Plugin.Config.GetFloat("b939_slow_amount", 10f);
			forceSlowDownTime = Plugin.Config.GetFloat("b939_force_slow_down_time", 3f);
			baseDamage = Plugin.Config.GetFloat("b939_base_damage", 40f);
			bonusAttackMaximum = Plugin.Config.GetFloat("b939_bonus_attack_maximum", 150f);
			angerMeterMaximum = Plugin.Config.GetFloat("b939_anger_meter_maximum", 500f);
			angerMeterDecayTime = Plugin.Config.GetFloat("b939_anger_meter_decay_time", 1f);
			angerMeterDecayValue = Plugin.Config.GetFloat("b939_anger_meter_decay_value", 3f);
			startingAnger = Plugin.Config.GetFloat("b939_starting_anger", 0);
			showSpawnBroadcastMessage = Plugin.Config.GetBool("b939_show_spawn_broadcast_message");
			spawnBroadcastMessageDuration = Plugin.Config.GetUInt("b939_spawn_broadcast_message_duration", 15);
			spawnBroadcastMessage = Plugin.Config.GetString("b939_spawn_broadcast_message", "<size=20><color=#00FFFF>You've spawned as an upgraded version of <color=#FF0000>SCP-939</color>!\nYou're faster than humans, your <color=#FF0000>anger</color> will increase after taking damage from them.\nMore anger means more damage inflicted to humans.\nAfter <color=#FF0000>hurting</color> someone, you'll get slowed down for <color=#FF0000>{0}</color> seconds</color></size>");
		}
	}
}
