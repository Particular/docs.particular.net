using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;

class TransportAddresses
{
    #region core-8to9-addresses-features
    class MyFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            QueueAddress local = context.LocalQueueAddress();
            QueueAddress instance = context.InstanceSpecificQueueAddress();
        }
    }
    #endregion

    class ReceiveAddressesFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            throw new NotImplementedException();
        }

        #region core-8to9-receive-addresses
        class StartupTask(ReceiveAddresses receiveAddresses) : FeatureStartupTask
        {
            protected override Task OnStart(IMessageSession session, CancellationToken cancellationToken = default)
            {
                // equivalent to settings.LocalAddress()
                Console.WriteLine($"Starting endpoint, listening on {receiveAddresses.MainReceiveAddress}");

                if (receiveAddresses.InstanceReceiveAddress != null)
                {
                    // equivalent to settings.InstanceSpecificQueue())
                    Console.WriteLine($"Starting endpoint, listening on {receiveAddresses.InstanceReceiveAddress}");
                }
                return Task.CompletedTask;
            }

            protected override Task OnStop(IMessageSession session, CancellationToken cancellationToken = default) => Task.CompletedTask;
        }
        #endregion
    }

    #region core-8to9-address-translation
    public class MyHandler(ITransportAddressResolver addressResolver) : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var destination = addressResolver.ToTransportAddress(new QueueAddress("Sales"));
            var sendOptions = new SendOptions();
            sendOptions.SetDestination(destination);
            return context.Send(new SomeMessage(), sendOptions);
        }
    }
    #endregion

    public class MyMessage
    {
    }

    public class SomeMessage
    {
    }
}