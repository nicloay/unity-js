namespace ClearScriptDemo.Demo.MessageExchangeBus
{
    [MessageId("key_down")]
    public class KeyDownMessage : IMessage
    {
        public string Key { get; set; }
    }
}