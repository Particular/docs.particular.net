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
        var message = new NativeMessage
        {
            Content = "Hello from native sender",
            SentOnUtc = DateTime.UtcNow
        };
        var serializedMessage = JsonConvert.SerializeObject(message);
        Debug.WriteLine("Serialized message to paste in code:\n");
        Debug.WriteLine($"@\"{serializedMessage.Replace("\"", "\"\"")}\"");
    }
}
