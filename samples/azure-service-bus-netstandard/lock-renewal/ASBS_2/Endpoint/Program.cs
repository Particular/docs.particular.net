using System;
using System.Threading.Tasks;
using Endpoint;
using Microsoft.Azure.ServiceBus.Management;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ASB.LockRenewal";

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.SendReply.LockRenewal");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var transport = new AzureServiceBusTransport(connectionString);
        endpointConfiguration.UseTransport(transport);

        #region override-lock-renewal-configuration

        endpointConfiguration.LockRenewal(options =>
        {
            options.LockDuration = TimeSpan.FromSeconds(30);
            options.ExecuteRenewalBefore = TimeSpan.FromSeconds(5);
        });

        #endregion

        var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

        await OverrideQueueLockDuration("Samples.ASB.SendReply.LockRenewal", connectionString).ConfigureAwait(false);

        await endpointInstance.SendLocal(new LongProcessingMessage { ProcessingDuration = TimeSpan.FromSeconds(45)});

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop().ConfigureAwait(false);
    }

    private static async Task OverrideQueueLockDuration(string queuePath, string connectionString)
    {
        var managementClient = new ManagementClient(connectionString);
        var queueDescription = new QueueDescription(queuePath)
        {
            LockDuration = TimeSpan.FromSeconds(30)
        };

        await managementClient.UpdateQueueAsync(queueDescription).ConfigureAwait(false);
    }
}