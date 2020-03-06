using BetterSCP939.Extensions;
using EXILED.Extensions;
using System.Linq;

namespace BetterSCP939.Events
{
    public class RoundEvent
    {
        private readonly BetterSCP939 pluginInstance;

        public RoundEvent(BetterSCP939 pluginInstance) => this.pluginInstance = pluginInstance;

        public void OnWaitingForPlayers() => pluginInstance.LoadConfigs();

        public void OnRoundRestart()
        {
            foreach (var player in Player.GetHubs().ToList())
            {
                var customSCP939 = player.GetComponent<CustomSCP939>();

                if (customSCP939 != null) customSCP939.Destroy();
            }
        }
    }
}
