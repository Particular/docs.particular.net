using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.MessageDurability.Sender";
        #region non-transactional
        var busConfiguration = new BusConfiguration();
        busConfiguration.Transactions()
            .Disable();
        #endregion
        busConfiguration.EndpointName("Samples.MessageDurability.Sender");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            bus.Send("Samples.MessageDurability.Receiver", new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
