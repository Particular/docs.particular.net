using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SampleNamespace;
#region Handler

class MyHandler : IHandleMessages<SampleMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(SampleMessage message, IMessageHandlerContext context)
    {
        log.Info("SampleMessage received");
        log.Info($"Property1={message.Property1}");
        log.Info($"Property2={message.Property2}");
        foreach (var header in context.MessageHeaders)
        {
            var headerSuffix = header.Key.Replace("NServiceBus.", "");
            log.Info($"{headerSuffix}={header.Value}");
        }

        return context.Attachments().ProcessStreams(WriteAttachment);
    }

    async Task WriteAttachment(string name, Stream stream)
    {
        using (var reader = new StreamReader(stream))
        {
            var contents = await reader.ReadToEndAsync()
                .ConfigureAwait(false);
            log.Info($"Attachment: {name}. Contents:{contents}");
        }
    }
}

#endregion