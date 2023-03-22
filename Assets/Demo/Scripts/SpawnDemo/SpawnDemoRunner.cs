﻿using System;
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
        private readonly MessageHandler _messageHandler = new();
        private readonly MessageQueue _messageQueue = new();

        private dynamic _module;
        private JSSandbox _sandbox;

        private async void Awake()
        {
            _sandbox = new JSSandbox(objectsOverride: new Dictionary<string, object> { { "~engine", this } });
            _sandbox.Script.sendMessages = new Func<IList, object>(sendMessages);
            _module = _sandbox.EvaluateCommonJSModule(Path.Combine(Application.streamingAssetsPath, JS_FILE_NAME));
            await _module.onStart();
            CallModuleUpdate.Instantiate(gameObject, _module);
            KeyDownUpInputDispatcher.Instantiate(gameObject, _messageQueue);
        }

        private void OnDestroy()
        {
            _sandbox.Dispose();
        }

#pragma warning disable CS1998
        // ReSharper disable once MemberCanBePrivate.Global
        public async Task<object> sendMessages(IList messages)
#pragma warning restore CS1998
        {
            foreach (var message in messages)
            {
                var data = ((string)message).ToMessage();
                _messageHandler.HandleMessage(data);
            }

            return _sandbox.Script.Array.from(_messageQueue.PopMessagesAsStringList());
        }
    }
}