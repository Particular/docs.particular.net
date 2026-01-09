namespace Core.Scanning;

using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation;

class ManualRegistration
{
    void RegisterHandlerManually(EndpointConfiguration endpointConfiguration)
    {
        #region RegisterHandlerManually

        endpointConfiguration.AddHandler<PlaceOrderHandler>();

        #endregion
    }

    void RegisterSagaManually(EndpointConfiguration endpointConfiguration)
    {
        #region RegisterSagaManually

        endpointConfiguration.AddSaga<ShippingSaga>();

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
}

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}

public class PlaceOrder
{
}

public class ShippingSaga : Saga<ShippingSagaData>,
    IAmStartedByMessages<ShipOrder>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingSagaData> mapper)
    {
        mapper.MapSaga(s => s.OrderId).ToMessage<ShipOrder>(m => m.OrderId);
    }

    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}

public class ShippingSagaData : ContainSagaData
{
    public string OrderId { get; set; }
}

public class ShipOrder
{
    public string OrderId { get; set; }
}

public class CustomRoutingFeature : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
    }
}

public class DatabaseSetupInstaller : INeedToInstallSomething
{
    public Task Install(string identity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
