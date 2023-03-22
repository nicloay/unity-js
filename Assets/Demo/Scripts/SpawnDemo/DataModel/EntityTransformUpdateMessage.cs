using ClearScriptDemo.SpawnDemo;
using UnityEngine;

namespace ClearScriptDemo.Demo.SpawnDemo
{
    [MessageId("entity_transform_update")]
    public class EntityTransformUpdateMessage : IJSMessage
    {
        public int EntityId { get; set; }
        public TransformData Transform { get; set; }
    }
    
    public static class EntityTransformUpdateHandler
    {
        public static void Handle(this EntityTransformUpdateMessage message)
        {
            var entity = Entities.GetById(message.EntityId);
            entity.transform.position = message.Transform.Position;
            entity.transform.rotation = Quaternion.Euler(message.Transform.Rotation);
            entity.transform.localScale = message.Transform.Scale;
        }
    }
}