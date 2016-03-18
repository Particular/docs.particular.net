using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.CustomTransport.Endpoint1";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.CustomTransport.Endpoint1");
#region UseDefinition
        endpointConfiguration.UseTransport<FileTransport>();
#endregion
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.DisableFeature<TimeoutManager>();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            #region StartMessageInteraction
            await endpoint.Send("Samples.CustomTransport.Endpoint2", new MessageA());
            #endregion
            Console.WriteLine("MessageA sent. Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}