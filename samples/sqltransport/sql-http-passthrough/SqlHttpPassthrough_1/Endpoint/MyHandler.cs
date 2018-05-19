using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using SampleNamespace;

class MyHandler : IHandleMessages<SampleMessage>
{
    public Task Handle(SampleMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("MyHandler");
        foreach (var header in context.MessageHeaders)
        {
            Console.WriteLine($"{header.Key.Replace("NServiceBus.","")}={header.Value}");
        }
        return context.Attachments().ProcessStreams(WriteAttachment);
    }

    async Task WriteAttachment(string name, Stream stream)
    {
        using (var reader = new StreamReader(stream))
        {
            var contents = await reader.ReadToEndAsync()
                .ConfigureAwait(false);
            Console.WriteLine("Attachment: {0}. Contents:{1}", name, contents);
        }
    }
}