using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Azure.ServiceBus.Endpoint2");
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UseTransport<AzureServiceBusTransport>()
            .ConnectionString(Environment.GetEnvironmentVariable("SamplesAzureServiceBusConnection"));
        busConfiguration.UsePersistence<InMemoryPersistence>();

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusSession busSession = endpoint.CreateBusSession();
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
        finally
        {
            await endpoint.Stop();
        }

    }
}