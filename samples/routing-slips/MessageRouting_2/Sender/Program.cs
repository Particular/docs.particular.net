using NServiceBus.Logging;
using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageRouting.RoutingSlips;

class Program
{

    static ILog log = LogManager.GetLogger(typeof(Program));

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.RoutingSlips.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.RoutingSlips.Sender");

        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        #region enableRoutingSlips
        endpointConfiguration.EnableFeature<RoutingSlips>();
        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);


        Console.WriteLine("Press Enter to send a message");
        Console.WriteLine("Press any other key to exit");
        #region alternate
        var toggle = false;
        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            if (toggle)
            {
                await SendToABC(endpoint);
            }
            else
            {
                await SendToAC(endpoint);
            }

            toggle = !toggle;
        }
        #endregion
        await endpoint.Stop()
            .ConfigureAwait(false);
    }
    #region SendAC
    static Task SendToAC(IEndpointInstance endpoint)
    {
        var sequentialProcess = new SequentialProcess
        {
            StepAInfo = "Foo",
            StepCInfo = "Baz",
        };

        log.Info("Sending message for step A, C");
        return endpoint.Route(sequentialProcess, Guid.NewGuid(),
            "Samples.RoutingSlips.StepA",
            "Samples.RoutingSlips.StepC",
            "Samples.RoutingSlips.ResultHost");
    }
    #endregion

    #region SendABC
    static Task SendToABC(IEndpointInstance endpoint)
    {
        #region multi-message-Send
        var sequentialProcess = new SequentialProcess
        {
            StepAInfo = "Foo",
            StepBInfo = "Bar",
            StepCInfo = "Baz",
        };
        #endregion

        log.Info("Sending message for step A, B, C");
        return endpoint.Route(sequentialProcess, Guid.NewGuid(),
            "Samples.RoutingSlips.StepA",
            "Samples.RoutingSlips.StepB",
            "Samples.RoutingSlips.StepC",
            "Samples.RoutingSlips.ResultHost");
    }
    #endregion
}