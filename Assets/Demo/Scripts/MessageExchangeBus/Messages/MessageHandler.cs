using System;
using ClearScriptDemo.Demo.MessageExchangeBus.Utils;

namespace ClearScriptDemo.Demo.MessageExchangeBus
{
    public class MessageHandler
    {
        private readonly EntityManager _entityManager = new();
        
        /// <summary>
        /// Single entry point to manage all IMessages,
        /// You should add another method here if you add new IMessage types which comes from JS to Unity. 
        /// </summary>
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