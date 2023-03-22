using System;
using System.Collections.Generic;
using System.Reflection;
using ClearScriptDemo.Demo.SpawnDemo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace ClearScriptDemo.JSonConverters
{
    public class MessageConverter : JsonConverter
    {
        private readonly Dictionary<string, Type> _messageTypeById;

        public MessageConverter()
        {
            _messageTypeById = GetJsonMap();
        }

        private Dictionary<string,Type> GetJsonMap()
        {
            var result = new Dictionary<string, Type>();
            var allTypes = GetType().Assembly.GetTypes();

            var attribute = typeof(MessageIdAttribute);
            foreach (var type in allTypes)
            {
                if (Attribute.IsDefined(type, attribute))
                {
                    var a = type.GetCustomAttribute<MessageIdAttribute>();
                    result.Add(a.MessageId, type);
                }
            }
            return result;
        }


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

            if (!_messageTypeById.ContainsKey(typeName!)) throw new JsonSerializationException("Unknown message: " + typeName);

            var type = _messageTypeById[typeName];
            if (data == null) return Activator.CreateInstance(type);

            return data.ToObject(type, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}