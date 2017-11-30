using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using Serilog;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Logging.SerilogCustom";
        var directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var logFilePath = Path.Combine(directoryName, "logFile.txt");
        #region ConfigureSerilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(logFilePath)
            .CreateLogger();
        #endregion

        #region UseConfig
        LogManager.Use<SerilogFactory>();

        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.SerilogCustom");

        #endregion

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
