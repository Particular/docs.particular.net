using System;
using System.Threading.Tasks;
using NServiceBus;

public static class Program
{
    public static async Task Main()
    {
        var config = new EndpointConfiguration(EndpointNames.WorkGenerator);

        config.UsePersistence<LearningPersistence>();
        config.UseSerialization<SystemJsonSerializer>();
        config.AuditProcessedMessagesTo("audit");
        config.SendFailedMessagesTo("error");

        var transport = config.UseTransport<LearningTransport>();
        var routing = transport.Routing();
        var endpoint = await config.StartWithDefaultRoutes(routing);

        Console.WriteLine("Started.");
        Console.WriteLine("Press 'S' to start a new process or [Enter] to exit.");

        while (true)
        {
            var pressedKey = Console.ReadKey();
            switch (pressedKey.Key)
            {
                case ConsoleKey.Enter:
                    {
                        return;
                    }
                case ConsoleKey.S:
                    {
                        await StartSaga(endpoint);
                        break;
                    }
            }
        }
    }

    static async Task StartSaga(IMessageSession session)
    {
        var id = Guid.NewGuid();
        var count = Random.Shared.Next(500, 1000);

        await session.SendLocal(new StartProcessing
        {
            ProcessId = id,
            WorkCount = count
        });
        Console.WriteLine($"Started process '{id}' with '{count}' work orders.");
    }
}