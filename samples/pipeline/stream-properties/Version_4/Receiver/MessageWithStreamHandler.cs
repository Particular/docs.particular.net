using System;
using System.IO;
using NServiceBus;

#region MessageWithLargePayloadHandler
public class MessageWithStreamHandler : IHandleMessages<MessageWithStream>
{
    public void Handle(MessageWithStream message)
    {
        Console.WriteLine("Message received, size of stream property: " + message.StreamProperty.Length + " Bytes");
        using (StreamReader streamReader = new StreamReader(message.StreamProperty))
        {
            string streamContents = streamReader.ReadToEnd();
            Console.WriteLine("Stream content: {0}...", streamContents.Substring(0, 20));
        }
    }
}
#endregion