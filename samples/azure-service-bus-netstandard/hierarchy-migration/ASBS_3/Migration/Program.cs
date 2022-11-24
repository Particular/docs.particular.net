using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

static class Program
{
    const string Endpoint1QueueName = "samples.asbs.hierarchymigration.endpoint1";
    const string Endpoint2QueueName = "samples.asbs.hierarchymigration.endpoint2";
    const string Endpoint2MigrationQueueName = "samples.asbs.hierarchymigration.endpoint2.migration";
    const string Endpoint2SubscriptionName = "Samples.ASBS.HierarchyMigration.Endpoint2";
    const string PublishBundleName = "bundle-to-publish-to";
    const string SubscriptionBundleName = "bundle-to-subscribe-to";

    static async Task Main()
    {
        Console.Title = "Samples.ASBS.HierarchyMigration.Migration";
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        // Endpoint 2 will be gradually migrated
        var adminClient = new ServiceBusAdministrationClient(connectionString);
        Console.WriteLine("Make sure Endpoint1 and Endpoint2 are not running");
        Console.WriteLine("Press any key to continue with the migration");
        Console.ReadLine();
        await DeleteExistingInfrastructure(adminClient);

        Console.WriteLine("Start Endpoint1 and Endpoint2 and wait a bit until some messages are published.");
        Console.WriteLine("Press any key to continue with the migration");
        Console.ReadLine();

        // Makes sure the new subscription bundle is there
        try
        {
            await adminClient.CreateTopicAsync(new CreateTopicOptions(SubscriptionBundleName)
            {
                EnableBatchedOperations = true,
                MaxSizeInMegabytes = 5120
            });
        }
        catch (ServiceBusException e) when (e.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
        {
        }

        var subscription = new CreateSubscriptionOptions(PublishBundleName, $"forwardTo-{SubscriptionBundleName}")
        {
            LockDuration = TimeSpan.FromMinutes(5),
            ForwardTo = SubscriptionBundleName,
            EnableDeadLetteringOnFilterEvaluationExceptions = false,
            MaxDeliveryCount = int.MaxValue,
            EnableBatchedOperations = true,
            UserMetadata = SubscriptionBundleName
        };

        try
        {
            await adminClient.CreateSubscriptionAsync(subscription,
                new CreateRuleOptions("$default", new TrueRuleFilter()));
        }
        catch (ServiceBusException sbe) when (sbe.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
        {
        }
        catch (ServiceBusException sbe) when (sbe.IsTransient)// An operation is in progress.
        {
        }

        try
        {
            var createQueueOptions = new CreateQueueOptions(Endpoint2MigrationQueueName)
            {
                RequiresDuplicateDetection = true,
                DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(5),
            };
            await adminClient.CreateQueueAsync(createQueueOptions);
        }
        catch (ServiceBusException sbe) when (sbe.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
        {
        }
        catch (ServiceBusException sbe) when (sbe.IsTransient)// An operation is in progress.
        {
        }

        SubscriptionProperties publishBundleEndpoint2Subscription = await adminClient.GetSubscriptionAsync(PublishBundleName, Endpoint2SubscriptionName);

        try
        {
            var subscriptionBundleEndpoint2SubscriptionCreationOptions = new CreateSubscriptionOptions(publishBundleEndpoint2Subscription)
            {
                TopicName = SubscriptionBundleName,
                ForwardTo = Endpoint2MigrationQueueName
            };
            await adminClient.CreateSubscriptionAsync(subscriptionBundleEndpoint2SubscriptionCreationOptions,
                new CreateRuleOptions("$default", new FalseRuleFilter()));
        }
        catch (ServiceBusException e) when (e.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
        {
        }

        publishBundleEndpoint2Subscription.ForwardTo = Endpoint2MigrationQueueName;
        await adminClient.UpdateSubscriptionAsync(publishBundleEndpoint2Subscription);

        Console.WriteLine("Original subscription forwarding to the migration queue setup");
        Console.WriteLine("Press any key to continue with the migration");
        Console.ReadLine();

        await foreach (var rule in adminClient.GetRulesAsync(PublishBundleName, Endpoint2SubscriptionName))
        {
            if (rule.Name == "$default")
            {
                continue;
            }
            var createRuleOptions = new CreateRuleOptions
            {
                Action = rule.Action,
                Filter = rule.Filter,
                Name = rule.Name
            };
            await adminClient.CreateRuleAsync(SubscriptionBundleName, Endpoint2SubscriptionName,
                createRuleOptions);
        }

        Console.WriteLine("New subscription setup with all the necessary rules of the original one");
        Console.WriteLine("Press any key to continue with the migration");
        Console.ReadLine();

        await adminClient.DeleteSubscriptionAsync(PublishBundleName, Endpoint2SubscriptionName);

        QueueRuntimeProperties migrationQueueRuntimeProperties = await adminClient.GetQueueRuntimePropertiesAsync(Endpoint2MigrationQueueName);
        Console.WriteLine(migrationQueueRuntimeProperties.ActiveMessageCount);

        QueueProperties migrationQueueProperties = await adminClient.GetQueueAsync(Endpoint2MigrationQueueName);
        migrationQueueProperties.ForwardTo = Endpoint2QueueName;
        await adminClient.UpdateQueueAsync(migrationQueueProperties);

        do
        {
            migrationQueueRuntimeProperties = await adminClient.GetQueueRuntimePropertiesAsync(Endpoint2MigrationQueueName);
            if(migrationQueueRuntimeProperties.ActiveMessageCount > 0)
            {
                Console.WriteLine(migrationQueueRuntimeProperties.ActiveMessageCount);
                await Task.Delay(2000);
            }
            else
            {
                Console.WriteLine("Everything forwarded");
            }
        } while (migrationQueueRuntimeProperties.ActiveMessageCount > 0);

        SubscriptionProperties subscriptionBundleEndpoint2Subscription = await adminClient.GetSubscriptionAsync(SubscriptionBundleName, Endpoint2SubscriptionName);
        subscriptionBundleEndpoint2Subscription.ForwardTo = Endpoint2SubscriptionName;
        await adminClient.UpdateSubscriptionAsync(subscriptionBundleEndpoint2Subscription);

        Console.WriteLine("New subscription changed to forward to the endpoint queue");
        Console.WriteLine("Press any key to continue with the migration");
        Console.ReadLine();

        await adminClient.DeleteQueueAsync(Endpoint2MigrationQueueName);
        Console.WriteLine("Migration queue deleted.");

        Console.WriteLine("Migration done.");
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