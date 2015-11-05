// ReSharper disable PossibleNullReferenceException
namespace Snippets6.UpgradeGuides._5to6
{
    using System.Collections.Generic;
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
            var outgoingMessage = new OutgoingMessage("MessageId", headers, new byte[] { });
            var constraints = new[] { new NonDurableDelivery() };
            var options = new DispatchOptions(new UnicastAddressTag("Destination"), DispatchConsistency.Default, constraints);
            TransportOperation transportOperation = new TransportOperation(outgoingMessage, options);
            await dispatcher.Dispatch(new[] { transportOperation }, new ContextBag());

            #endregion
        }
    }
}