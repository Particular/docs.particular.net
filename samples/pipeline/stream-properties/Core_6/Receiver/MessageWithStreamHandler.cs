using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region message-with-stream-handler
public class MessageWithStreamHandler :
    IHandleMessages<MessageWithStream>
{
    static ILog log = LogManager.GetLogger<MessageWithStreamHandler>();

    public async Task Handle(MessageWithStream message, IMessageHandlerContext context)
    {
        var streamProperty = message.StreamProperty;
        log.Info($"Message received, size of stream property: {streamProperty.Length} Bytes");
        using (var streamReader = new StreamReader(streamProperty))
        {
            var streamContents = await streamReader.ReadToEndAsync()
                .ConfigureAwait(false);
            log.Info($"Stream content: {streamContents.Substring(0, 20)}...");
        }
    }

}
#endregion