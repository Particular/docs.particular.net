using NServiceBus;
using NServiceBus.Features;

class Usage
{
    Usage(BusConfiguration busConfiguration, IBus Bus)
    {
        #region GatewayConfiguration

        busConfiguration.EnableFeature<Gateway>();

        #endregion

        #region SendToSites

        Bus.SendToSites(new[] { "SiteA", "SiteB" }, new MyMessage());

        #endregion

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
    }

}