
using System;

namespace ClearScriptDemo.Demo.SpawnDemo
{
    public static class MessageHandler
    {
        public static void HandleMessage(IMessage message)
        {
            switch (message)
            {
                case EntityAddMessage addMessage:
                    addMessage.Handle();
                    break;
                case EntityTransformUpdateMessage transformMessage:
                    transformMessage.Handle();
                    break;
                default:
                    throw new NotSupportedException($"NotSupported message [{message}]");
            }
        }
    }
}