using UnityEngine;

namespace Olimpik.NConsole.Runtime
{
    internal class Bootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void EntryPoint()
        {
            if (Application.isEditor)
            {
                return;
            }

            var behaviour = new CommandBehaviour();
            var registration = new CommandsRegistration(behaviour, behaviour);
            registration.Register();
        }
    }
}
