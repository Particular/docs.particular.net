using System;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Messages;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using NServiceBus;

namespace CandidateVoteCount
{
    public class EndpointCommunicationListener : ICommunicationListener
    {
        private StatefulServiceContext context;
        private IEndpointInstance endpointInstance;

        public EndpointCommunicationListener(StatefulServiceContext context)
        {
            this.context = context;
        }

        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            NamedPartitionInformation rangePartitionInformation;
            using (var client = new FabricClient())
            {
                var servicePartitionList = await client.QueryManager.GetPartitionListAsync(context.ServiceName, context.PartitionId).ConfigureAwait(false);
                rangePartitionInformation = servicePartitionList.Select(x => x.PartitionInformation).Cast<NamedPartitionInformation>().Single(p => p.Id == context.PartitionId);
            }

            var endpointConfiguration = new EndpointConfiguration("CandidateVoteCount");
            endpointConfiguration.MakeInstanceUniquelyAddressable(rangePartitionInformation.Name);
            
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterComponents(components => components.RegisterSingleton(context));
            var transportConfig = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
            }
            transportConfig.ConnectionString(connectionString);
            transportConfig.UseForwardingTopology();

            transportConfig.Routing().RouteToEndpoint(typeof(TrackZipCode), "ZipCodeVoteCount");
            
            endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            return null;
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            return endpointInstance.Stop();
        }

        public void Abort()
        {
            // Fire & Forget Close
            CloseAsync(CancellationToken.None);
        }
    }
}