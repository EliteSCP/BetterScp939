using BetterScp939.Components;
using EXILED;
using EXILED.Extensions;

namespace BetterScp939.Events
{
	public class PlayerHandler
	{
		public void OnSetClass(SetClassEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player?.GetUserId())) return;

			if (ev.Role.Is939())
			{
				if (ev.Player.TryGetComponent(out CustomScp939 customSCP939)) customSCP939.Destroy();

				ev.Player.gameObject.AddComponent<CustomScp939>();

				return;
			}
		}
	}
}
