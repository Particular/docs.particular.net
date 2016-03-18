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
        Console.Title = "Samples.Azure.ServiceBus.Endpoint2";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.Endpoint2");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseTransport<AzureServiceBusTransport>()
            .ConnectionString(Environment.GetEnvironmentVariable("SamplesAzureServiceBusConnection"));
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }

    }
}