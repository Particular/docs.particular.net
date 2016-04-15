// ReSharper disable PossibleNullReferenceException
namespace Snippets6.UpgradeGuides._5to6
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Extensibility;
    using NServiceBus.Routing;
    using NServiceBus.Transports;

    class RawSend
    {
        async Task RawSending(IDispatchMessages dispatcher)
        {
            #region DispatcherRawSending

            Dictionary<string, string> headers = new Dictionary<string, string>();
            OutgoingMessage outgoingMessage = new OutgoingMessage("MessageId", headers, new byte[]
            {
            });
            NonDurableDelivery[] constraints =
            {
                new NonDurableDelivery()
            };
            UnicastAddressTag address = new UnicastAddressTag("Destination");
            TransportOperation transportOperation = new TransportOperation(outgoingMessage, address, DispatchConsistency.Default, constraints);
            TransportOperations operations = new TransportOperations(transportOperation);
            await dispatcher.Dispatch(operations, new ContextBag());

            #endregion
        }
    }
}