using BetterSCP939.Events;
using EXILED;
using Harmony;
using System.Reflection;

namespace BetterSCP939
{
    public class BetterSCP939 : Plugin
    {
        internal RoundEvent RoundEvent { get; set; }
        internal PlayerEvent PlayerEvent { get; set; }

        #region Configs

        internal bool isEnabled;

        internal static float size;
        internal static float forcePositionTime;
        internal static float bonusAttackMaximum;
        internal static float angerMeterMaximum;
        internal static float angerMeterDecayTime;
        internal static float angerMeterDecayValue;

        #endregion

        public override string getName => "BetterSCP939";

        /// <summary>
        /// Fired when the plugin has been disabled.
        /// </summary>
        public override void OnDisable()
        {
            UnregisterEvents();

            Log.Info($"{getName} has been disabled!");
        }

        /// <summary>
        /// Fired when the plugin has been enabled.
        /// </summary>
        public override void OnEnable()
        {
            LoadConfigs();

            if (!isEnabled) return;

            RegisterEvents();

            HarmonyInstance.Create("com.iopietro.better.scp939").PatchAll(Assembly.GetExecutingAssembly());

            Log.Info($"{getName} has been enabled!");
        }

        /// <summary>
        /// Fired when the plugin has been reloaded.
        /// </summary>
        public override void OnReload()
        {
            UnregisterEvents();
            RegisterEvents();
            LoadConfigs();

            Log.Info($"{getName} has been reloaded!");
        }

        /// <summary>
        /// Registers the plugin events.
        /// </summary>
        internal void RegisterEvents()
        {
            RoundEvent = new RoundEvent(this);
            PlayerEvent = new PlayerEvent(this);

            EXILED.Events.WaitingForPlayersEvent += RoundEvent.OnWaitingForPlayers;

            EXILED.Events.SetClassEvent += PlayerEvent.OnSetClass;
        }

        /// <summary>
        /// Unregisters the plugin events.
        /// </summary>
        internal void UnregisterEvents()
        {
            EXILED.Events.WaitingForPlayersEvent -= RoundEvent.OnWaitingForPlayers;

            EXILED.Events.SetClassEvent -= PlayerEvent.OnSetClass;

            RoundEvent = null;
            PlayerEvent = null;
        }

        /// <summary>
        /// Loads the plugin configs.
        /// </summary>
        internal void LoadConfigs()
        {
            isEnabled = Config.GetBool("b939_enabled", true);

            size = Config.GetFloat("b939_size", 0.75f);
            forcePositionTime = Config.GetFloat("b939_force_position_time", 2.5f);
            bonusAttackMaximum = Config.GetFloat("b939_bonus_attack_maximum", 150f);
            angerMeterMaximum = Config.GetFloat("b939_anger_meter_maximum", 500f);
            angerMeterDecayTime = Config.GetFloat("b939_anger_meter_decay_time", 1f);
            angerMeterDecayValue = Config.GetFloat("b939_anger_meter_decay_value", 3f);
        }
    }
}
