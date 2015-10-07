using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;

#region message-with-stream-handler
public class MessageWithStreamHandler : IHandleMessages<MessageWithStream>
{
    public async Task Handle(MessageWithStream message)
    {
        Console.WriteLine("Message received, size of stream property: " + message.StreamProperty.Length + " Bytes");
        using (StreamReader streamReader = new StreamReader(message.StreamProperty))
        {
            string streamContents = await streamReader.ReadToEndAsync();
            Console.WriteLine("Stream content: {0}...", streamContents.Substring(0, 20));
        }
    }
}
#endregion