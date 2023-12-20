using NServiceBus;
using NServiceBus.Features;
using System.Threading.Tasks;
using System.Threading;
using System;
using NServiceBus.Transport;

class TransportAddresses
{
    class MyFeature : Feature
    {
        #region core-8to9-adresses-features
        protected override void Setup(FeatureConfigurationContext context)
        {
            Console.WriteLine(context.LocalQueueAddress());
            Console.WriteLine(context.InstanceSpecificQueueAddress());
        }
        #endregion

        #region core-8to9-adresses-runtime
        class StartupTask : FeatureStartupTask
        {
            readonly ReceiveAddresses receiveAddresses;

            public StartupTask(ReceiveAddresses receiveAddresses)
            {
                this.receiveAddresses = receiveAddresses;
            }

            protected override Task OnStart(IMessageSession session, CancellationToken cancellationToken = default)
            {
                Console.WriteLine($"Starting endpoint, listening on {receiveAddresses.MainReceiveAddress}");

                if (receiveAddresses.InstanceReceiveAddress != null)
                {
                    Console.WriteLine($"Starting endpoint, listening on {receiveAddresses.InstanceReceiveAddress}");
                }
                return Task.CompletedTask;
            }

            protected override Task OnStop(IMessageSession session, CancellationToken cancellationToken = default) => Task.CompletedTask;
        }
        #endregion
    }

    #region core-8to9-adresses-translation
    public class MyHandler : IHandleMessages<MyMessage>
    {
        readonly ITransportAddressResolver addressResolver;

        public MyHandler(ITransportAddressResolver addressResolver)
        {
            this.addressResolver = addressResolver;
        }

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