namespace Core6.DataBus.CleanupStrategy
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using DataBusProperty;
    using NServiceBus;

    #region HandlerThatCleansUpDatabus

    public class Handler :
        IHandleMessages<MessageWithLargePayload>,
        IHandleMessages<RemoveDatabusAttachment>
    {

        public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
        {
            string filePath = Path.Combine(@"\\share\databus_attachments\", message.LargeBlob.Key);
            var removeAttachment = new RemoveDatabusAttachment
            {
                FilePath = filePath
            };
            SendOptions options = new SendOptions();
            options.RouteToThisEndpoint();
            options.DelayDeliveryWith(TimeSpan.FromDays(30));
            return context.Send(removeAttachment, options);
        }

        public Task Handle(RemoveDatabusAttachment message, IMessageHandlerContext context)
        {
            var filePath = message.FilePath;
            // Code to clean up
            return Task.FromResult(0);
        }
    }

    #endregion

    public class RemoveDatabusAttachment : ICommand
    {
        public string FilePath { get; set; }
    }
}
