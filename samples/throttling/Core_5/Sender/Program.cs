using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Throttling.Sender";

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Throttling.Sender");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Sending messages...");
            #region Sending
            for (var i = 0; i < 100; i++)
            {
                var searchGitHub = new SearchGitHub
                {
                    Repository = "NServiceBus",
                    Owner = "Particular",
                    SearchFor = "IBus"
                };
                bus.Send("Samples.Throttling.Limited",searchGitHub);
            }
            #endregion
            Console.WriteLine("Messages sent.");
            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
        }
    }
}