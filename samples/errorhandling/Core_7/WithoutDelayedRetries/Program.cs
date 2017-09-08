using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.ErrorHandling.WithoutDelayedRetries";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Warn);

        #region Disable

        var endpointConfiguration = new EndpointConfiguration("Samples.ErrorHandling.WithoutDelayedRetries");
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            customizations: delayed =>
            {
                delayed.NumberOfRetries(0);
            });

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to send a message that will throw an exception.");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            var myMessage = new MyMessage
            {
                Id = Guid.NewGuid()
            };
            await endpointInstance.SendLocal(myMessage)
                .ConfigureAwait(false);
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}