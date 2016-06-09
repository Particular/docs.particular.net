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
        #region Sending
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Sending messages...");
            for (var i = 0; i < 100; i++)
            {
                var searchGitHub = new SearchGitHub
                {
                    Repository = "NServiceBus",
                    RepositoryOwner = "Particular",
                    SearchFor = "IBus"
                };
                bus.Send("Samples.Throttling.Limited",searchGitHub);
            }
            Console.WriteLine("Messages sent.");
            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
        }
        #endregion
    }
}