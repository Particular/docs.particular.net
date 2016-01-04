namespace Snippets5.Azure.Transports.AzureServiceBus
{
    using System.IO;
    using Microsoft.ServiceBus.Messaging;
    using NServiceBus.Azure.Transports.WindowsAzureServiceBus;

    public class BrokeredMessageBodyConventionUsage
    {
        public void Snippet()
        {
            #region ASB-outgoing-message-convention 6.3

            BrokeredMessageBodyConversion.InjectBody = bytes =>
            {
                MemoryStream messageAsStream = new MemoryStream(bytes);
                return new BrokeredMessage(messageAsStream);
            };

            #endregion

            #region ASB-incoming-message-convention 6.3

            BrokeredMessageBodyConversion.ExtractBody = brokeredMessage =>
            {
                using (var stream = new MemoryStream())
                using (var body = brokeredMessage.GetBody<Stream>())
                {
                    body.CopyTo(stream);
                    return stream.ToArray();
                }
            };

            #endregion

        }
    }
}