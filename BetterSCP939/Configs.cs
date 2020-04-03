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

		/// <summary>
		/// Reloads the plugin configs.
		/// </summary>
		public static void Reload()
		{
			isEnabled = Plugin.Config.GetBool("b939_enabled", true);
			size = Plugin.Config.GetFloat("b939_size", 0.75f);
			slowAmount = Plugin.Config.GetFloat("b939_slow_amount", 10f);
			baseDamage = Plugin.Config.GetFloat("b939_base_damage", 40f);
			forceSlowDownTime = Plugin.Config.GetFloat("b939_force_slow_down_time", 3f);
			bonusAttackMaximum = Plugin.Config.GetFloat("b939_bonus_attack_maximum", 150f);
			angerMeterMaximum = Plugin.Config.GetFloat("b939_anger_meter_maximum", 500f);
			angerMeterDecayTime = Plugin.Config.GetFloat("b939_anger_meter_decay_time", 1f);
			angerMeterDecayValue = Plugin.Config.GetFloat("b939_anger_meter_decay_value", 3f);
			startingAnger = Plugin.Config.GetFloat("b939_starting_anger", 0);
		}
	}
}
