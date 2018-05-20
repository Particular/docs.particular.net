using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Attachments.Sql;
using NServiceBus.Features;

class Program
{
    static async Task Main()
    {
        SqlHelper.EnsureDatabaseExists(SqlHelper.ConnectionString);
        #region EndpointConfiguration
        var endpointConfiguration = new EndpointConfiguration("Endpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.EnableAttachments(SqlHelper.ConnectionString, TimeToKeep.Default);
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();
        endpointConfiguration.DisableFeature<TimeoutManager>();
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(SqlHelper.ConnectionString);
        endpointConfiguration.EnableInstallers();
        #endregion
        Console.Title = "SampleEndpoint Press Ctrl-C to Exit.";
        Console.TreatControlCAsInput = true;
        var endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        Console.ReadKey(true);
        await endpoint.Stop().ConfigureAwait(false);
    }
}