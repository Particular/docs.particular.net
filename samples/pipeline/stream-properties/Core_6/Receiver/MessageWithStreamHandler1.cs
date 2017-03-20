using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region message-with-stream-handler
public class MessageWithStreamHandler1 :
    IHandleMessages<MessageWithStream>
{
    static ILog log = LogManager.GetLogger<MessageWithStreamHandler1>();

    public async Task Handle(MessageWithStream message, IMessageHandlerContext context)
    {
        var stream = message.StreamProperty;
        log.Info($"Message received, size of stream property: {stream.Length} Bytes");
        using (var streamReader =  new ResettingStreamReader(stream))
        {
            var streamContents = await streamReader.ReadToEndAsync()
                .ConfigureAwait(false);
            log.Info($"Stream content: {streamContents.Substring(0, 20)}...");
        }
    }

}
#endregion