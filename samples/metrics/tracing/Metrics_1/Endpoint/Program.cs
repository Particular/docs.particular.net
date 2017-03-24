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
        Console.Title = "Samples.Metrics.Tracing.Endpoint";
        var endpointConfig = new EndpointConfiguration("Samples.Metrics.Tracing.Endpoint");
        endpointConfig.SendFailedMessagesTo("error");
        endpointConfig.UsePersistence<InMemoryPersistence>();
        endpointConfig.EnableInstallers();

        #region EnableMetricTracing

        var metricsOptions = endpointConfig.EnableMetrics();
        metricsOptions.EnableMetricTracing(TimeSpan.FromSeconds(5));

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfig)
            .ConfigureAwait(false);

        try
        {
            Console.WriteLine("Endpoint Started. Press any key to send a message");
            Console.WriteLine("Press [ESC] to quit");

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }

                await endpointInstance.SendLocal(new SomeCommand())
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}