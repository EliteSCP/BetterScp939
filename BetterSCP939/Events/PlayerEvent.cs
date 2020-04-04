using BetterSCP939.Components;
using EXILED;
using EXILED.Extensions;

namespace BetterSCP939.Events
{
	public class PlayerEvent
	{
		private readonly BetterSCP939 pluginInstance;

		public PlayerEvent(BetterSCP939 pluginInstance) => this.pluginInstance = pluginInstance;

		public void OnSetClass(SetClassEvent ev)
		{
			if (ev.Player.GetNickname() == "Dedicated Server") return;

			if (ev.Role.Is939())
			{
				var betterSCP939 = ev.Player.gameObject.GetComponent<CustomSCP939>();

				if (betterSCP939 != null) betterSCP939.Destroy();

				ev.Player.gameObject.AddComponent<CustomSCP939>();

				return;
			}
		}
	}
}
