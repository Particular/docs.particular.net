using System;
using log4net.Config;
using NServiceBus;
using NServiceBus.Log4Net;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        #region ConfigureAppender
        MyConsoleAppender consoleAppender = new MyConsoleAppender
                              {
                                  Color = ConsoleColor.Green
                              };
        BasicConfigurator.Configure(consoleAppender);
        LogManager.Use<Log4NetFactory>();
        #endregion

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Log4Net.Appender");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new MyMessage());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
