using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        #region non-transactional
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.Transactions()
            .Disable();
        #endregion
        busConfiguration.EndpointName("Samples.MessageDurability.Sender");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.Send("Samples.MessageDurability.Receiver", new MyMessage());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
