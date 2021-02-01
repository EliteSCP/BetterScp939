namespace BetterScp939.Events
{
    using Components;
    using Exiled.Events.EventArgs;

    public class PlayerHandler
    {
        public void OnSetClass(ChangingRoleEventArgs ev)
        {
            if (ev.Player?.IsHost ?? true)
                return;

            if (ev.NewRole.Is939())
            {
                if (ev.Player.GameObject.TryGetComponent(out BetterScp939Controller customScp939))
                    customScp939.Destroy();

                ev.Player.GameObject.AddComponent<BetterScp939Controller>();
            }
        }
    }
}
