using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Sagas;

internal class SagaNotFoundHandler : IHandleSagaNotFound
{
    public Task Handle(object message, IMessageProcessingContext context)
    {
        ConsoleColor foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("The correlated saga could not be found. Have you configured the RavenDB.Bundle.UniqueConstrains on your RavenDB server?");
        Console.ForegroundColor = foregroundColor;

        return Task.FromResult(0);
    }
}