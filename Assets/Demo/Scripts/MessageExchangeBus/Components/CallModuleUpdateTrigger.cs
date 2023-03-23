using UnityEngine;

namespace ClearScriptDemo.Demo.MessageExchangeBus.Components
{
    /// <summary>
    ///     This component call onUpdate on javascript module provided by Instantiate method
    /// </summary>
    public class CallModuleUpdateTrigger : MonoBehaviour
    {
        // reference to javascript module which method onUpdate will be triggered every frame
        private dynamic _receiverModule;

        private void Update()
        {
            _receiverModule.onUpdate(Time.deltaTime);
        }

        
        // Factory method which will create instance on the host object
        public static CallModuleUpdateTrigger Instantiate(GameObject host, dynamic receiverModule)
        {
            var instance = host.AddComponent<CallModuleUpdateTrigger>();
            instance._receiverModule = receiverModule;
            return instance;
        }
    }
}