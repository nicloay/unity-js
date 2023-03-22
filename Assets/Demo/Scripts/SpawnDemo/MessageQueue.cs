using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ClearScriptDemo.Demo.SpawnDemo;
using ClearScriptDemo.JSonConverters;
using Newtonsoft.Json;
using UnityEngine;

namespace ClearScriptDemo.Demo.Scripts.SpawnDemo.Components
{
    
    /// <summary>
    /// Message accumulator,
    /// Used to collect messages from different component,
    /// Pop method return collected messages and clear internal cache
    /// </summary>
    public class MessageQueue
    {
        private readonly List<IMessage> _messages = new List<IMessage>();
        public bool isNotEmpty => _messages.Count > 0;

        public void AddMessage(IMessage message)
        {
            _messages.Add(message);
        }

        public string[] PopMessagesAsStringList()
        {
            var result = _messages.Select(message =>
                {
                    var txt =JsonConvert.SerializeObject(message, new MessageConverter());
                    return txt;
                })
                .ToArray();
            _messages.Clear();
            return result;
        }
        
    }
}