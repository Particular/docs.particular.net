using System;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Logging.CustomFactory";
        #region ConfigureLogging

        var loggerDefinition = LogManager.Use<ConsoleLoggerDefinition>();
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.CustomFactory");

        #endregion
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}