using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.FullDuplex.Client");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'Enter' to send a message.To exit, Ctrl + C");

            #region ClientLoop

            while (Console.ReadLine() != null)
            {
                Guid g = Guid.NewGuid();
                Console.WriteLine("Requesting to get data by id: {0}", g.ToString("N"));

                RequestDataMessage message = new RequestDataMessage
                                             {
                                                 DataId = g,
                                                 String = "<node>it's my \"node\" & i like it<node>"
                                             };
                bus.Send("Samples.FullDuplex.Server", message);
            }

            #endregion
        }
    }
}