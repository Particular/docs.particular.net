using System;
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
        Console.Title = "Samples.CustomErrorHandling";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Warn);

        var configure = new EndpointConfiguration("Samples.CustomErrorHandling");
        configure.UseSerialization<JsonSerializer>();
        configure.UsePersistence<InMemoryPersistence>();
        configure.EnableInstallers();
        configure.SendFailedMessagesTo("error");

        var recoverability = configure.Recoverability();
        recoverability.Delayed(
            customizations: delayedRetriesSettings =>
            {
                delayedRetriesSettings.NumberOfRetries(0);
            });

        #region Registering-Behavior

        var pipeline = configure.Pipeline;
        pipeline.Register(
            behavior: new CustomErrorHandlingBehavior(),
            description: "Manages thrown exceptions instead of delayed retries.");

        #endregion

        var endpointInstance = await Endpoint.Start(configure)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press enter to send a message that will throw an exception or \r\n" +
                              "Press [E] key to send a message failing with the custom exception.");
            Console.WriteLine("Press [ESC] key to exit");

            while (true)
            {
                var input = Console.ReadKey();

                var myMessage = new MyMessage
                {
                    Id = Guid.NewGuid(),
                    ThrowCustomException = input.Key == ConsoleKey.E
                };

                if (input.Key == ConsoleKey.Escape)
                {
                    break;
                }
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