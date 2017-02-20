using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

namespace Voter
{
    using System.Linq;
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

            var transportConfig = endpointConfiguration.ApplyCommonConfiguration();

            ConfigureSenderSizeRoutingForCandidateVoteCount(transportConfig);

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Enter to start election");
            Console.WriteLine("Press Esc to stop election");
            Console.ReadLine();

            var random = new Random();
            var votedZipCode = Enumerable.Range(1, 10).Select(x => random.Next(0, 99001).ToString("d5")).ToArray();

            do
            {
                while (!Console.KeyAvailable)
                {
                    var choice = random.Next(1, 3);
                    var candidate = choice == 1 ? "John" : "Abby";
                    var zipcode = votedZipCode[random.Next(0, 10)];

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

        static void ConfigureSenderSizeRoutingForCandidateVoteCount(TransportExtensions<AzureServiceBusTransport> transportConfig)
        {
            var distributionConfig = transportConfig.Routing()
                .RegisterPartitionedDestinationEndpoint("CandidateVoteCount", new[] {"John", "Abby"});
            distributionConfig.AddPartitionMappingForMessageType<CloseElection>(message => message.Candidate);
        }
    }
}