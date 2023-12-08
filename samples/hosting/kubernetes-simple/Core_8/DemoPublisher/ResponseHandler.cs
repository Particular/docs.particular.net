using NServiceBus;
using Shared;

public class ResponseHandler : IHandleMessages<DemoResponse>
{
    public Task Handle(DemoResponse message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Received response {message.Id}");
        return Task.CompletedTask;
    }
}