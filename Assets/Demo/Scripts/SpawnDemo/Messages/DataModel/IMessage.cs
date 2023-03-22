namespace ClearScriptDemo.Demo.SpawnDemo
{
    /// <summary>
    ///  protocol used by JS and .NET for message exchange
    /// every message must implement this interface
    /// all public properties will be serialized and transferred back and forth
    /// so don't forget to add {get;set;} otherwise this data would NOT be serialized
    ///
    /// Also you must use [MessageId] attribute 
    /// </summary>
    public interface IMessage
    {
    }
}