using System;
using System.Configuration;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Server");
        busConfiguration.EndpointInstanceId(() => ConfigurationManager.AppSettings["InstanceId"]);
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");
        Run(busConfiguration).GetAwaiter().GetResult();
    }

    static async Task Run(BusConfiguration busConfig)
    {
        IEndpointInstance endpoint = await Endpoint.Start(busConfig);
        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();
        await endpoint.Stop();
    }
}