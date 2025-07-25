using System;
using System.Threading.Tasks;
using NServiceBus;

public static class Program
{
    public static async Task Main()
    {
        var config = new EndpointConfiguration(EndpointNames.WorkProcessor);

        config.UsePersistence<LearningPersistence>();
        config.UseTransport<LearningTransport>();
        //config.UseSerialization<NewtonsoftJsonSerializer>();
        config.UseSerialization<SystemJsonSerializer>();
        config.AuditProcessedMessagesTo("audit");
        config.SendFailedMessagesTo("error");
        config.LimitMessageProcessingConcurrencyTo(64);

        var transport = config.UseTransport<LearningTransport>();
        var routing = transport.Routing();
        var _ = await config.StartWithDefaultRoutes(routing);

        Console.WriteLine("Started.");
        Console.WriteLine("Press [Enter] to exit.");

        while (true)
        {
            var pressedKey = Console.ReadKey();
            switch (pressedKey.Key)
            {
                case ConsoleKey.Enter:
                    {
                        return;
                    }
            }
        }
    }
}