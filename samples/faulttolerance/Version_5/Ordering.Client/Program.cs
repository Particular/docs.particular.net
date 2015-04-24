using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.FaultTolerance.Client");
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfig))
        {
            bus.Start();
            Console.WriteLine("Press 'Enter' to send a message.To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {
                Guid id = Guid.NewGuid();

                bus.Send("Samples.FaultTolerance.Server", new MyMessage
                {
                    Id = id
                });

                Console.WriteLine("Sent a new message with id: {0}", id.ToString("N"));
            }

        }
    }
}
