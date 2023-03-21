using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClearScriptDemo.Demo.SpawnDemo;
using ClearScriptDemo.JSonConverters;
using JSContainer;
using Newtonsoft.Json;
using UnityEngine;

namespace ClearScriptDemo.SpawnDemo
{
    public class SpawnDemoRunner : MonoBehaviour
    {
        private Dictionary<int, GameObject> _entitiesById = new Dictionary<int, GameObject>();
        
        private const string JS_FILE_NAME = "spawn-demo.js";
        private dynamic _module;
        private JSSandbox _sandbox;

        private async void Awake()
        {
            _sandbox = new JSSandbox(objectsOverride: new Dictionary<string, object>{{"~engine", this}});
            _sandbox.Script.sendMessages = new Func<object[], object[]>(sendMessages);
            _module = _sandbox.EvaluateCommonJSModule(Path.Combine(Application.streamingAssetsPath, JS_FILE_NAME));

            await _module.onStart();
        }

        private readonly MessageConverter _converter = new();

        public object[] sendMessages(IList messages)
        {
            foreach (var message in messages)
            {
                var data = JsonConvert.DeserializeObject<IJSMessage>((string)message, _converter);
                MessageHandler.HandleMessage(data);
            }

            return new string[0];
        }

        private void OnDestroy()
        {
            _sandbox.Dispose();
        }
    }
    
}