using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Gateway;

class Usage
{
    public void NonDurableDeduplicationConfigurationCacheSize(EndpointConfiguration endpointConfiguration)
    {
        #region NonDurableDeduplicationConfigurationCacheSize

        var gatewayStorageConfiguration = new NonDurableDeduplicationConfiguration
        {
            CacheSize = 50000,
        };

        endpointConfiguration.Gateway(gatewayStorageConfiguration);

        #endregion
    }
    
    void GatewayConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayConfiguration

        var config = new NonDurableDeduplicationConfiguration();
        var gateway = endpointConfiguration.Gateway(config);

        #endregion
    }

    void GatewayDefaultRetryPolicyConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayDefaultRetryPolicyConfiguration

        var config = new NonDurableDeduplicationConfiguration();
        var gateway = endpointConfiguration.Gateway(config);
        gateway.Retries(5, TimeSpan.FromMinutes(1));

        #endregion
    }

    void GatewayCustomRetryPolicyConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayCustomRetryPolicyConfiguration

        var config = new NonDurableDeduplicationConfiguration();
        var gateway = endpointConfiguration.Gateway(config);
        gateway.CustomRetryPolicy(
            customRetryPolicy: (message, exception, currentRetry) =>
            {
                if (currentRetry > 4)
                {
                    return TimeSpan.MinValue;
                }
                return TimeSpan.FromSeconds(currentRetry*60);
            });

        #endregion
    }
    
    
    void GatewayDisableRetriesConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayDisableRetriesConfiguration

        var config = new NonDurableDeduplicationConfiguration();
        var gateway = endpointConfiguration.Gateway(config);
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
            }, new MyMessage())
            .ConfigureAwait(false);

        #endregion
    }
    
    void TransactionTimeout(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayCustomTransactionTimeout

        var config = new NonDurableDeduplicationConfiguration();
        var gateway = endpointConfiguration.Gateway(config);
        gateway.TransactionTimeout(TimeSpan.FromSeconds(40));

        #endregion
    }
}
