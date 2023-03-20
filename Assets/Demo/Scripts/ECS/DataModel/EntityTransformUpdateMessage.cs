namespace ClearScriptDemo.Demo.ECS
{
    public class EntityTransformUpdateMessage : IJSMessage
    {
        public int EntityId { get; set; }
        public TransformData Transform { get; set; }
    }
}