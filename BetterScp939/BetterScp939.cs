using BetterScp939.Components;
using BetterScp939.Events;
using System;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;
using PlayerEvents = Exiled.Events.Handlers.Player;

namespace BetterScp939
{
    public class BetterScp939 : Plugin<Configs>
	{
		internal PlayerHandler PlayerHandler { get; set; }
		public static BetterScp939 singleton;

		public override string Author { get; } = "iopietro";
		public override Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;
		public override Version RequiredExiledVersion { get; } = new Version(2, 0, 1);
		public override string Prefix { get; } = "B939";
		public override string Name { get; } = "Better SCP-939";

		public override void OnEnabled()
		{
			singleton = this;

			RegisterEvents();
		}

		public override void OnDisabled()
		{
			UnregisterEvents();

			foreach (var player in Player.List.Where(p => p.Team == Team.SCP))
			{
				if (player.ReferenceHub.TryGetComponent<CustomScp939>(out var customScp939)) 
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
