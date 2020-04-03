using BetterSCP939.Events;
using BetterSCP939.Components;
using EXILED;
using EXILED.Extensions;
using Harmony;
using System.Reflection;
using System;

namespace BetterSCP939
{
	public class BetterSCP939 : Plugin
	{
		private int patchesCounter;
		private HarmonyInstance harmonyInstance;

		internal PlayerEvent PlayerEvent { get; set; }
		internal ExiledVersion ExiledVersion { get; private set; } = new ExiledVersion() { Major = 1, Minor = 9, Patch = 10 };

		public override string getName => "BetterSCP939";

		/// <summary>
		/// Fired when the plugin has been enabled.
		/// </summary>
		public override void OnEnable()
		{
			if (Version.Parse($"{EventPlugin.Version.Major}.{EventPlugin.Version.Minor}.{EventPlugin.Version.Patch}") < Version.Parse($"{ExiledVersion.Major}.{ExiledVersion.Minor}.{ExiledVersion.Patch}"))
			{
				Log.Warn($"You're running an older version of EXILED ({EventPlugin.Version.Major}.{EventPlugin.Version.Minor}.{EventPlugin.Version.Patch}), the plugin may not be compatible with it! Recommended version: {ExiledVersion.Major}.{ExiledVersion.Minor}.{ExiledVersion.Patch}");
			}

			Configs.Reload();

			if (!Configs.isEnabled) return;

			RegisterEvents();

			harmonyInstance = HarmonyInstance.Create($"com.iopietro.betterscp939.{patchesCounter++}");
			harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

			Log.Info($"{getName} has been enabled!");
		}

		/// <summary>
		/// Fired when the plugin has been disabled.
		/// </summary>
		public override void OnDisable()
		{
			UnregisterEvents();

			harmonyInstance.UnpatchAll();

			foreach (var player in Player.GetHubs())
			{
				if (player.TryGetComponent<CustomSCP939>(out var customSCP939)) customSCP939.Destroy();
			}

			Log.Info($"{getName} has been disabled!");
		}

		/// <summary>
		/// Fired when the plugin has been reloaded.
		/// </summary>
		public override void OnReload() => Log.Info($"{getName} has been reloaded!");

		/// <summary>
		/// Registers the plugin events.
		/// </summary>
		internal void RegisterEvents()
		{
			PlayerEvent = new PlayerEvent(this);

			EXILED.Events.SetClassEvent += PlayerEvent.OnSetClass;
		}

		/// <summary>
		/// Unregisters the plugin events.
		/// </summary>
		internal void UnregisterEvents()
		{
			EXILED.Events.SetClassEvent -= PlayerEvent.OnSetClass;

			PlayerEvent = null;
		}
	}
}
