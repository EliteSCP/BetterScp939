using BetterSCP939.Events;
using BetterSCP939.Extensions;
using EXILED;
using EXILED.Extensions;
using Harmony;
using System.Reflection;

namespace BetterSCP939
{
	public class BetterSCP939 : Plugin
	{
		private int patchesCounter;
		private HarmonyInstance harmonyInstance;

		internal PlayerEvent PlayerEvent { get; set; }
		internal ExiledVersion ExiledVersion { get; private set; } = new ExiledVersion() { Major = 1, Minor = 9, Patch = 8 };

		public override string getName => "BetterSCP939";

		/// <summary>
		/// Fired when the plugin has been enabled.
		/// </summary>
		public override void OnEnable()
		{
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
