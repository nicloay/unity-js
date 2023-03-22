using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ClearScriptDemo.Demo.SpawnDemo.Utils
{
    /// <summary>
    ///     This manager instantiates instances of a prefab on the scene with a specified ID,
    ///     and keeps an internal cache of all spawned entities.
    ///     This cache allows for quick querying of spawned instances by their ID.
    ///     To ensure consistency, all instances must be spawned and destroyed through this class.
    /// </summary>
    public class EntityManager
    {
        private readonly Dictionary<int, GameObject> _instanceById = new();

        public void InstantiatePrimitive(int id, PrimitiveType primitiveType)
        {
            if (_instanceById.ContainsKey(id))
            {
                Debug.LogError(
                    $"instance with id {id} already exists on the scene. multiple instance with same id is not supported");
                throw new Exception();
            }

            var instance = GameObject.CreatePrimitive(primitiveType);
            _instanceById.Add(id, instance);
        }

        public void Instantiate(int id, GameObject prefab)
        {
            if (_instanceById.ContainsKey(id))
            {
                Debug.LogError(
                    $"instance with id {id} already exists on the scene. multiple instance with same id is not supported");
                throw new Exception();
            }

            var instance = Object.Instantiate(prefab);
            _instanceById.Add(id, instance);
        }

        public GameObject GetById(int id)
        {
            return _instanceById[id];
        }
    }
}