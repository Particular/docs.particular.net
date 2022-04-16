using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Bridge";

        await Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = false;
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss ";
                });
            })
            .UseNServiceBusBridge((ctx, bridgeConfiguration) =>
            {
                var learningLeft = new BridgeTransport(new LearningTransport());
                learningLeft.HasEndpoint("Samples.Bridge.Sender");

                var leftReceiver = new BridgeEndpoint("Samples.Bridge.LeftReceiver");
                leftReceiver.RegisterPublisher(typeof(OrderReceived), "Samples.Bridge.Sender");

                var learningRight = new BridgeTransport(new LearningTransport());

                var billing = new BridgeEndpoint("Samples.Bridge.RightReceiver");
                billing.RegisterPublisher("Messages.Events.OrderPlaced", "Samples.Bridge.Sender");

                bridgeConfiguration.AddTransport(learningLeft);
                bridgeConfiguration.AddTransport(learningRight);
            })
            .Build()
            .RunAsync().ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}