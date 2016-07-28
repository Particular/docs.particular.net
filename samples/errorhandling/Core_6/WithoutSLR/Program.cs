﻿using System;
using System.Threading.Tasks;
using NServiceBus;
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
            .Level(LogLevel.Info);

        #region DisableSLR
        var endpointConfiguration = new EndpointConfiguration("Samples.ErrorHandling.WithoutSLR");
        endpointConfiguration.Recoverability().Delayed(delayed => delayed.NumberOfRetries(0));
        #endregion
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press enter to send a message that will throw an exception.");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var myMessage = new MyMessage
                {
                    Id = Guid.NewGuid()
                };
                await endpointInstance.SendLocal(myMessage)
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}