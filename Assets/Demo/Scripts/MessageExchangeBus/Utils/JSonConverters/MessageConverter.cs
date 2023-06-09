﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ClearScriptDemo.Demo.MessageExchangeBus.Utils
{
    /// <summary>
    /// Convert messages from json to specific objects.
    /// messages comes as java script in the following format
    /// <code>{
    ///     "method":"MESSAGE_TYPE",
    ///     "data": {
    ///         ...
    ///     }
    /// }</code>
    /// based on MESSAGE_TYPE corresponding type will be created <see cref="MessageIdAttribute"/> which establish link between this MESSAGE_TYPE and exact type
    /// data object fields are serialized in to final result
    /// </summary>
    public class MessageConverter : JsonConverter
    {
        private static readonly IReadOnlyDictionary<string, Type> MESSAGE_TYPE_BY_ID;
        private static readonly Type MESSAGE_ID_ATTRIBUTE = typeof(MessageIdAttribute);

        static MessageConverter()
        {
            MESSAGE_TYPE_BY_ID = GetJsonMap();
        }

        private static Dictionary<string, Type> GetJsonMap()
        {
            var typeById = new Dictionary<string, Type>();
            var allTypes = typeof(MessageConverter).Assembly.GetTypes();

            foreach (var type in allTypes)
                if (Attribute.IsDefined(type, MESSAGE_ID_ATTRIBUTE))
                {
                    var a = type.GetCustomAttribute<MessageIdAttribute>();
                    typeById.Add(a.MessageId, type);
                }

            return typeById;
        }


        public override bool CanConvert(Type objectType)
        {
            return typeof(IMessage).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = (JObject)JToken.ReadFrom(reader);
            var typeName = (string)jObject["method"];
            var data = jObject["data"];

            if (!MESSAGE_TYPE_BY_ID.ContainsKey(typeName!))
                throw new JsonSerializationException("Unknown message: " + typeName);

            var type = MESSAGE_TYPE_BY_ID[typeName];
            var result = Activator.CreateInstance(type);
            if (data != null) serializer.Populate(data.CreateReader(), result);

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();
            if (!Attribute.IsDefined(type, MESSAGE_ID_ATTRIBUTE))
            {
                Debug.LogError($"attribute {nameof(MessageIdAttribute)} is not presented at {type}");
                throw new Exception();
            }

            var messageId = type.GetCustomAttribute<MessageIdAttribute>().MessageId;

            var messageProps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var dataProps = new List<JProperty>();


            foreach (var prop in messageProps)
            {
                var propValue = prop.GetValue(value);
                if (propValue != null)
                {
                    var propJson = new JProperty(prop.Name.ToCamelCase(), JToken.FromObject(propValue));
                    dataProps.Add(propJson);
                }
            }


            // ReSharper disable once CoVariantArrayConversion
            var dataObj = new JObject(dataProps.ToArray());
            var messageObj = new JObject(
                new JProperty("method", messageId),
                new JProperty("data", dataObj));

            messageObj.WriteTo(writer);
        }
    }
}