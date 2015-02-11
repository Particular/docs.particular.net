using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MessageBodyEncryption.Endpoint1");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.RegisterMessageEncryptor();
        var startableBus = Bus.Create(busConfiguration);
        using (var bus = startableBus.Start())
        {
            var completeOrder = new CompleteOrder
                                          {
                                              CreditCard = "123-456-789"
                                          };
            bus.Send("Samples.MessageBodyEncryption.Endpoint2", completeOrder);
            Console.WriteLine("Message sent. Press any key to exit");
            Console.ReadLine();
        }
    }
}