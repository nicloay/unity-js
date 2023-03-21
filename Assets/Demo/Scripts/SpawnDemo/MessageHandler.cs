
using System;

namespace ClearScriptDemo.Demo.SpawnDemo
{
    public static class MessageHandler
    {
        public static void HandleMessage(IJSMessage jsMessage)
        {
            switch (jsMessage)
            {
                case EntityAddMessage addMessage:
                    addMessage.Handle();
                    break;
                case EntityTransformUpdateMessage transformMessage:
                    transformMessage.Handle();
                    break;
                default:
                    throw new NotSupportedException($"NotSupported message [{jsMessage}]");
            }
        }
    }
}