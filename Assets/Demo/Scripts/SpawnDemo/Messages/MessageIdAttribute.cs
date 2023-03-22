using System;

namespace ClearScriptDemo.Demo.SpawnDemo
{
    public class MessageIdAttribute : Attribute
    {
        public readonly string MessageId;

        public MessageIdAttribute(string messageId)
        {
            MessageId = messageId;
        }
    }
}