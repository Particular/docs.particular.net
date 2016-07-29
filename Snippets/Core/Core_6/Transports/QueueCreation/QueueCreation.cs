namespace Core6.Transports.QueueCreation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Settings;
    using NServiceBus.Transport;

    #region RegisteringTheQueueCreator

    class MyTransport :
        TransportDefinition
    {

        protected override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            return new MyTransportInfrastructure();
        }

        class MyTransportInfrastructure :
            TransportInfrastructure
        {
            public override TransportReceiveInfrastructure ConfigureReceiveInfrastructure()
            {
                return new TransportReceiveInfrastructure(
                    () => new YourMessagePump(),
                    () => new YourQueueCreator(),
                    () => Task.FromResult(StartupCheckResult.Success));
            }

            #endregion

            public override TransportSendInfrastructure ConfigureSendInfrastructure()
            {
                throw new NotImplementedException();
            }

            public override TransportSubscriptionInfrastructure ConfigureSubscriptionInfrastructure()
            {
                throw new NotImplementedException();
            }

            public override EndpointInstance BindToLocalEndpoint(EndpointInstance instance)
            {
                throw new NotImplementedException();
            }

            public override string ToTransportAddress(LogicalAddress logicalAddress)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<Type> DeliveryConstraints { get; }
            public override TransportTransactionMode TransactionMode { get; }
            public override OutboundRoutingPolicy OutboundRoutingPolicy { get; }
        }


        public override string ExampleConnectionStringForErrorMessage { get; }
    }
}