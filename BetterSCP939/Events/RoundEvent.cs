namespace BetterSCP939.Events
{
    public class RoundEvent
    {
        private readonly BetterSCP939 pluginInstance;

        public RoundEvent(BetterSCP939 pluginInstance) => this.pluginInstance = pluginInstance;

        public void OnWaitingForPlayers() => pluginInstance.LoadConfigs();
    }
}
