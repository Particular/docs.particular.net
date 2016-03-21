using System.IO;
using NServiceBus;
using NServiceBus.Logging;

#region message-with-stream-handler
public class MessageWithStreamHandler : IHandleMessages<MessageWithStream>
{
    static ILog log = LogManager.GetLogger(typeof(MessageWithStreamHandler));

    public void Handle(MessageWithStream message)
    {
        log.Info("Message received, size of stream property: " + message.StreamProperty.Length + " Bytes");
        using (StreamReader streamReader = new StreamReader(message.StreamProperty))
        {
            string streamContents = streamReader.ReadToEnd();
            log.InfoFormat("Stream content: {0}...", streamContents.Substring(0, 20));
        }
    }
}
#endregion