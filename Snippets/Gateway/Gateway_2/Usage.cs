namespace Gateway_2
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Gateway;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint)
        {
            #region GatewayConfiguration

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableFeature<Gateway>();

            #endregion

            #region GatewayDefaultRetryPolicyConfiguration

            endpointConfiguration.Gateway().Retries(5, TimeSpan.FromMinutes(1));

            #endregion

            #region GatewayCustomRetryPolicyConfiguration

            endpointConfiguration.Gateway().CustomRetryPolicy((message, exception, currentRetry) => { return currentRetry > 4 ? TimeSpan.MinValue : TimeSpan.FromSeconds(currentRetry*60); });

            #endregion

            #region GatewayDisableRetriesConfiguration

            endpointConfiguration.Gateway().DisableRetries();

            #endregion

            #region GatewayChannelFactoriesConfiguration

            endpointConfiguration.Gateway().ChannelFactories(channelType => { return new CustomChannelSender(); }, channelType => { return new CustomChannelReceiver(); });

            #endregion

            #region SendToSites

            endpoint.SendToSites(new[]
            {
                "SiteA",
                "SiteB"
            }, new MyMessage());

            #endregion
        }

        class CustomChannelReceiver : IChannelReceiver
        {
            public void Start(string address, int maxConcurrency, Func<DataReceivedOnChannelArgs, Task> dataReceivedOnChannel)
            {
                throw new NotImplementedException();
            }

            public Task Stop()
            {
                throw new NotImplementedException();
            }
        }

        class CustomChannelSender : IChannelSender
        {
            public Task Send(string remoteAddress, IDictionary<string, string> headers, Stream data)
            {
                throw new NotImplementedException();
            }
        }
    }
}