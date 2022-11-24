using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

static class Program
{
    const string Endpoint1QueueName = "samples.asbs.hierarchymigration.endpoint1";
    const string Endpoint1SubscriptionName = "Samples.ASBS.HierarchyMigration.Endpoint1";
    const string Endpoint2QueueName = "samples.asbs.hierarchymigration.endpoint2";
    const string Endpoint2MigrationQueueName = "samples.asbs.hierarchymigration.endpoint2.migration";
    const string Endpoint2SubscriptionName = "Samples.ASBS.HierarchyMigration.Endpoint2";
    const string PublishBundleName = "bundle-to-publish-to";
    const string SubscriptionBundleName = "bundle-to-subscribe-to";

    static async Task Main()
    {
        Console.Title = "Samples.ASBS.HierarchyMigration.Migration";
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine("| Endpoint 2 Migration Sample                                                              |");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        // Endpoint 2 will be gradually migrated
        var adminClient = new ServiceBusAdministrationClient(connectionString);
        Console.WriteLine("Make sure Endpoint1 and Endpoint2 are not running");
        Console.WriteLine("Press any key to cleanup infrastructure from previous runs of this sample.");
        Console.ReadLine();

        await DeleteExistingInfrastructure(adminClient);

        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine($@"Deleted topics:
- {PublishBundleName}
- {SubscriptionBundleName}

Deleted queues:
- {Endpoint1QueueName}
- {Endpoint2QueueName}
- {Endpoint2MigrationQueueName}
");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        Console.WriteLine("Start Endpoint1 and Endpoint2 and wait a bit until some messages are published.");

        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine($@"When the endpoints are running the topology should look like the following:
{PublishBundleName}
    |__{Endpoint1SubscriptionName}
       |__ $default: 1=0
       |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
        --> {Endpoint1QueueName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
       |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        --> {Endpoint2QueueName}

{Endpoint1QueueName}
{Endpoint2QueueName}
");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        Console.WriteLine("Press any key to setup the sample topology");
        Console.ReadLine();

        await CreateEntityGracefully(async () => await adminClient.CreateTopicAsync(
            new CreateTopicOptions(SubscriptionBundleName)
            {
                EnableBatchedOperations = true,
                MaxSizeInMegabytes = 5120
            }));

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine($@"The {SubscriptionBundleName} topic has been setup:
{PublishBundleName}
    |__{Endpoint1SubscriptionName}
       |__ $default: 1=0
       |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
        --> {Endpoint1QueueName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
       |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        --> {Endpoint2QueueName}

{SubscriptionBundleName}
    |__

{Endpoint1QueueName}
{Endpoint2QueueName}
");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        await CreateEntityGracefully(async () =>
            await adminClient.CreateSubscriptionAsync(
                new CreateSubscriptionOptions(PublishBundleName, $"forwardTo-{SubscriptionBundleName}")
                {
                    LockDuration = TimeSpan.FromMinutes(5),
                    ForwardTo = SubscriptionBundleName,
                    EnableDeadLetteringOnFilterEvaluationExceptions = false,
                    MaxDeliveryCount = int.MaxValue,
                    EnableBatchedOperations = true,
                    UserMetadata = SubscriptionBundleName
                },
                new CreateRuleOptions("$default", new TrueRuleFilter())));

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine($@"The forward rule on topic {PublishBundleName} has been setup:
{PublishBundleName}
    |__{Endpoint1SubscriptionName}
       |__ $default: 1=0
       |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
        --> {Endpoint1QueueName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
       |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        --> {Endpoint2QueueName}
    |__forwardTo-{SubscriptionBundleName}
       |__ $default: 1=1
        --> {SubscriptionBundleName}

{SubscriptionBundleName}
    |__

{Endpoint1QueueName}
{Endpoint2QueueName}
");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        await CreateEntityGracefully(async () =>
            await adminClient.CreateQueueAsync(
                new CreateQueueOptions(Endpoint2MigrationQueueName)
                {
                    RequiresDuplicateDetection = true,
                    DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(5),
                }));

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine($@"The migration queue {Endpoint2MigrationQueueName} has been setup:
{PublishBundleName}
    |__{Endpoint1SubscriptionName}
       |__ $default: 1=0
       |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
        --> {Endpoint1QueueName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
       |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        --> {Endpoint2QueueName}
    |__forwardTo-{SubscriptionBundleName}
       |__ $default: 1=1
        --> {SubscriptionBundleName}

{SubscriptionBundleName}
    |__

{Endpoint1QueueName}
{Endpoint2QueueName}
{Endpoint2MigrationQueueName}
");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        SubscriptionProperties publishBundleEndpoint2Subscription = await adminClient.GetSubscriptionAsync(PublishBundleName, Endpoint2SubscriptionName);

        await CreateEntityGracefully(async () =>
            await adminClient.CreateSubscriptionAsync(
                new CreateSubscriptionOptions(publishBundleEndpoint2Subscription)
                {
                    TopicName = SubscriptionBundleName,
                    ForwardTo = Endpoint2MigrationQueueName
                },
                new CreateRuleOptions("$default", new FalseRuleFilter())));

        publishBundleEndpoint2Subscription.ForwardTo = Endpoint2MigrationQueueName;
        await adminClient.UpdateSubscriptionAsync(publishBundleEndpoint2Subscription);

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine($@"The subscription {Endpoint2SubscriptionName} on topic {SubscriptionBundleName} has been setup:
{PublishBundleName}
    |__{Endpoint1SubscriptionName}
       |__ $default: 1=0
       |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
        --> {Endpoint1QueueName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
       |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        --> {Endpoint2QueueName}
    |__forwardTo-{SubscriptionBundleName}
       |__ $default: 1=1
        --> {SubscriptionBundleName}

{SubscriptionBundleName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
        --> {Endpoint2MigrationQueueName}

{Endpoint1QueueName}
{Endpoint2QueueName}
{Endpoint2MigrationQueueName}
");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        await foreach (var rule in adminClient.GetRulesAsync(PublishBundleName, Endpoint2SubscriptionName))
        {
            if (rule.Name == "$default")
            {
                continue;
            }

            await adminClient.CreateRuleAsync(SubscriptionBundleName, Endpoint2SubscriptionName,
                new CreateRuleOptions
                {
                    Action = rule.Action,
                    Filter = rule.Filter,
                    Name = rule.Name
                });
        }

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine($@"The subscription {Endpoint2SubscriptionName} on topic {SubscriptionBundleName} has been setup:
{PublishBundleName}
    |__{Endpoint1SubscriptionName}
       |__ $default: 1=0
       |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
        --> {Endpoint1QueueName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
       |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        --> {Endpoint2QueueName}
    |__forwardTo-{SubscriptionBundleName}
       |__ $default: 1=1
        --> {SubscriptionBundleName}

{SubscriptionBundleName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
       |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        --> {Endpoint2MigrationQueueName}

{Endpoint1QueueName}
{Endpoint2QueueName}
{Endpoint2MigrationQueueName}
");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        Console.WriteLine("Press any key to continue with the migration");
        Console.ReadLine();

        await adminClient.DeleteSubscriptionAsync(PublishBundleName, Endpoint2SubscriptionName);

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine($@"The subscription {Endpoint2SubscriptionName} on topic {PublishBundleName} has been deleted:
{PublishBundleName}
    |__{Endpoint1SubscriptionName}
       |__ $default: 1=0
       |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
        --> {Endpoint1QueueName}
    |__forwardTo-{SubscriptionBundleName}
       |__ $default: 1=1
        --> {SubscriptionBundleName}

{SubscriptionBundleName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
       |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        --> {Endpoint2MigrationQueueName}

{Endpoint1QueueName}
{Endpoint2QueueName}
{Endpoint2MigrationQueueName}
");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        QueueRuntimeProperties migrationQueueRuntimeProperties = await adminClient.GetQueueRuntimePropertiesAsync(Endpoint2MigrationQueueName);
        Console.WriteLine($"There are currently {migrationQueueRuntimeProperties.ActiveMessageCount} messages in the migration queue.");

        QueueProperties migrationQueueProperties = await adminClient.GetQueueAsync(Endpoint2MigrationQueueName);
        migrationQueueProperties.ForwardTo = Endpoint2QueueName;
        await adminClient.UpdateQueueAsync(migrationQueueProperties);

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine($@"The queue {Endpoint2MigrationQueueName} forwarding to {Endpoint2QueueName} has been setup:
{PublishBundleName}
    |__{Endpoint1SubscriptionName}
       |__ $default: 1=0
       |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
        --> {Endpoint1QueueName}
    |__forwardTo-{SubscriptionBundleName}
       |__ $default: 1=1
        --> {SubscriptionBundleName}

{SubscriptionBundleName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
       |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        --> {Endpoint2MigrationQueueName}

{Endpoint1QueueName}
{Endpoint2QueueName}
{Endpoint2MigrationQueueName}
--> {Endpoint2QueueName}
");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        do
        {
            migrationQueueRuntimeProperties = await adminClient.GetQueueRuntimePropertiesAsync(Endpoint2MigrationQueueName);
            if(migrationQueueRuntimeProperties.ActiveMessageCount > 0)
            {
                Console.WriteLine($"There are still {migrationQueueRuntimeProperties.ActiveMessageCount} messages in the migration queue.");
                await Task.Delay(2000);
            }
            else
            {
                Console.WriteLine("Everything forwarded. Moving on");
            }
        } while (migrationQueueRuntimeProperties.ActiveMessageCount > 0);

        SubscriptionProperties subscriptionBundleEndpoint2Subscription = await adminClient.GetSubscriptionAsync(SubscriptionBundleName, Endpoint2SubscriptionName);
        subscriptionBundleEndpoint2Subscription.ForwardTo = Endpoint2SubscriptionName;
        await adminClient.UpdateSubscriptionAsync(subscriptionBundleEndpoint2Subscription);

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine($@"The subscription {Endpoint2SubscriptionName} on topic {SubscriptionBundleName} has been changed to forward to {Endpoint2QueueName}:
{PublishBundleName}
    |__{Endpoint1SubscriptionName}
       |__ $default: 1=0
       |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
        --> {Endpoint1QueueName}
    |__forwardTo-{SubscriptionBundleName}
       |__ $default: 1=1
        --> {SubscriptionBundleName}

{SubscriptionBundleName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
       |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        --> {Endpoint2SubscriptionName}

{Endpoint1QueueName}
{Endpoint2QueueName}
{Endpoint2MigrationQueueName}
--> {Endpoint2QueueName}
");
        Console.WriteLine("--------------------------------------------------------------------------------------------");


        Console.WriteLine("Press any key to continue with the migration");
        Console.ReadLine();

        await adminClient.DeleteQueueAsync(Endpoint2MigrationQueueName);

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine($@"The queue {Endpoint2MigrationQueueName} was removed:
{PublishBundleName}
    |__{Endpoint1SubscriptionName}
       |__ $default: 1=0
       |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
        --> {Endpoint1QueueName}
    |__forwardTo-{SubscriptionBundleName}
       |__ $default: 1=1
        --> {SubscriptionBundleName}

{SubscriptionBundleName}
    |__{Endpoint2SubscriptionName}
       |__ $default: 1=0
       |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        --> {Endpoint2SubscriptionName}

{Endpoint1QueueName}
{Endpoint2QueueName}
");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        Console.WriteLine("Migration done.");

        Console.WriteLine($@"Stop Endpoint 2 and change:
transport.Topology = TopicTopology.Single(""{PublishBundleName}"");

to

transport.Topology = TopicTopology.Hierarchy(""{PublishBundleName}"", ""{SubscriptionBundleName}"");

and then start Endpoint 2.
");
    }

    static async Task DeleteExistingInfrastructure(ServiceBusAdministrationClient adminClient)
    {
        await DeleteEntityGracefully(async () => await adminClient.DeleteTopicAsync(PublishBundleName));
        await DeleteEntityGracefully(async () => await adminClient.DeleteTopicAsync(SubscriptionBundleName));
        await DeleteEntityGracefully(async () => await adminClient.DeleteQueueAsync(Endpoint2MigrationQueueName));
        await DeleteEntityGracefully(async () => await adminClient.DeleteQueueAsync(Endpoint1QueueName));
        await DeleteEntityGracefully(async () => await adminClient.DeleteQueueAsync(Endpoint2QueueName));
    }

    static Task CreateEntityGracefully(Func<Task> func) =>
        HandleFailure(func, ServiceBusFailureReason.MessagingEntityAlreadyExists);

    static Task DeleteEntityGracefully(Func<Task> func) =>
        HandleFailure(func, ServiceBusFailureReason.MessagingEntityNotFound);

    static async Task HandleFailure(Func<Task> func, ServiceBusFailureReason reason)
    {
        try
        {
            await func();
        }
        catch (ServiceBusException e) when (e.Reason == reason)
        {
        }
        catch (ServiceBusException sbe) when (sbe.IsTransient) // An operation is in progress.
        {
        }
    }
}