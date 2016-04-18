using System;
using System.Diagnostics;
using Newtonsoft.Json;
using NUnit.Framework;

[TestFixture]
public class SerializedMessageGenerator
{
    [Test]
    public static void Generate()
    {
        NativeMessage message = new NativeMessage
        {
            Content = "Hello from native sender",
            SendOnUtc = DateTime.UtcNow
        };
        string serializedMessage = JsonConvert.SerializeObject(message);
        Debug.WriteLine("Serialized message to paste in code:\n");
        Debug.WriteLine(string.Format("@\"{0}\"", serializedMessage.Replace("\"", "\"\"")));
    }
}
