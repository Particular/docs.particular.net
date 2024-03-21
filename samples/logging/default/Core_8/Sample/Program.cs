using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "LoggingDefault";
        #region ConfigureLogging

        var defaultFactory = NServiceBus.Logging.LogManager.Use<NServiceBus.Logging.DefaultFactory>();

        // The default logging directory is HttpContext.Current.Server.MapPath("~/App_Data/") for websites
        // and AppDomain.CurrentDomain.BaseDirectory for all other processes.
        defaultFactory.Directory(".");

        // Default is LogLevel.Info
        defaultFactory.Level(NServiceBus.Logging.LogLevel.Debug);

        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.Default");
        #endregion
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
