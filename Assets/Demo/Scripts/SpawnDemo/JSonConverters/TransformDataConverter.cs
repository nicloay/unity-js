using System;
using ClearScriptDemo.Demo.SpawnDemo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ClearScriptDemo.Demo.ECS.JSonConverters
{
    public class TransformDataConverter : JsonConverter<TransformData>
    {
        public override bool CanWrite => false;

        public override TransformData ReadJson(JsonReader reader, Type objectType, TransformData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var positionArray = jObject.GetValue("position").ToObject<float[]>();
            var rotationArray = jObject.GetValue("rotation").ToObject<float[]>();
            var scaleArray = jObject.GetValue("scale").ToObject<float[]>();

            var position = new Vector3(positionArray[0], positionArray[1], positionArray[2]);
            var rotation = new Vector4(rotationArray[0], rotationArray[1], rotationArray[2], rotationArray[3]);
            var scale = new Vector3(scaleArray[0], scaleArray[1], scaleArray[2]);

            return new TransformData
            {
                Position = position,
                Rotation = rotation,
                Scale = scale
            };
        }

        public override void WriteJson(JsonWriter writer, TransformData value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}