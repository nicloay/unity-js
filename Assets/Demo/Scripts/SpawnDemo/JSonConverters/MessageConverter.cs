using System;
using System.Collections.Generic;
using ClearScriptDemo.Demo.SpawnDemo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace ClearScriptDemo.JSonConverters
{
    public class MessageConverter : JsonConverter
    {
        private static readonly Dictionary<string, Type> JSON_MAP = new()
        {
            { "entity_transform_update", typeof(EntityTransformUpdateMessage) },
            { "entity_add", typeof(EntityAddMessage) }
        };


        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(IJSMessage));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = (JObject)JToken.ReadFrom(reader);
            var typeName = (string)jObject["method"];
            var data = jObject["data"];

            if (!JSON_MAP.ContainsKey(typeName!)) throw new JsonSerializationException("Unknown message: " + typeName);

            var type = JSON_MAP[typeName];
            if (data == null) return Activator.CreateInstance(type);

            return data.ToObject(type, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}