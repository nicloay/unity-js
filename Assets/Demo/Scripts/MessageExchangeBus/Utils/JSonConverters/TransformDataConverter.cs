﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ClearScriptDemo.Demo.MessageExchangeBus.Utils
{
    /// <summary>
    /// convert javascript transform in to TransformData
    /// incoming json has following format
    /// <code>{"transform": {
    ///    "position": [x, y, z],
    ///    "rotation": [x, y, z, w],
    ///    "scale": [x, y, z]
    ///  }}</code>
    /// </summary>
    public class TransformDataConverter : JsonConverter<TransformData>
    {
        public override bool CanWrite => true;

        public override TransformData ReadJson(JsonReader reader, Type objectType, TransformData existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var positionArray = jObject.GetValue("position")?.ToObject<float[]>();
            var rotationArray = jObject.GetValue("rotation")?.ToObject<float[]>();
            var scaleArray = jObject.GetValue("scale")?.ToObject<float[]>();

            var position = new Vector3(positionArray![0], positionArray[1], positionArray[2]);
            var rotation = new Quaternion(rotationArray![0], rotationArray[1], rotationArray[2], rotationArray[3]);
            var scale = new Vector3(scaleArray![0], scaleArray[1], scaleArray[2]);

            return new TransformData
            {
                Position = position,
                Rotation = rotation,
                Scale = scale
            };
        }

        public override void WriteJson(JsonWriter writer, TransformData value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("position");
            writer.WriteStartArray();
            writer.WriteValue(value.Position.x);
            writer.WriteValue(value.Position.y);
            writer.WriteValue(value.Position.z);
            writer.WriteEndArray();
            writer.WritePropertyName("rotation");
            writer.WriteStartArray();
            writer.WriteValue(value.Rotation.x);
            writer.WriteValue(value.Rotation.y);
            writer.WriteValue(value.Rotation.z);
            writer.WriteValue(value.Rotation.w);
            writer.WriteEndArray();
            writer.WritePropertyName("scale");
            writer.WriteStartArray();
            writer.WriteValue(value.Scale.x);
            writer.WriteValue(value.Scale.y);
            writer.WriteValue(value.Scale.z);
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}