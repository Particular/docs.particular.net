using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("FullDuplexSample_Client");
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
                bus.Send("FullDuplexSample_Server", message);
            }
            #endregion
        }
    }
}