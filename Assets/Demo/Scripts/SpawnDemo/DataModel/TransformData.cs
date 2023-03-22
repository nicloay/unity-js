using ClearScriptDemo.Demo.SpawnDemo.JSonConverters;
using Newtonsoft.Json;
using UnityEngine;

namespace ClearScriptDemo.Demo.SpawnDemo
{
    [JsonConverter(typeof(TransformDataConverter))]
    public class TransformData
    {
        public Vector3 Position { get; set; }
        public Vector4 Rotation { get; set; }
        public Vector3 Scale { get; set; }
    }
}