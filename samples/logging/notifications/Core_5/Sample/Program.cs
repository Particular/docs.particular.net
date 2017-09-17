using System;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Notifications";
        #region logging
        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Fatal);
        #endregion
        #region endpointConfig
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Notifications");
        #endregion
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var message = new MyMessage
            {
                Property = "PropertyValue"
            };
            bus.SendLocal(message);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}