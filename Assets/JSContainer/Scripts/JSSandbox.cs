using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using JSContainer.Interop;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using UnityEngine;

namespace JSContainer
{
    /// <summary>
    ///     Adapter class to ClearScript package with V8 javascript backend
    /// </summary>
    public class JSSandbox : IDisposable
    {
        private static readonly Type InteropAttribute = typeof(InteropModule);

        private static readonly HashSet<Type> ExposedTypes = new()
        {
            typeof(Vector3), typeof(Color)
        };

        private readonly V8ScriptEngine _engine;
        private readonly Dictionary<string, object> _globalModuleInstances = new();
        private readonly IReadOnlyDictionary<string, Type> _ioModulesByName;

        static JSSandbox()
        {
            V8Settings.GlobalFlags |= V8GlobalFlags.DisableBackgroundWork;
        }

        public JSSandbox()
        {
            _engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableTaskPromiseConversion);

            // 1. Add internal method
            _engine.Script.JSContainerLazyLoadModule = new Func<string, object>(LazyLoadModule);

            // 2. Expose the whole unity Debug class for direct access
            _engine.AddHostType("Debug", typeof(Debug)); // always enabled global unity api

            // 2. Expose UnityEngine types see ExposedTypes
            var collection = new HostTypeCollection();
            collection.AddAssembly(typeof(Vector3).Assembly, ExposedTypes.Contains);
            _engine.AddHostObject("unity", collection);

            // 4. Load Custom Interop Objects which can be used as CommonJS modules
            _engine.DocumentSettings.AccessFlags = DocumentAccessFlags.EnableFileLoading;
            _ioModulesByName =
                LoadIOModules(); // FUTURE_TASK: it's possible to reuse the same list for multiple containers
            foreach (var keyValuePair in _ioModulesByName) Include(keyValuePair.Key);
        }

        public dynamic Script => _engine.Script;

        public void Dispose()
        {
            _engine?.Dispose();
        }

        /// <summary>
        ///     Evaluate CommonJS module and return object to the managed code
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>object returned by JS sandbox</returns>
        public object EvaluateCommonJSModule(string filePath)
        {
            filePath = filePath.Replace('\\', '/'); // ClearScriptV8 accept paths only in unix format
            if (!File.Exists(filePath))
            {
                Debug.LogError($"File [{filePath}] doesn't exists");
                throw new FileNotFoundException();
            }

            return _engine.Evaluate(new DocumentInfo { Category = ModuleCategory.CommonJS },
                $"return require('{filePath.Normalize()}')");
        }

        [UsedImplicitly]
        internal object LazyLoadModule(string itemName)
        {
            if (!_globalModuleInstances.ContainsKey(itemName))
            {
                var instance = Activator.CreateInstance(_ioModulesByName[itemName]);
                _globalModuleInstances.Add(itemName, instance);
            }

            return _globalModuleInstances[itemName];
        }

        private IReadOnlyDictionary<string, Type> LoadIOModules()
        {
            var result = new Dictionary<string, Type>();
            var assembly = typeof(InteropModule).Assembly;
            foreach (var type in assembly.GetTypes())
                if (Attribute.IsDefined(type, InteropAttribute))
                {
                    var attribute = (InteropModule)Attribute.GetCustomAttribute(type, InteropAttribute);
                    if (result.ContainsKey(attribute.ItemName))
                    {
                        Debug.LogError($"problem with multiple module with the same itemName: {attribute.ItemName} " +
                                       $"types: {result[attribute.ItemName]}, {type}");
                        throw new Exception();
                    }

                    if (attribute.AlwaysOn)
                        _engine.AddHostType(attribute.ItemName, type);
                    else
                        result.Add(attribute.ItemName, type);
                }

            result.Add("Debug", typeof(Debug)); // hacky way to expose all unity Debug class 
            return result;
        }

        private void Include(string itemName)
        {
            if (!_ioModulesByName.ContainsKey(itemName)) Debug.LogError($"Unknown module: {itemName}");

            var type = _ioModulesByName[itemName];
            _engine.AddHostType(itemName, type);
            _engine.DocumentSettings.AddSystemDocument($"{itemName}", ModuleCategory.CommonJS,
                $"module.exports = JSContainerLazyLoadModule(\"{itemName}\")");
        }
    }
}