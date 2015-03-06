using System;
using System.IO;
using Messages;
using NServiceBus;

#region MessageWithLargePayloadHandler
public class MessageWithStreamHandler : IHandleMessages<MessageWithStream>
{
    public void Handle(MessageWithStream message)
    {
        Console.WriteLine("Message received, size of stream property: " + message.StreamProperty.Length + " Bytes");
        string streamContents = new StreamReader(message.StreamProperty).ReadToEnd();
        Console.WriteLine("Stream content: " + streamContents);
    }
}
#endregion