using System;
using NServiceBus;

class Program
{
    #region ServerInit
    static void Main()
    {
        Console.Title = "Samples.StepByStep.Server";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.StepByStep.Server");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
    #endregion
}
