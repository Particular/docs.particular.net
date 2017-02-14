using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;

namespace Voter
{
    using Contracts;

    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            var endpointConfiguration = new EndpointConfiguration("Voter");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.Recoverability().DisableLegacyRetriesSatellite();
            var transportConfig = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
            }
            transportConfig.ConnectionString(connectionString);
            transportConfig.UseForwardingTopology();

            transportConfig.Routing().RouteToEndpoint(typeof(PlaceVote), "CandidateVoteCount");

            var internalSettings = endpointConfiguration.GetSettings();

            var policy = internalSettings.GetOrCreate<DistributionPolicy>();

            policy.SetDistributionStrategy(new CandidatePartitionDistributionStrategy("CandidateVoteCount", DistributionStrategyScope.Send));
            policy.SetDistributionStrategy(new CandidatePartitionDistributionStrategy("CandidateVoteCount", DistributionStrategyScope.Publish));

            var candidateVoteCountInstances = new List<EndpointInstance>
            {
                new EndpointInstance("CandidateVoteCount", "John"),
                new EndpointInstance("CandidateVoteCount", "Abby"),
            };

            var instances = internalSettings.GetOrCreate<EndpointInstances>();
            instances.AddOrReplaceInstances("CandidateVoteCount", candidateVoteCountInstances);

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press 'x' to exit or 'v' to vote");
            var x = Console.ReadLine();

            while (x != "x")
            {
                Console.WriteLine("Press '1' to vote for John or '2' to vote for Abby");
                var choice = Console.ReadLine();

                if (choice == "1" || choice == "2")
                {
                    Console.WriteLine("Please enter your ZipCode");
                    var zipcode = Console.ReadLine();

                    await endpointInstance.Send(new PlaceVote()
                    {
                        Candidate = choice == "1" ? "John" : "Abby",
                        ZipCode = zipcode
                    });
                }

                Console.WriteLine("Press 'x' to exit or 'v' to vote");
                x = Console.ReadLine();
            }

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}