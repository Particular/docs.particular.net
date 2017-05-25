namespace Core3.CleanupStrategy
{
    using System;
    using System.IO;
    using NServiceBus;

    #region HandlerThatCleansUpDatabus

    public class Handler :
        IHandleMessages<MessageWithLargePayload>,
        IHandleMessages<RemoveDatabusAttachment>
    {
        IBus bus;

        public Handler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MessageWithLargePayload message)
        {
            var filePath = Path.Combine(@"\\share\databus_attachments\", message.LargeBlob.Key);
            var removeAttachment = new RemoveDatabusAttachment
            {
                FilePath = filePath
            };
            bus.Defer(TimeSpan.FromDays(30), removeAttachment);
        }

        public void Handle(RemoveDatabusAttachment message)
        {
            var filePath = message.FilePath;
            // Code to clean up
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