using UnityEngine;

// ReSharper disable InconsistentNaming

namespace JSContainer.Interop
{
    [InteropModule("~engine")]
    public class EngineModule
    {
        public void sendMessage(string message)
        {
            Debug.Log($"EngineModule: sendMessage: {message}");
        }
    }
}