using Mirror;

namespace BetterSCP939.Extensions
{
    public class CustomSCP939 : NetworkBehaviour
    {
        public void Start()
        {
            EXILED.Log.Info("Test");
        }

        public void Awake()
        {
            EXILED.Log.Info("Test2");
        }
    }
}
