using System;
using ClearScriptDemo.Demo.MessageExchangeBus.Utils;

namespace ClearScriptDemo.Demo.MessageExchangeBus
{
    public class MessageHandler
    {
        private readonly EntityManager _entityManager = new();

        public void HandleMessage(IMessage message)
        {
            switch (message)
            {
                case EntityAddMessage addMessage:
                    addMessage.Handle(_entityManager);
                    break;
                case EntityTransformUpdateMessage transformMessage:
                    transformMessage.Handle(_entityManager);
                    break;
                default:
                    throw new NotSupportedException($"NotSupported message [{message}]");
            }
        }
    }
}