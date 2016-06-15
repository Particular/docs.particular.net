using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence;
using NServiceBus.Routing;

class Program
{
    const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=AutomaticRouting;Integrated Security=True";

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.FlowControl.Server.1";
        var endpointConfiguration = new EndpointConfiguration("Samples.FlowControl.Server");
        endpointConfiguration.ScaleOut().InstanceDiscriminator("1");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);

        endpointConfiguration.EnableFeature<FlowControl>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}