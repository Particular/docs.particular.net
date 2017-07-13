using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Endpoint2";
        var endpointConfiguration = new EndpointConfiguration("Endpoint2");
        endpointConfiguration.UseTransport<LearningTransport>();
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(
            settings =>
            {
                settings.NumberOfRetries(0);
            });
        recoverability.Delayed(
            settings =>
            {
                settings.NumberOfRetries(0);
            });

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}