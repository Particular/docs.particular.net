using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MessageBodyEncryption.Endpoint1";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MessageBodyEncryption.Endpoint1");
        busConfiguration.UsePersistence<InMemoryPersistence>();

        #region RegisterMessageEncryptor

        busConfiguration.RegisterMessageEncryptor();

        #endregion

        var startableBus = Bus.Create(busConfiguration);
        using (var bus = startableBus.Start())
        {
            var completeOrder = new CompleteOrder
            {
                CreditCard = "123-456-789"
            };
            bus.Send("Samples.MessageBodyEncryption.Endpoint2", completeOrder);
            Console.WriteLine("Message sent");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}