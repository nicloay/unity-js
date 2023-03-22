namespace ClearScriptDemo.Demo.SpawnDemo
{
    [MessageId("key_up")]
    public class KeyUpMessage : IJSMessage
    {
        public string Key;
    }
}