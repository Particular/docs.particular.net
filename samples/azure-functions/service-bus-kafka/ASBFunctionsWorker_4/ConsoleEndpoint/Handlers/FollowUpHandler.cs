using AzureFunctions.Messages.NServiceBusMessages;
using NServiceBus;

namespace ConsoleEndpoint.Handlers;

public class FollowUpHandler : IHandleMessages<FollowUp>
{
    public Task Handle(FollowUp message, IMessageHandlerContext context)
    {
        Console.WriteLine($"User [{message.CustomerId}] for unit [{message.UnitId}] has message: {message.Description}");

        return Task.CompletedTask;
    }
}