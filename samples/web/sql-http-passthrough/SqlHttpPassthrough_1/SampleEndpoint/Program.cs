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
            #if NETCOREAPP2_0
            // Since Transaction scope mode is not supported in .NET Core 2.0
            // https://docs.particular.net/transports/sql/transactions#transaction-scope-distributed-transaction
            transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            #endif
            endpointConfiguration.EnableInstallers();

            #endregion

            return Endpoint.Start(endpointConfiguration);
        }
    }
}