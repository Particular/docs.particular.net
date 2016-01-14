using System;
using NServiceBus.Saga;

internal class SagaNotFoundHandler : IHandleSagaNotFound
{
    public void Handle(object message)
    {
        ConsoleColor foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("The correlated saga could not be found. Have you configured the RavenDB.Bundle.UniqueConstrains on your RavenDB server?");
        Console.ForegroundColor = foregroundColor;
    }
}