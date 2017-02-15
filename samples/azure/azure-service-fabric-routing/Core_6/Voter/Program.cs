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

            #region Configure Sender-Side routing for CandidateVoteCount

            transportConfig.Routing().RouteToEndpoint(typeof(CloseElection), "CandidateVoteCount");

            var internalSettings = endpointConfiguration.GetSettings();

            var policy = internalSettings.GetOrCreate<DistributionPolicy>();

            Func<object, string> mapper = message =>
            {
                var candidate = message as CloseElection;
                if (candidate != null)
                {
                    return candidate.Candidate;
                }

                throw new Exception($"No partition mapping is found for message type '{message.GetType()}'.");
            };

            policy.SetDistributionStrategy(new PartitionAwareDistributionStrategy("CandidateVoteCount", mapper, DistributionStrategyScope.Send));

            var candidateVoteCountInstances = new List<EndpointInstance>
            {
                new EndpointInstance("CandidateVoteCount", "John"),
                new EndpointInstance("CandidateVoteCount", "Abby"),
            };

            var instances = internalSettings.GetOrCreate<EndpointInstances>();
            instances.AddOrReplaceInstances("CandidateVoteCount", candidateVoteCountInstances);
            #endregion

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Enter to start election");
            Console.WriteLine("Press Esc to stop election");
            Console.ReadLine();
            do
            {
                while (!Console.KeyAvailable)
                {
                    var choice = new Random().Next(1, 3);
                    var zipcode = new Random().Next(0, 99001).ToString("d5");
                    var candidate = choice == 1 ? "John" : "Abby";

                    Console.WriteLine($"Voted for {candidate} from zip code {zipcode}");

                await endpointInstance.Publish(new VotePlaced
                {
                    Candidate = candidate,
                    ZipCode = zipcode
                });

                    await Task.Delay(1000);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Console.WriteLine("Closing election");

            await endpointInstance.Send(new CloseElection
            {
                Candidate = "John",
            });
            await endpointInstance.Send(new CloseElection
            {
                Candidate = "Abby",
            });

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
