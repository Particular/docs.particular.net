using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Gateway;
using NServiceBus.MessageMutator;

class Usage
{
    void GatewayConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayConfiguration

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableFeature<Gateway>();

        #endregion
    }

    void GatewayDefaultRetryPolicyConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayDefaultRetryPolicyConfiguration

        var gateway = endpointConfiguration.Gateway();
        gateway.Retries(5, TimeSpan.FromMinutes(1));

        #endregion
    }

    void GatewayCustomRetryPolicyConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region GatewayCustomRetryPolicyConfiguration

        var gateway = endpointConfiguration.Gateway();
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

        var gateway = endpointConfiguration.Gateway();
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

    #region HeaderMutator

    public class AddRequiredHeadersForGatewayBackwardsCompatibility : IMutateOutgoingTransportMessages
    {
        public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
        {
            var headers = context.OutgoingHeaders;
            headers.Add(Headers.TimeToBeReceived, TimeSpan.MaxValue.ToString());
            headers.Add(Headers.NonDurableMessage, false.ToString());

            return Task.CompletedTask;
        }
    }

    #endregion

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