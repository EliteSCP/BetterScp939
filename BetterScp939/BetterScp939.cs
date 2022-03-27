namespace BetterScp939
{
    using Components;
    using Events;
    using Exiled.API.Features;
    using System;
    using PlayerEvents = Exiled.Events.Handlers.Player;

    public class BetterScp939 : Plugin<Config>
    {
        private static readonly BetterScp939 InstanceValue = new BetterScp939();
        private PlayerHandler playerHandler;

        public override Version RequiredExiledVersion { get; } = new Version(5, 0, 0);

        private BetterScp939()
        {
        }

        public static BetterScp939 Instance => InstanceValue;

        public override void OnEnabled()
        {
            RegisterEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();

            foreach (var player in Player.Get(Team.SCP))
            {
                if (player.ReferenceHub.TryGetComponent<BetterScp939Controller>(out var customScp939))
                    customScp939.Destroy();
            }

            base.OnDisabled();
        }

        internal void RegisterEvents()
        {
            playerHandler = new PlayerHandler();

            PlayerEvents.ChangingRole += playerHandler.OnSetClass;
        }

        internal void UnregisterEvents()
        {
            PlayerEvents.ChangingRole -= playerHandler.OnSetClass;

            playerHandler = null;
        }
    }
}
