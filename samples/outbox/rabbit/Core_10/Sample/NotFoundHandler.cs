using System;
using System.Threading.Tasks;
using NServiceBus;

public class NotFoundHandler : ISagaNotFoundHandler
{
    public Task Handle(object message, IMessageProcessingContext context)
    {
        Console.WriteLine("Saga not found");
        return Task.CompletedTask;
    }
}