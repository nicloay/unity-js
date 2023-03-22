using ClearScriptDemo.Demo.SpawnDemo.Utils;

namespace ClearScriptDemo.Demo.SpawnDemo
{
    [MessageId("entity_transform_update")]
    public class EntityTransformUpdateMessage : IMessage
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
            entity.transform.rotation = message.Transform.Rotation;
            entity.transform.localScale = message.Transform.Scale;
        }
    }
}