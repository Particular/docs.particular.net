using NServiceBus;
using NServiceBus.Gateway;
using NServiceBus.MessageMutator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

class Usage
{
    void GatewayConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayConfiguration

        endpointConfiguration.Gateway(new InMemoryDeduplicationConfiguration());

        #endregion
    }

    void GatewayDefaultRetryPolicyConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayDefaultRetryPolicyConfiguration

        var gateway = endpointConfiguration.Gateway(new InMemoryDeduplicationConfiguration());
        gateway.Retries(5, TimeSpan.FromMinutes(1));

        #endregion
    }

    void GatewayCustomRetryPolicyConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayCustomRetryPolicyConfiguration

        var gateway = endpointConfiguration.Gateway(new InMemoryDeduplicationConfiguration());
        gateway.CustomRetryPolicy(
            customRetryPolicy: (message, exception, currentRetry) =>
            {
                if (currentRetry > 4)
                {
                    return TimeSpan.MinValue;
                }
                return TimeSpan.FromSeconds(currentRetry * 60);
            });

        #endregion
    }

    void GatewayDisableRetriesConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayDisableRetriesConfiguration

        var gateway = endpointConfiguration.Gateway(new InMemoryDeduplicationConfiguration());
        gateway.DisableRetries();

        #endregion
    }

    async Task SendToSites(IEndpointInstance endpoint)
    {
        #region SendToSites

        await endpoint.SendToSites(new[]
        {
            "SiteA",
            "SiteB"
        }, new MyMessage());

        #endregion
    }

    void TransactionTimeout(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayCustomTransactionTimeout

        var gateway = endpointConfiguration.Gateway(new InMemoryDeduplicationConfiguration());
        gateway.TransactionTimeout(TimeSpan.FromSeconds(40));

        #endregion
    }

    public void InMemoryDeduplicationConfigurationCacheSize(EndpointConfiguration endpointConfiguration)
    {
        #region NonDurableDeduplicationConfigurationCacheSize

        var gatewayStorageConfiguration = new InMemoryDeduplicationConfiguration
        {
            CacheSize = 50000
        };

        endpointConfiguration.Gateway(gatewayStorageConfiguration);

        #endregion
    }

    void ReplyUri(EndpointConfiguration endpointConfiguration)
    {
        #region SetReplyToUri

        var gatewaySettings = endpointConfiguration.Gateway(new InMemoryDeduplicationConfiguration());

        // Local HTTP binding address uses wilcard domain
        gatewaySettings.AddReceiveChannel("http://+:12345/MyEndpoint/");

        // Set the reply-to URI as the public address of a load balancer or proxy
        gatewaySettings.SetReplyToUri("http://my-public-domain.com:54321/MyEndpoint/");

        #endregion
    }
    
    class CustomChannelReceiver :
        IChannelReceiver
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

    class CustomChannelSender :
        IChannelSender
    {
        public Task Send(string remoteAddress, IDictionary<string, string> headers, Stream data)
        {
            throw new NotImplementedException();
        }
    }

}