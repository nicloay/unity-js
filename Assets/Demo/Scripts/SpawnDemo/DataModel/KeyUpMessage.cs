namespace ClearScriptDemo.Demo.SpawnDemo
{
    [MessageId("key_up")]
    public class KeyUpMessage : IMessage
    {
        public string Key { get; set; }
    }
}