using System.IO;
using ClearScriptDemo.Demo.SpawnDemo;
using ClearScriptDemo.JSonConverters;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests
{
    public class TestJson
    {
        [Test]
        public void TestDeserialization()
        {
            MessageConverter converter = new();
            var json = File.ReadAllText("Assets/Tests/TestJson.json");
            var data = JsonConvert.DeserializeObject<IJSMessage[]>(json, converter);

            Assert.That(data.Length, Is.EqualTo(2));
            Assert.That(data[0], Is.TypeOf<EntityAddMessage>());
            Assert.That(data[1], Is.TypeOf<EntityTransformUpdateMessage>());

            var first = (EntityAddMessage)data[0];
            Assert.That(first.Id, Is.EqualTo(21));

            var second = (EntityTransformUpdateMessage)data[1];
            Assert.That(second.Transform.Position.x, Is.EqualTo(1.3).Within(0.000001f));
            Assert.That(second.Transform.Position.y, Is.EqualTo(2 / 3f).Within(0.000001f));
            Assert.That(second.Transform.Position.z, Is.EqualTo(1 / 7f).Within(0.000001f));
        }
    }
}