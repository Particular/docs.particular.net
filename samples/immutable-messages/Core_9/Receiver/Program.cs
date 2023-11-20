using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ImmutableMessages.UsingInterfaces.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.ImmutableMessages.UsingInterfaces.Receiver");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport(new LearningTransport());

        endpointConfiguration.ApplyCustomConventions();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Samples.ImmutableMessages.UsingInterfaces.Receiver started. Press any key to exit.");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}

