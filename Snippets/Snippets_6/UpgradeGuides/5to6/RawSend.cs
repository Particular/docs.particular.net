// ReSharper disable PossibleNullReferenceException
namespace Snippets6.UpgradeGuides._5to6
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Extensibility;
    using NServiceBus.Routing;
    using NServiceBus.Transports;

    public class RawSend
    {
        public async Task RawSending()
        {
            IDispatchMessages dispatcher = null;

            #region DispatcherRawSending

            var headers = new Dictionary<string, string>();
            var outgoingMessage = new OutgoingMessage("MessageId", headers, new byte[]
            {
            });
            var constraints = new[]
            {
                new NonDurableDelivery()
            };
            UnicastAddressTag address = new UnicastAddressTag("Destination");
            TransportOperation transportOperation = new TransportOperation(outgoingMessage, address, DispatchConsistency.Default, constraints);
            UnicastTransportOperation unicastTransportOperation = new UnicastTransportOperation(outgoingMessage, "destination");
            IEnumerable<MulticastTransportOperation> multicastOperations = Enumerable.Empty<MulticastTransportOperation>();
            UnicastTransportOperation[] unicastOperations = {unicastTransportOperation};
            TransportOperations operations = new TransportOperations(multicastOperations, unicastOperations);
            await dispatcher.Dispatch(operations, new ContextBag());

            #endregion
        }
    }
}