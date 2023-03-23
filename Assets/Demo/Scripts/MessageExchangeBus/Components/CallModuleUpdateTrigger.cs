using UnityEngine;

namespace ClearScriptDemo.Demo.MessageExchangeBus.Components
{
    /// <summary>
    ///     This component call onUpdate on javascript module provided by Instantiate method
    /// </summary>
    public class CallModuleUpdateTrigger : MonoBehaviour
    {
        private dynamic _receiverModule;

        private void Update()
        {
            _receiverModule.onUpdate(Time.deltaTime);
        }

        public static CallModuleUpdateTrigger Instantiate(GameObject host, dynamic receiverModule)
        {
            var instance = host.AddComponent<CallModuleUpdateTrigger>();
            instance._receiverModule = receiverModule;
            return instance;
        }
    }
}