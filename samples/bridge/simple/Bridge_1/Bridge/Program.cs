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
                #region endpoint-adding-simple

                var learningLeft = new BridgeTransport(new LearningTransport());
                learningLeft.HasEndpoint("Samples.Bridge.Sender");

                #endregion

                #region endpoint-adding-register-publisher-by-type

                var leftReceiver = new BridgeEndpoint("Samples.Bridge.LeftReceiver");
                leftReceiver.RegisterPublisher(typeof(OrderReceived), "Samples.Bridge.Sender");

                #endregion

                var learningRight = new BridgeTransport(new LearningTransport());

                #region endpoint-adding-register-publisher-by-string

                var rightReceiver = new BridgeEndpoint("Samples.Bridge.RightReceiver");
                rightReceiver.RegisterPublisher("Messages.Events.OrderPlaced", "Samples.Bridge.Sender");

                #endregion

                #region add-transports-to-bridge

                bridgeConfiguration.AddTransport(learningLeft);
                bridgeConfiguration.AddTransport(learningRight);

                #endregion
            })
            .Build()
            .RunAsync().ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}