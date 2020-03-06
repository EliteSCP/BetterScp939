using BetterSCP939.Extensions;
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

            var betterSCP939 = ev.Player.gameObject.GetComponent<CustomSCP939>();

            if (ev.Role == RoleType.Scp93953 || ev.Role == RoleType.Scp93989)
            {
                if (betterSCP939 != null) betterSCP939.Destroy();

                ev.Player.gameObject.AddComponent<CustomSCP939>();

                return;
            }
            else if (betterSCP939 != null) betterSCP939.Destroy();
        }
    }
}
