using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Attachments.Sql;
using NServiceBus.Features;

namespace SampleEndpoint
{
    public class Program
    {
        static async Task Main()
        {
            var endpoint = await StartEndpoint().ConfigureAwait(false);
            Console.Title = "SampleEndpoint Press Ctrl-C to Exit.";
            Console.TreatControlCAsInput = true;
            Console.ReadKey(true);
            await endpoint.Stop().ConfigureAwait(false);
        }

        public static Task<IEndpointInstance> StartEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration("SampleEndpoint");
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();
            endpointConfiguration.DisableFeature<TimeoutManager>();
            #region EndpointConfiguration
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.EnableAttachments(SqlHelper.ConnectionString, TimeToKeep.Default);
            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(SqlHelper.ConnectionString);
            endpointConfiguration.EnableInstallers();

            #endregion

            return Endpoint.Start(endpointConfiguration);
        }
    }
}