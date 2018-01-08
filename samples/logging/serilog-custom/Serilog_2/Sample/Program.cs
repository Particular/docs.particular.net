using System;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using Serilog;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Logging.SerilogCustom";
        #region ConfigureSerilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.ColoredConsole()
            .WriteTo.File("logFile.txt")
            .CreateLogger();
        #endregion

        #region UseConfig
        LogManager.Use<SerilogFactory>();

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.SerilogCustom");

        #endregion

        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        #region Cleanup
        Log.CloseAndFlush();
        #endregion
    }
}
