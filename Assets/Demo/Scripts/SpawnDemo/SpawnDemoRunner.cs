using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClearScriptDemo.Demo.SpawnDemo.Components;
using ClearScriptDemo.Demo.SpawnDemo.Utils;
using JSContainer;
using UnityEngine;

namespace ClearScriptDemo.Demo.SpawnDemo
{
    public class SpawnDemoRunner : MonoBehaviour
    {
        private const string JS_FILE_NAME = "spawn-demo.js";
        private readonly MessageQueue _messageQueue = new();

        private Dictionary<int, GameObject> _entitiesById = new();
        private dynamic _module;
        private JSSandbox _sandbox;

        private async void Awake()
        {
            KeyDownUpInputDispatcher.Instantiate(gameObject, _messageQueue);
            _sandbox = new JSSandbox(objectsOverride: new Dictionary<string, object> { { "~engine", this } });
            _sandbox.Script.sendMessages = new Func<IList, object>(sendMessages);
            _module = _sandbox.EvaluateCommonJSModule(Path.Combine(Application.streamingAssetsPath, JS_FILE_NAME));
            CallModuleUpdate.Instantiate(gameObject, _module);
            await _module.onStart();
        }

        private void OnDestroy()
        {
            _sandbox.Dispose();
        }

        public async Task<object> sendMessages(IList messages)
        {
            foreach (var message in messages)
            {
                var data = ((string)message).ToMessage();
                MessageHandler.HandleMessage(data);
            }

            return _sandbox.Script.Array.from(_messageQueue.PopMessagesAsStringList());
        }
    }
}