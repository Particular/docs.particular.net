using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence;

class Program
{
    const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=AutomaticRouting;Integrated Security=True";

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.CustomRouting.Sales.2";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomRouting.Sales");
        endpointConfiguration.ScaleOut().InstanceDiscriminator("2");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<NHibernatePersistence>().ConnectionString(ConnectionString);
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration
            .EnableAutomaticRouting(ConnectionString)
            .AdvertisePublishing(typeof(OrderAccepted));

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