using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
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
        context.RegisterStartupTask(new Truncate());
    }

    class Truncate : FeatureStartupTask
    {
        protected override async Task OnStart(IMessageSession session, CancellationToken cancellationToken = new CancellationToken())
        {
            // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SQLServerTruncate;Integrated Security=True;Max Pool Size=100;Encrypt=false
            var connectionString = @"Server=localhost,1433;Initial Catalog=SQLServerTruncate;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
            await SqlHelper.TruncateMessageTable(connectionString, "Samples.SqlServer.TruncateReceiver");
        }

        protected override Task OnStop(IMessageSession session, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }
    }
}

#endregion