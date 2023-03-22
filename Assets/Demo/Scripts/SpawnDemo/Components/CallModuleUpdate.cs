using UnityEngine;

namespace ClearScriptDemo.Demo.SpawnDemo.Components
{
    /// <summary>
    ///     This component call onUpdate on javascript module provided by Instantiate method
    /// </summary>
    public class CallModuleUpdate : MonoBehaviour
    {
        private dynamic _receiverModule;

        private void Update()
        {
            _receiverModule.onUpdate(Time.deltaTime);
        }

        public static CallModuleUpdate Instantiate(GameObject host, dynamic receiverModule)
        {
            var instance = host.AddComponent<CallModuleUpdate>();
            instance._receiverModule = receiverModule;
            return instance;
        }
    }
}