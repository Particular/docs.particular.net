using System;
using System.Threading.Tasks;
using NServiceBus;

public class Startup : IWantToRunWhenBusStartsAndStops
{
    public async Task Start(IBusSession session)
    {
        Console.WriteLine("sending message...");
        for (int i = 0; i < 100; i++)
        {
            await session.SendLocal(new SearchGitHub
            {
                Repository = "NServiceBus",
                RepositoryOwner = "Particular",
                SearchFor = "IBus"
            });
        }
        Console.WriteLine("message sent.");
    }

    public Task Stop(IBusSession session)
    {
        return Task.FromResult(0);
    }
}