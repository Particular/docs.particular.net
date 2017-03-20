using NServiceBus;
using NServiceBus.Logging;

#region message-with-stream-handler
public class MessageWithStreamHandler1 :
    IHandleMessages<MessageWithStream>
{
    static ILog log = LogManager.GetLogger<MessageWithStreamHandler1>();

    public void Handle(MessageWithStream message)
    {
        var stream = message.StreamProperty;
        log.Info($"Message received, size of stream property: {stream.Length} Bytes");
        using (var streamReader = new ResettingStreamReader(stream))
        {
            var streamContents = streamReader.ReadToEnd();
            log.Info($"Stream content: {streamContents.Substring(0, 20)}...");
        }
    }
}
#endregion