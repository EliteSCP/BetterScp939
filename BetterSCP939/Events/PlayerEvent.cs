using BetterSCP939.Extensions;
using EXILED;

namespace BetterSCP939.Events
{
    public class PlayerEvent
    {
        private readonly BetterSCP939 pluginInstance;

        public PlayerEvent(BetterSCP939 pluginInstance) => this.pluginInstance = pluginInstance;

        public void OnPlayerHurt(ref PlayerHurtEvent ev)
        {

        }

        public void OnSetClass(SetClassEvent ev)
        {
            ev.Player.gameObject.AddComponent<CustomSCP939>();
        }
    }
}
