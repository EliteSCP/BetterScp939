using BetterSCP939.Events;
using EXILED;

namespace BetterSCP939
{
    public class BetterSCP939 : Plugin
    {
        internal RoundEvent RoundEvent { get; set; }
        internal PlayerEvent PlayerEvent { get; set; }

        #region Configs

        internal bool isEnabled;

        #endregion

        public override string getName => "BetterSCP939";

        /// <summary>
        /// Fired when the plugin has been disabled.
        /// </summary>
        public override void OnDisable() => UnregisterEvents();

        /// <summary>
        /// Fired when the plugin has been enabled.
        /// </summary>
        public override void OnEnable()
        {
            LoadConfigs();

            if (!isEnabled) return;

            RegisterEvents();
        }

        /// <summary>
        /// Fired when the plugin has been reloaded.
        /// </summary>
        public override void OnReload()
        {
            UnregisterEvents();
            RegisterEvents();
            LoadConfigs();
        }

        /// <summary>
        /// Registers the plugin events.
        /// </summary>
        internal void RegisterEvents()
        {
            RoundEvent = new RoundEvent(this);
            PlayerEvent = new PlayerEvent(this);

            EXILED.Events.WaitingForPlayersEvent += RoundEvent.OnWaitingForPlayers;

            EXILED.Events.PlayerHurtEvent += PlayerEvent.OnPlayerHurt;
            EXILED.Events.SetClassEvent += PlayerEvent.OnSetClass;
        }

        /// <summary>
        /// Unregisters the plugin events.
        /// </summary>
        internal void UnregisterEvents()
        {
            EXILED.Events.WaitingForPlayersEvent -= RoundEvent.OnWaitingForPlayers;

            EXILED.Events.PlayerHurtEvent -= PlayerEvent.OnPlayerHurt;
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


        }
    }
}
