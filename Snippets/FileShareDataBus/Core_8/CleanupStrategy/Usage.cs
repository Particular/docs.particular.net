namespace Core7.CleanupStrategy
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using NServiceBus;

    #region HandlerThatCleansUpDatabus

    public class Handler :
        IHandleMessages<MessageWithLargePayload>,
        IHandleMessages<RemoveDatabusAttachment>
    {

        public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
        {
            var filePath = Path.Combine(@"\\share\databus_attachments\", message.LargeBlob.Key);
            var removeAttachment = new RemoveDatabusAttachment
            {
                FilePath = filePath
            };
            var options = new SendOptions();
            options.RouteToThisEndpoint();
            options.DelayDeliveryWith(TimeSpan.FromDays(30));
            return context.Send(removeAttachment, options);
        }

        public Task Handle(RemoveDatabusAttachment message, IMessageHandlerContext context)
        {
            var filePath = message.FilePath;
            // Code to clean up
            return Task.CompletedTask;
        }
    }

    #endregion

    public class RemoveDatabusAttachment :
        ICommand
    {
        public string FilePath { get; set; }
    }

    public class MessageWithLargePayload
    {
        public string SomeProperty { get; set; }
        public DataBusProperty<byte[]> LargeBlob { get; set; }
    }
}