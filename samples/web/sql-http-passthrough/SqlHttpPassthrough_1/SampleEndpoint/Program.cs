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
            await StopEndpoint(endpoint).ConfigureAwait(false);
        }

        public static Task StopEndpoint(IEndpointInstance endpoint)
        {
            return endpoint.Stop();
        }

        public static Task<IEndpointInstance> StartEndpoint()
        {
            #region EndpointConfiguration

            var endpointConfiguration = new EndpointConfiguration("SampleEndpoint");
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.EnableAttachments(SqlHelper.ConnectionString, TimeToKeep.Default);
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();
            endpointConfiguration.DisableFeature<TimeoutManager>();
            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(SqlHelper.ConnectionString);
            endpointConfiguration.EnableInstallers();

            #endregion

            return Endpoint.Start(endpointConfiguration);
        }
    }
}