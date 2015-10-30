using System;
using System.Diagnostics;
using NServiceBus.Serializers.Binary;
using NServiceBus.Serializers.Json;
using NUnit.Framework;

[TestFixture]
public class SerializedMessageGenerator
{
    [Test]
    public static void Generate()
    {
        var serializer = new JsonMessageSerializer(new SimpleMessageMapper());
        var serializedMessage = serializer.SerializeObject(new NativeMessage
        {
            Content = "Hello from native sender",
            SendOnUtc = DateTime.UtcNow
        });
        Debug.WriteLine("Serialized message to paste in code:\n");
        Debug.WriteLine(string.Format("@\"{0}\"", serializedMessage.Replace("\"", "\"\"")));
    }
}
