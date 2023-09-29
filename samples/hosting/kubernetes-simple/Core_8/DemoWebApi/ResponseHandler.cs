using NServiceBus;
using Shared;

namespace DemoWebApi;

public class ResponseHandler : IHandleMessages<DemoResponse>
{
    public Task Handle(DemoResponse message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Received response {message.Id}");
        return Task.CompletedTask;
    }
}