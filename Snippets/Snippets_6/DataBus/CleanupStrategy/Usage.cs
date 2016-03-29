namespace Snippets6.DataBus.CleanupStrategy
{
    using System.IO;
    using System.Threading.Tasks;
    using NServiceBus;
    using Snippets6.DataBus.DataBusProperty;

    public class Usage
    {
        #region FileLocationForDatabusFiles
        public async Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
        {
            string filename = Path.Combine(@"c:\databus_attachments\", message.LargeBlob.Key);
        }
        #endregion
    }
}
