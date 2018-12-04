using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Microsoft.Extensions.Logging;
using NServiceBus.Logging;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

class Program
{
    public static async Task Main()
    {
        Console.Title = "Samples.Logging.MsLoggingCustom";

        #region MsLoggingInCode

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddFilter(level => level >= LogLevel.Information);
            loggingBuilder.AddConsole();
        });

        var serviceProvider = serviceCollection.BuildServiceProvider();

        using (var loggerFactory = serviceProvider.GetService<ILoggerFactory>())
        {
            var logFactory = LogManager.Use<MicrosoftLogFactory>();
            logFactory.UseMsFactory(loggerFactory);

            var endpointConfiguration = new EndpointConfiguration("Samples.Logging.MsLoggingCustom");
            await ConfigureAndRunEndpoint(endpointConfiguration)
                .ConfigureAwait(false);
        }
        #endregion
    }

    static async Task ConfigureAndRunEndpoint(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}