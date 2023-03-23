namespace ClearScriptDemo.Demo.MessageExchangeBus
{
    [MessageId("key_up")]
    public class KeyUpMessage : IMessage
    {
        public string Key { get; set; }
    }
}