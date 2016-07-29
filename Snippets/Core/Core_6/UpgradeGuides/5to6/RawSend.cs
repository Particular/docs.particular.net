// ReSharper disable PossibleNullReferenceException
namespace Core6.UpgradeGuides._5to6
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Extensibility;
    using NServiceBus.Routing;
    using NServiceBus.Transport;

    class RawSend
    {
        async Task RawSending(IDispatchMessages dispatcher)
        {
            #region DispatcherRawSending

            var headers = new Dictionary<string, string>();
            var outgoingMessage = new OutgoingMessage("MessageId", headers, new byte[]
            {
            });
            NonDurableDelivery[] constraints =
            {
                new NonDurableDelivery()
            };
            var address = new UnicastAddressTag("Destination");
            var operation = new TransportOperation(
                message: outgoingMessage,
                addressTag: address,
                requiredDispatchConsistency: DispatchConsistency.Default,
                deliveryConstraints: constraints);
            var operations = new TransportOperations(operation);
            await dispatcher.Dispatch(operations, new ContextBag())
                .ConfigureAwait(false);

            #endregion
        }
    }
}