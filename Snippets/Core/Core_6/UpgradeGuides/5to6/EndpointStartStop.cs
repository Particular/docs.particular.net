namespace Core6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class EndpointStartStop
    {
        async Task StartEndpoint()
        {
            #region 5to6-endpoint-start-stop
            var endpointConfiguration = new EndpointConfiguration("EndpointName");

            // Custom code before start
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            // Custom code after start

            // Block the process

            // Custom code before stop
            await endpointInstance.Stop()
                .ConfigureAwait(false);
            // Custom code after stop
            #endregion
        }

        async Task SendMessagesOutsideMessageHandler(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-endpoint-send-messages-outside-handlers
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            IMessageSession messageSession = endpointInstance;
            await messageSession.Send(new SomeMessage())
                .ConfigureAwait(false);
            #endregion
        }

    }

    class EndpointStartStopFullAsync
    {
        #region v6-endpoint-start-stop-full-async

        async Task Run(EndpointConfiguration config)
        {
            // pre startup
            var endpointInstance = await Endpoint.Start(config)
                .ConfigureAwait(false);
            // post startup

            // block process

            // pre shutdown
            await endpointInstance.Stop()
                .ConfigureAwait(false);
            // post shutdown
        }

        #endregion
    }

    class EndpointStartStopSyncToAsync
    {
        #region v6-endpoint-start-stop-sync-wrapper

        void Run(EndpointConfiguration config)
        {
            RunAsync(config).GetAwaiter().GetResult();
        }

        async Task RunAsync(EndpointConfiguration config)
        {
            // pre startup
            var endpointInstance = await Endpoint.Start(config)
                .ConfigureAwait(false);
            // post startup

            // block process

            // pre shutdown
            await endpointInstance.Stop()
                .ConfigureAwait(false);
            // post shutdown
        }

        #endregion
    }

    class SomeMessage
    {
    }
}
