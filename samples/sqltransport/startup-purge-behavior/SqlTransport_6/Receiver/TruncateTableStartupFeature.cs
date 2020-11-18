using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;

#region TruncateTableAtStartup

public class TruncateTableStartupFeature : Feature
{
    public TruncateTableStartupFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var transportInfrastructure = context.Settings.Get<TransportInfrastructure>();

        var endpoint = new NServiceBus.Routing.EndpointInstance("Samples.SqlServer.TruncateReceiver");

        var connectionString = @"Data Source=.\SqlExpress;Database=SQLServerTruncate;Integrated Security=True;Max Pool Size=100";

        SqlHelper.TruncateMessageTable(connectionString, transportInfrastructure.ToTransportAddress(LogicalAddress.CreateRemoteAddress(endpoint)));
    }
}

#endregion