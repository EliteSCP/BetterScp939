namespace BetterScp939
{
    using Components;
    using Events;
    using Exiled.API.Features;
    using System;
    using System.Linq;
    using PlayerEvents = Exiled.Events.Handlers.Player;
    public class BetterScp939 : Plugin<Config>
    {
        private static readonly Lazy<BetterScp939> LazyInstance = new Lazy<BetterScp939>(() => new BetterScp939());

        internal PlayerHandler PlayerHandler { get; set; }

        public static BetterScp939 Instance => LazyInstance.Value;

        private BetterScp939()
        {
        }

        public override void OnEnabled() => RegisterEvents();

        public override void OnDisabled()
        {
            UnregisterEvents();

            foreach (var player in Player.List.Where(p => p.Team == Team.SCP))
            {
                if (player.ReferenceHub.TryGetComponent<BetterScp939Controller>(out var customScp939))
                    customScp939.Destroy();
            }
        }

        internal void RegisterEvents()
        {
            PlayerHandler = new PlayerHandler();

            PlayerEvents.ChangingRole += PlayerHandler.OnSetClass;
        }

        internal void UnregisterEvents()
        {
            PlayerEvents.ChangingRole -= PlayerHandler.OnSetClass;

            PlayerHandler = null;
        }
    }
}
