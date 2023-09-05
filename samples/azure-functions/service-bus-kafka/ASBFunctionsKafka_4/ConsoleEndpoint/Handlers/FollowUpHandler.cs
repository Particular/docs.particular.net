using NServiceBus;

namespace ConsoleEndpoint.Handlers;

public class FollowUpHandler : IHandleMessages<FollowUp>
{
    public Task Handle(FollowUp message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Received message from Kafka trigger via Azure Service Bus: {message.Value}");

        return Task.CompletedTask;
    }
}