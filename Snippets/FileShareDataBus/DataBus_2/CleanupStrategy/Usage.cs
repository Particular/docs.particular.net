namespace CleanupStrategy;

using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;

#region HandlerThatCleansUpDatabus

public class Handler :
    NServiceBus.IHandleMessages<MessageWithLargePayload>,
    NServiceBus.IHandleMessages<RemoveDatabusAttachment>
{

    public Task Handle(MessageWithLargePayload message, NServiceBus.IMessageHandlerContext context)
    {
        var filePath = Path.Combine(@"\\share\databus_attachments\", message.LargeBlob.Key);
        var removeAttachment = new RemoveDatabusAttachment
        {
            FilePath = filePath
        };
        var options = new NServiceBus.SendOptions();
        options.RouteToThisEndpoint();
        options.DelayDeliveryWith(TimeSpan.FromDays(30));
        return context.Send(removeAttachment, options);
    }

    public Task Handle(RemoveDatabusAttachment message, NServiceBus.IMessageHandlerContext context)
    {
        var filePath = message.FilePath;
        // Code to clean up
        return Task.CompletedTask;
    }
}

#endregion

public class RemoveDatabusAttachment :
    NServiceBus.ICommand
{
    public string FilePath { get; set; }
}

public class MessageWithLargePayload
{
    public string SomeProperty { get; set; }
    public ClaimCheckProperty<byte[]> LargeBlob { get; set; }
}