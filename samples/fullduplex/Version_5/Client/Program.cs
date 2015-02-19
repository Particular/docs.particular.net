using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.FullDuplex.Client");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            Console.WriteLine("Press 'Enter' to send a message.To exit, Ctrl + C");
            #region ClientLoop
            while (Console.ReadLine() != null)
            {
                var g = Guid.NewGuid();
                Console.WriteLine("Requesting to get data by id: {0}", g.ToString("N"));

                var message = new RequestDataMessage
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