using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        var endpointConfiguration = new EndpointConfiguration("Voter");

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);

        #region ConfigureSenderSideRouting-Voter

        var remotePartitions = new[] { "John", "Abby" };

        var routing = transport.Routing();
        var distribution = routing.RegisterPartitionedDestinationEndpoint(
            destinationEndpoint: "CandidateVoteCount",
            partitions: remotePartitions);

        distribution.AddPartitionMappingForMessageType<CloseElection>(
            mapMessageToPartitionKey: message =>
            {
                return message.Candidate;
            });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press Enter to start election");
        Console.WriteLine("Press Esc to stop election");
        Console.ReadLine();

        var random = new Random();
        var votedZipCode = Enumerable.Range(1, 10).Select(x => random.Next(0, 99001).ToString("d5")).ToArray();

        do
        {
            while (!Console.KeyAvailable)
            {
                var choice = random.Next(0, 2);
                var candidate = remotePartitions[choice % 2];
                var zipcode = votedZipCode[random.Next(0, 10)];

                Console.WriteLine($"Voted for {candidate} from zip code {zipcode}");

                var votePlaced = new VotePlaced
                {
                    Candidate = candidate,
                    ZipCode = zipcode
                };
                await endpointInstance.Publish(votePlaced)
                .ConfigureAwait(false);

                await Task.Delay(1000)
                    .ConfigureAwait(false);
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        Console.WriteLine("Closing election");

        var closeElection1 = new CloseElection
        {
            Candidate = remotePartitions[0]
        };
        await endpointInstance.Send(closeElection1)
            .ConfigureAwait(false);
        var closeElection2 = new CloseElection
        {
            Candidate = remotePartitions[1]
        };
        await endpointInstance.Send(closeElection2)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit the sample");
        Console.ReadKey(true);

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}