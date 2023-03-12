using System;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JSContainer;
using UnityEngine;

namespace ClearScriptDemo
{
    /// <summary>
    ///     Demonstrate how code from JS Sandbox send Debug.Draw line every frame 
    /// </summary>
    public class AsyncDemoRunner : MonoBehaviour
    {
        private const string JS_FILE_NAME = "async-demo.js";

        private async void Start()
        {
            using var sandbox = new JSSandbox();
            sandbox.Script.waitOneFrame = new Func<object>(WaitOneFrame);
            dynamic module =
                sandbox.EvaluateCommonJSModule(Path.Combine(Application.streamingAssetsPath, JS_FILE_NAME));
            await module.debugDrawLine();
        }

        private async Task WaitOneFrame()
        {
            await UniTask.DelayFrame(1).AsTask();
        }
    }
}