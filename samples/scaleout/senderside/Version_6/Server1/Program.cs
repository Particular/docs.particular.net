using System;
using System.Threading.Tasks;
using NServiceBus;
using System.Configuration;

class Program
{
    static void Main()
    {
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.EndpointName("Server");

        #region Server-Set-InstanceId

        busConfig.EndpointInstanceId(() => ConfigurationManager.AppSettings["InstanceId"]);

        #endregion

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