using ClearScriptDemo.Demo.MessageExchangeBus.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace ClearScriptDemo.Demo.MessageExchangeBus
{
    [UsedImplicitly]
    [MessageId("entity_add")]
    public class EntityAddMessage : IMessage
    {
        public int Id { get; set; }
    }

    public static class AddMessageHandler
    {
        public static void Handle(this EntityAddMessage message, EntityManager entityManager)
        {
            entityManager.InstantiatePrimitive(message.Id, PrimitiveType.Cube);
        }
    }
}