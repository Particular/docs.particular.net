using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.ErrorHandling.WithoutSLR";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Warn);

        #region DisableSLR
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.ErrorHandling.WithoutSLR");
        endpointConfiguration.DisableFeature<SecondLevelRetries>();
        #endregion
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Console.WriteLine("Press enter to send a message that will throw an exception.");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                MyMessage m = new MyMessage
                {
                    Id = Guid.NewGuid()
                };
                await endpoint.SendLocal(m);
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}