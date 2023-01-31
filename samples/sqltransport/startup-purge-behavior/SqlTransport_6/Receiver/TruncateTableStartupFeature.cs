using System.Threading.Tasks;
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
        context.RegisterStartupTask(new Truncate(context.Settings.Get<TransportInfrastructure>()));
    }

    class Truncate : FeatureStartupTask
    {
        readonly TransportInfrastructure transportInfrastructure;

        public Truncate(TransportInfrastructure transportInfrastructure)
        {
            this.transportInfrastructure = transportInfrastructure;
        }

        protected override async Task OnStart(IMessageSession session)
        {
            var endpoint = new NServiceBus.Routing.EndpointInstance("Samples.SqlServer.TruncateReceiver");

            // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SQLServerTruncate;Integrated Security=True;Max Pool Size=100;Encrypt=false
            var connectionString = @"Server=localhost,1433;Initial Catalog=SQLServerTruncate;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

            await SqlHelper.TruncateMessageTable(connectionString, transportInfrastructure.ToTransportAddress(LogicalAddress.CreateRemoteAddress(endpoint)));
        }

        protected override Task OnStop(IMessageSession session)
        {
            return Task.CompletedTask;
        }
    }
}

#endregion