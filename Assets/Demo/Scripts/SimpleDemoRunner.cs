using System.IO;
using JSContainer;
using UnityEngine;

namespace ClearScriptDemo.Demo
{
    /// <summary>
    ///     This Demo call JS method from Update loop with parameters
    ///     Demo js is CommonJS module
    /// </summary>
    public class SimpleDemoRunner : MonoBehaviour
    {
        private const string JS_FILE_NAME = "simple-demo.js";
        private JSSandbox _container;
        private int _frameNumber = -1;
        private dynamic _module;

        private void Start()
        {
            _container = new JSSandbox();

            var filePath = Path.Combine(Application.streamingAssetsPath, JS_FILE_NAME);
            _module = _container.EvaluateCommonJSModule(filePath);

            _module.onStart();
            _frameNumber = 1;
        }

        private void Update()
        {
            if (_frameNumber is >= 0 and <= 3) _module.onUpdate(_frameNumber++);
        }

        private void OnDestroy()
        {
            _container?.Dispose();
        }
    }
}