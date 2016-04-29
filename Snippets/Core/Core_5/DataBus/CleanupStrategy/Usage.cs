namespace Core5.DataBus.CleanupStrategy
{
    using System;
    using System.IO;
    using DataBusProperty;
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
            string filePath = Path.Combine(@"\\share\databus_attachments\", message.LargeBlob.Key);
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

    public class RemoveDatabusAttachment : ICommand
    {
        public string FilePath { get; set; }
    }
}
