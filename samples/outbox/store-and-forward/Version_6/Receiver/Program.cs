using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
        AsyncRun().GetAwaiter().GetResult();
    }

    static async Task AsyncRun()
    {
        #region ReceiverConfiguration

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.UseTransport<SqlServerTransport>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=sender;Integrated Security=True");

        endpointConfiguration.UsePersistence<NHibernatePersistence>();
        endpointConfiguration.EnableOutbox();

        #endregion

        endpointConfiguration.DisableFeature<SecondLevelRetries>();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        
        await endpoint.Stop();
    }
}