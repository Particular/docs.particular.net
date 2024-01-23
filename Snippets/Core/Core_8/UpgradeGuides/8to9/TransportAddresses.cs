using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Settings;
using NServiceBus.Transport;

class TransportAddresses
{
#pragma warning disable CS0618 // Type or member is obsolete
    #region core-8to9-addresses-features
    class MyFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            string local = context.Settings.LocalAddress();
            string instance = context.Settings.InstanceSpecificQueue();
        }
    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete
    #region core-8to9-address-translation
    public class MyHandler : IHandleMessages<MyMessage>
    {
        readonly IReadOnlySettings settings;

        public MyHandler(IReadOnlySettings settings)
        {
            this.settings = settings;
        }

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var destination = settings.Get<TransportDefinition>().ToTransportAddress(new QueueAddress("Sales"));
            var sendOptions = new SendOptions();
            sendOptions.SetDestination(destination);
            return context.Send(new SomeMessage(), sendOptions);
        }
    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    public class MyMessage
    {
    }
    public class SomeMessage
    {
    }
}