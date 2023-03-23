using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClearScriptDemo.Demo.MessageExchangeBus.Components;
using ClearScriptDemo.Demo.MessageExchangeBus.Utils;
using JSContainer;
using UnityEngine;

namespace ClearScriptDemo.Demo.MessageExchangeBus
{
    public class MessageExchangeBusRunner : MonoBehaviour
    {
        private const string JS_FILE_NAME = "message-exchange-bus-demo.js";
        private readonly MessageHandler _messageHandler = new();
        private readonly MessageQueue _messageQueue = new();

        private dynamic _module;
        private JSSandbox _sandbox;

        private void Awake()
        {
            _sandbox = new JSSandbox(objectsOverride: new Dictionary<string, object> { { "~engine", this } });
            _sandbox.Script.sendMessages = new Func<IList, object>(sendMessages);
            _module = _sandbox.EvaluateCommonJSModule(Path.Combine(Application.streamingAssetsPath, JS_FILE_NAME));
        }

        private async void Start()
        {
            await _module.onStart();
            CallModuleUpdateTrigger.Instantiate(gameObject, _module); // this component will call update on the _module.
            // ReSharper disable once Unity.InefficientPropertyAccess
            KeyDownUpInputDispatcher.Instantiate(gameObject, _messageQueue); // this component will populate message queue with input messages when they occurs.
        }

        private void OnDestroy()
        {
            _sandbox.Dispose();
        }

#pragma warning disable CS1998
        /// <summary>
        /// All message exchange happens through this method. it receive list of IMessages and return another list of IMessages
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once InconsistentNaming
        public async Task<object> sendMessages(IList messages)
#pragma warning restore CS1998
        {
            foreach (var message in messages)
            {
                var data = ((string)message).ToMessage();
                _messageHandler.HandleMessage(data);
            }

            return _sandbox.Script.Array.from(_messageQueue.PopMessagesAsJsonArray());
        }
    }
}