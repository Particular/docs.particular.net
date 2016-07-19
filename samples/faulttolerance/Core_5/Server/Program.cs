using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.FaultTolerance.Server";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.FaultTolerance.Server");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        #region diableSlr
        //busConfiguration.DisableFeature<NServiceBus.Features.SecondLevelRetries>();
        #endregion
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
