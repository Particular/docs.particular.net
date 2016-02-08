using System;
using System.Configuration;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.EndpointName("Server");
        busConfig.EndpointInstanceId(() => ConfigurationManager.AppSettings["InstanceId"]);
        busConfig.UsePersistence<InMemoryPersistence>();
        busConfig.SendFailedMessagesTo("error");
        Run(busConfig).GetAwaiter().GetResult();
    }

    static async Task Run(BusConfiguration busConfig)
    {
        IEndpointInstance endpoint = await Endpoint.Start(busConfig);
        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();
        await endpoint.Stop();
    }
}