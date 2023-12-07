using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

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
                learningLeft.HasEndpoint("Samples.Bridge.LeftSender");

                #endregion

                var learningTransport = new LearningTransport
                {
                    // Set storage directory and add the character '2' to simulate a different transport.
                    StorageDirectory = $"{LearningTransportInfrastructure.FindStoragePath()}2"
                };
                var learningRight = new BridgeTransport(learningTransport)
                {
                    // A different name is required if transports are used twice.
                    Name = "right-side"
                };

                #region endpoint-adding-register-publisher-by-string

                var rightReceiver = new BridgeEndpoint("Samples.Bridge.RightReceiver");
                rightReceiver.RegisterPublisher("OrderReceived", "Samples.Bridge.LeftSender");

                #endregion
                learningRight.HasEndpoint(rightReceiver);

                #region add-transports-to-bridge

                bridgeConfiguration.AddTransport(learningLeft);
                bridgeConfiguration.AddTransport(learningRight);

                #endregion
            })
            .Build()
            .RunAsync().ConfigureAwait(false);
    }
}