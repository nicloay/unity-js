using ClearScriptDemo.SpawnDemo;
using UnityEngine;

namespace ClearScriptDemo.Demo.SpawnDemo
{
    [MessageId("entity_add")]
    public class EntityAddMessage : IMessage
    {
        public int Id { get; set; }
    }

    public static class AddMessageHandler
    {
        public static void Handle(this EntityAddMessage message)
        {
            Entities.InstantiatePrimitive(message.Id, PrimitiveType.Cube);
        }
    }
}