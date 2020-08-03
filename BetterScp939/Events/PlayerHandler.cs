using BetterScp939.Components;
using Exiled.Events.EventArgs;

namespace BetterScp939.Events
{
    public class PlayerHandler
    {
        public void OnSetClass(ChangingRoleEventArgs ev)
        {
            if (string.IsNullOrEmpty(ev.Player?.UserId))
                return;

            if (ev.NewRole.Is939())
            {
                if (ev.Player.ReferenceHub.TryGetComponent(out BetterScp939Controller customScp939))
                    customScp939.Destroy();

                ev.Player.ReferenceHub.gameObject.AddComponent<BetterScp939Controller>();
            }
        }
    }
}
