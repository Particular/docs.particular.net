using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main(string[] args)
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        var config = new EndpointConfiguration("SampleEndpoint");
        config.UseTransport<MsmqTransport>();
        config.UsePersistence<InMemoryPersistence>();
        config.SendFailedMessagesTo("error");

        #region configure-custom-checks

        config.CustomCheckPlugin("Particular.ServiceControl");
        
        #endregion

        var endpoint = await Endpoint.Start(config).ConfigureAwait(false);

        Console.WriteLine("Endpoint Started");

        Console.ReadLine();

        await endpoint.Stop().ConfigureAwait(false);
    }
}