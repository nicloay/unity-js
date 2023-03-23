using ClearScriptDemo.Demo.MessageExchangeBus.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace ClearScriptDemo.Demo.MessageExchangeBus
{
    [JsonConverter(typeof(TransformDataConverter))]
    public class TransformData
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
    }
}