namespace Core.Scanning;

using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation;
using System.Threading;
using System.Threading.Tasks;

class ManualRegistration
{
    void RegisterHandlerManually(EndpointConfiguration endpointConfiguration)
    {
        #region RegisterHandlerManually

        endpointConfiguration.AddHandler<OrderHandler>();

        #endregion
    }

    void RegisterSagaManually(EndpointConfiguration endpointConfiguration)
    {
        #region RegisterSagaManually

        endpointConfiguration.AddSaga<OrderSaga>();

        #endregion
    }

    void EnableFeatureManually(EndpointConfiguration endpointConfiguration)
    {
        #region EnableFeatureManually

        endpointConfiguration.EnableFeature<CustomRoutingFeature>();

        #endregion
    }

    void RegisterInstallerManually(EndpointConfiguration endpointConfiguration)
    {
        #region RegisterInstallerManually

        endpointConfiguration.AddInstaller<DatabaseSetupInstaller>();

        #endregion
    }

    void CompleteManualRegistration(EndpointConfiguration endpointConfiguration)
    {
        #region CompleteManualRegistration

        var scanner = endpointConfiguration.AssemblyScanner();
        scanner.Disable = true;

        // Register message handlers
        endpointConfiguration.AddHandler<OrderHandler>();
        endpointConfiguration.AddHandler<PaymentHandler>();

        // Register sagas
        endpointConfiguration.AddSaga<OrderSaga>();

        // Enable required features
        endpointConfiguration.EnableFeature<MyCustomFeature>();

        // Register installers
        endpointConfiguration.AddInstaller<MyInstaller>();

        #endregion
    }

    class MyMessageHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }

    class OrderHandler : IHandleMessages<OrderMessage>
    {
        public Task Handle(OrderMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }

    class PaymentHandler : IHandleMessages<PaymentMessage>
    {
        public Task Handle(PaymentMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }

    class MySaga : Saga<MySagaData>,
        IAmStartedByMessages<StartMessage>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.ConfigureMapping<StartMessage>(m => m.OrderId).ToSaga(s => s.OrderId);
        }

        public Task Handle(StartMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }

    class OrderSaga : Saga<OrderSagaData>,
        IAmStartedByMessages<StartOrder>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<StartOrder>(m => m.OrderId).ToSaga(s => s.OrderId);
        }

        public Task Handle(StartOrder message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }

    class MyFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }

    class MyCustomFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }

    class MyInstaller : INeedToInstallSomething
    {
        public Task Install(string identity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    class DatabaseSetupInstaller : INeedToInstallSomething
    {
        public Task Install(string identity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    class CustomRoutingFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }

    class MyMessage
    {
    }

    class OrderMessage
    {
    }

    class PaymentMessage
    {
    }

    class StartMessage
    {
        public string OrderId { get; set; }
    }

    class StartOrder
    {
        public string OrderId { get; set; }
    }

    class MySagaData : ContainSagaData
    {
        public string OrderId { get; set; }
    }

    class OrderSagaData : ContainSagaData
    {
        public string OrderId { get; set; }
    }
}

