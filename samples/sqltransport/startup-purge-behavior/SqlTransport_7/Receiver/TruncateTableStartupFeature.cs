using NServiceBus.Features;

#region TruncateTableAtStartup

public class TruncateTableStartupFeature : Feature
{
    public TruncateTableStartupFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var connectionString = @"Data Source=.\sqlsexpress;Database=SQLServerTruncate;Integrated Security=True;Max Pool Size=100";
        SqlHelper.TruncateMessageTable(connectionString, "Samples.SqlServer.TruncateReceiver");
    }
}

#endregion