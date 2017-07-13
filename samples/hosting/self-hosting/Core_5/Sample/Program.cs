using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.SelfHosting";
        #region self-hosting

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SelfHosting");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);
            Console.ReadKey();
        }

        #endregion
    }
}