using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.MessageDurability.Sender";
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
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
