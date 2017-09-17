using System;
using NServiceBus;

class Program
{
    static void Main()
    {

        Console.Title = "Samples.RabbitMQ.Simple";
        #region ConfigureRabbit

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.RabbitMQ.Simple");
        var transport = busConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=localhost");

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
    }
}