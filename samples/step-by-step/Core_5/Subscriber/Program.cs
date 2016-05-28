using System;
using NServiceBus;

class Program
{
    #region SubscriberInit
    static void Main()
    {
        Console.Title = "Samples.StepByStep.Subscriber";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.StepByStep.Subscriber");
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