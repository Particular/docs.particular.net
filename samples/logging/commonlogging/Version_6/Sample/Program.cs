using System;
using System.Threading.Tasks;
using Common.Logging;
using NServiceBus;
using Common.Logging.Simple;
// ReSharper disable RedundantNameQualifier

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Logging.CommonLogging";
        #region ConfigureLogging

        Common.Logging.LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter
        {
            Level = LogLevel.Info
        };

        NServiceBus.Logging.LogManager.Use<CommonLoggingFactory>();

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.Logging.CommonLogging");

        #endregion

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await endpoint.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}
