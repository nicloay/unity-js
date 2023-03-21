using UnityEngine;

namespace ClearScriptDemo.Demo.SpawnDemo
{
    public class EntityTransformUpdateMessage : IJSMessage
    {
        public int EntityId { get; set; }
        public TransformData Transform { get; set; }
    }
    
    public static class EntityTransformUpdateHandler
    {
        public static void Handle(this EntityTransformUpdateMessage message)
        {
            Debug.LogError("Not implemented yet");
        }
    }
}