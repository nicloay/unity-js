using System.IO;
using ClearScriptDemo.Demo.MessageExchangeBus;
using ClearScriptDemo.Demo.MessageExchangeBus.Utils;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace EditorTests
{
    public class TestJson
    {
        [Test]
        public void Deserialize_EntityTransformUpdateMessage()
        {
            MessageConverter converter = new();
            var json = File.ReadAllText("Assets/Tests/Editor/TestJson.json");
            var data = JsonConvert.DeserializeObject<IMessage[]>(json, converter);

            Assert.That(data.Length, Is.EqualTo(2));
            Assert.That(data[0], Is.TypeOf<EntityAddMessage>());
            Assert.That(data[1], Is.TypeOf<EntityTransformUpdateMessage>());

            var first = (EntityAddMessage)data[0];
            Assert.That(first.EntityId, Is.EqualTo(21));

            var second = (EntityTransformUpdateMessage)data[1];
            Assert.That(second.Transform.Position.x, Is.EqualTo(1.3).Within(0.000001f));
            Assert.That(second.Transform.Position.y, Is.EqualTo(2 / 3f).Within(0.000001f));
            Assert.That(second.Transform.Position.z, Is.EqualTo(1 / 7f).Within(0.000001f));
        }

        [Test]
        public void Serialize_TransformData_ReturnsExpectedJson()
        {
            // Arrange
            var data = new TransformData
            {
                Position = new Vector3(1.0f, 2.0f, 3.0f),
                Rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                Scale = new Vector3(2.0f, 2.0f, 2.0f)
            };

            var expectedJson = "{\"position\":[1.0,2.0,3.0],\"rotation\":[0.0,0.0,0.0,1.0],\"scale\":[2.0,2.0,2.0]}";
            var actualJson = JsonConvert.SerializeObject(data, new MessageConverter());

            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }

        [Test]
        public void Serialize_EntityTransformUpdateMessage_ReturnsExpectedJson()
        {
            // Arrange
            var message = new EntityTransformUpdateMessage
            {
                EntityId = 123,
                Transform = new TransformData
                {
                    Position = new Vector3(1, 2, 3),
                    Rotation = new Quaternion(0, 0, 0, 1),
                    Scale = new Vector3(2, 2, 2)
                }
            };

            const string expectedJson =
                "{\"method\":\"entity_transform_update\",\"data\":{\"entityId\":123,\"transform\":{\"position\":[1.0,2.0,3.0],\"rotation\":[0.0,0.0,0.0,1.0],\"scale\":[2.0,2.0,2.0]}}}";

            var actualJson = JsonConvert.SerializeObject(message, new MessageConverter());
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }
    }
}