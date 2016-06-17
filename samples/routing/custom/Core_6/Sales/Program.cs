using System;
using System.Threading.Tasks;
using NServiceBus;
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
        Console.Title = "Samples.CustomRouting.Sales.1";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomRouting.Sales");
        endpointConfiguration.ScaleOut().InstanceDiscriminator("1");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<NHibernatePersistence>()
            .ConnectionString(ConnectionString);
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region EnableAutomaticRouting

        endpointConfiguration
            .EnableAutomaticRouting(ConnectionString)
            .AdvertisePublishing(typeof(OrderAccepted));

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}