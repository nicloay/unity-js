using System.Collections.Generic;
using System.Linq;
using ClearScriptDemo.Demo.SpawnDemo.Utils;

namespace ClearScriptDemo.Demo.SpawnDemo
{
    /// <summary>
    ///     Message accumulator,
    ///     Used to collect messages from different component,
    ///     Pop method return collected messages and clear internal cache
    /// </summary>
    public class MessageQueue
    {
        private readonly List<IMessage> _messages = new();
        public bool IsNotEmpty => _messages.Count > 0;

        public void AddMessage(IMessage message)
        {
            _messages.Add(message);
        }

        public string[] PopMessagesAsStringList()
        {
            var result = _messages.Select(message => message.ToJsonString()).ToArray();
            _messages.Clear();
            return result;
        }
    }
}