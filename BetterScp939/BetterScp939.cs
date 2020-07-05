using BetterScp939.Components;
using BetterScp939.Events;
using EXILED;
using EXILED.Extensions;
using System;
using System.Reflection;

namespace BetterScp939
{
    public class BetterScp939 : Plugin
	{
		internal PlayerHandler PlayerHandler { get; set; }
		internal ExiledVersion ExiledVersion { get; private set; } = new ExiledVersion() { Major = 1, Minor = 12, Patch = 26 };
		internal Version Version { get; private set; } = Assembly.GetExecutingAssembly().GetName().Version;

		public override string getName { get; }

		public BetterScp939() => getName = $"BetterSCP939 {Version.Major}.{Version.Minor}.{Version.Build}";

		public override void OnEnable()
		{
			if (Version.Parse($"{EventPlugin.Version.Major}.{EventPlugin.Version.Minor}.{EventPlugin.Version.Patch}") < Version.Parse($"{ExiledVersion.Major}.{ExiledVersion.Minor}.{ExiledVersion.Patch}"))
			{
				Log.Warn($"You're running an older version of EXILED ({EventPlugin.Version.Major}.{EventPlugin.Version.Minor}.{EventPlugin.Version.Patch}), the plugin may not be compatible with it! Recommended version: {ExiledVersion.Major}.{ExiledVersion.Minor}.{ExiledVersion.Patch}");
			}

			Configs.Reload();

			if (!Configs.isEnabled) return;

			RegisterEvents();

			Log.Info($"{getName} has been enabled!");
		}

		public override void OnDisable()
		{
			UnregisterEvents();

			foreach (var player in Team.SCP.GetHubs())
			{
				if (player.TryGetComponent<CustomScp939>(out var customSCP939)) customSCP939.Destroy();
			}

			Log.Info($"{getName} has been disabled!");
		}

		public override void OnReload() => Log.Info($"{getName} has been reloaded!");

		internal void RegisterEvents()
		{
			PlayerHandler = new PlayerHandler();

			EXILED.Events.SetClassEvent += PlayerHandler.OnSetClass;
		}

		internal void UnregisterEvents()
		{
			EXILED.Events.SetClassEvent -= PlayerHandler.OnSetClass;

			PlayerHandler = null;
		}
	}
}
