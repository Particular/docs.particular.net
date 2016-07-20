using System.IO;
using NServiceBus;
using NServiceBus.Logging;

#region message-with-stream-handler
public class MessageWithStreamHandler :
    IHandleMessages<MessageWithStream>
{
    static ILog log = LogManager.GetLogger<MessageWithStreamHandler>();

    public void Handle(MessageWithStream message)
    {
        var streamProperty = message.StreamProperty;
        log.Info($"Message received, size of stream property: {streamProperty.Length} Bytes");
        using (var streamReader = new StreamReader(streamProperty))
        {
            var streamContents = streamReader.ReadToEnd();
            log.Info($"Stream content: {streamContents.Substring(0, 20)}...");
        }
    }
}
#endregion