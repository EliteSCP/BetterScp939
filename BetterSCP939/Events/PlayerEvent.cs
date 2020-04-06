using BetterSCP939.Components;
using EXILED;
using EXILED.Extensions;

namespace BetterSCP939.Events
{
	public class PlayerEvent
	{
		public void OnSetClass(SetClassEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.GetUserId())) return;

			if (ev.Role.Is939())
			{
				if (ev.Player.TryGetComponent(out CustomSCP939 customSCP939)) customSCP939.Destroy();

				ev.Player.gameObject.AddComponent<CustomSCP939>();

				return;
			}
		}
	}
}
