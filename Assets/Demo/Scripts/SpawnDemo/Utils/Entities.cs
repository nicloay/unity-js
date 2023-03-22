using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ClearScriptDemo.Demo.SpawnDemo.Utils
{
    /// <summary>
    ///     Factory class which instantiate instances of the prefab on the scene.
    ///     it keeps internal cache of all spawned entities, so you must spawn/destroy instances through this class
    ///     and off course maintain cache.
    ///     It subscribe to scene change event, so on every reload it must clear  all refs.
    ///     It singleton with scope=scene, on scene reload this class is disposed
    ///     This singleton right now works only in a single scene environment, as it clear all data on any scene unload
    /// </summary>
    public class Entities : IDisposable
    {
        private static Entities _instance;
        private Dictionary<int, GameObject> _instanceById = new();

        private Entities()
        {
        }

        private static Entities Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Entities();
                    SceneManager.sceneUnloaded += _instance.OnSceneUnloaded;
                }

                return _instance;
            }
        }

        public void Dispose()
        {
            _instanceById = null;
            _instance = null;
        }

        public static void InstantiatePrimitive(int id, PrimitiveType primitiveType)
        {
            var cache = Instance._instanceById;
            if (cache.ContainsKey(id))
            {
                Debug.LogError(
                    $"instance with id {id} already exists on the scene. multiple instance with same id is not supported");
                throw new Exception();
            }

            var instance = GameObject.CreatePrimitive(primitiveType);
            cache.Add(id, instance);
        }

        public static void Instantiate(int id, GameObject prefab)
        {
            var cache = Instance._instanceById;
            if (cache.ContainsKey(id))
            {
                Debug.LogError(
                    $"instance with id {id} already exists on the scene. multiple instance with same id is not supported");
                throw new Exception();
            }

            var instance = Object.Instantiate(prefab);
            cache.Add(id, instance);
        }

        public static GameObject GetById(int id)
        {
            return Instance._instanceById[id];
        }

        private void OnSceneUnloaded(Scene scene)
        {
            SceneManager.sceneUnloaded -= _instance.OnSceneUnloaded;
            _instance.Dispose();
            _instance = null;
        }
    }
}