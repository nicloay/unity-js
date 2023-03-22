namespace ClearScriptDemo.Demo.SpawnDemo
{
    [MessageId("key_down")]
    public class KeyDownMessage : IJSMessage
    {
        public string Key;
    }
}