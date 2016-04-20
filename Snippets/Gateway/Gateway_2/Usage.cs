namespace Gateway_2
{
    using System;
    using NServiceBus;
    using NServiceBus.Features;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint)
        {
            #region GatewayConfiguration

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableFeature<Gateway>();

            #endregion

            #region GatewayDefaultRetryPolicyConfiguration

            endpointConfiguration.Gateway().Retries(numberOfRetries: 5, timeIncrease: TimeSpan.FromMinutes(1));

            #endregion

            #region GatewayCustomRetryPolicyConfiguration

            endpointConfiguration.Gateway().CustomRetryPolicy(message =>
            {
                var currentRetry = message.GetCurrentRetry();
                return currentRetry > 4 ? TimeSpan.Zero : TimeSpan.FromSeconds(currentRetry * 60);
            });

            #endregion

            #region GatewayDisableRetriesConfiguration

            endpointConfiguration.Gateway().DisableRetries();

            #endregion

            #region SendToSites

            endpoint.SendToSites(new[] { "SiteA", "SiteB" }, new MyMessage());

            #endregion
        }

    }
}
