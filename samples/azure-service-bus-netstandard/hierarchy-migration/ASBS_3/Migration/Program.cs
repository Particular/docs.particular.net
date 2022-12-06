using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Migration;
using Spectre.Console;

static class Program
{


    static async Task Main()
    {
        Console.Title = "Samples.ASBS.HierarchyMigration.Migration";

        AnsiConsole.Write(
            new FigletText("Endpoint 2 Migration")
                .Centered()
                .Color(Color.Green));

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        // Endpoint 2 will be gradually migrated
        var adminClient = new ServiceBusAdministrationClient(connectionString);

        var endpointsShouldBeStopped = new Rule($"Endpoints should be stopped")
        {
            Alignment = Justify.Center,
            Border = BoxBorder.Ascii
        };
        AnsiConsole.Write(endpointsShouldBeStopped);
        AnsiConsole.MarkupLine(":warning: Make sure Endpoint1 and Endpoint2 are not running");
        AnsiConsole.WriteLine();

        var infrastructureCleanup = new Rule($"Infrastructure cleanup")
        {
            Alignment = Justify.Center,
            Border = BoxBorder.Ascii
        };
        AnsiConsole.Write(infrastructureCleanup);
        if (AnsiConsole.Confirm("Cleanup infrastructure from previous runs of this sample?"))
        {
            await DeleteExistingInfrastructure(adminClient);

            Visualize.DeletedInfrastructure();
        }
        AnsiConsole.WriteLine();

        var endpointsShouldBeRunning = new Rule($"Endpoints should be running")
        {
            Alignment = Justify.Center,
            Border = BoxBorder.Ascii
        };
        AnsiConsole.Write(endpointsShouldBeRunning);
        AnsiConsole.MarkupLine(":warning: Start Endpoint1 and Endpoint2 and wait a bit until some messages are published.");
        AnsiConsole.WriteLine();

        Visualize.TopologyBeforeMigration();

        AnsiConsole.WriteLine(":: Press any key to setup the sample topology");
        Console.ReadLine();

        await CreateEntityGracefully(async () => await adminClient.CreateTopicAsync(
            new CreateTopicOptions(Hierarchy.SubscriptionBundleName)
            {
                EnableBatchedOperations = true,
                MaxSizeInMegabytes = 5120
            }));

        Visualize.SubscriptionBundleSetup();

        await CreateEntityGracefully(async () =>
            await adminClient.CreateSubscriptionAsync(
                new CreateSubscriptionOptions(Hierarchy.PublishBundleName, $"forwardTo-{Hierarchy.SubscriptionBundleName}")
                {
                    LockDuration = TimeSpan.FromMinutes(5),
                    ForwardTo = Hierarchy.SubscriptionBundleName,
                    EnableDeadLetteringOnFilterEvaluationExceptions = false,
                    MaxDeliveryCount = int.MaxValue,
                    EnableBatchedOperations = true,
                    UserMetadata = Hierarchy.SubscriptionBundleName
                },
                new CreateRuleOptions("$default", new TrueRuleFilter())));

        Visualize.ForwardRuleSetup();

        await CreateEntityGracefully(async () =>
            await adminClient.CreateQueueAsync(
                new CreateQueueOptions(Hierarchy.Endpoint2MigrationQueueName)
                {
                    RequiresDuplicateDetection = true,
                    DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(5),
                }));

        Visualize.MigrationQueueSetup();

        SubscriptionProperties publishBundleEndpoint2Subscription = await adminClient.GetSubscriptionAsync(Hierarchy.PublishBundleName, Hierarchy.Endpoint2SubscriptionName);

        await CreateEntityGracefully(async () =>
            await adminClient.CreateSubscriptionAsync(
                new CreateSubscriptionOptions(publishBundleEndpoint2Subscription)
                {
                    TopicName = Hierarchy.SubscriptionBundleName,
                    ForwardTo = Hierarchy.Endpoint2MigrationQueueName
                },
                new CreateRuleOptions("$default", new FalseRuleFilter())));

        publishBundleEndpoint2Subscription.ForwardTo = Hierarchy.Endpoint2MigrationQueueName;
        await adminClient.UpdateSubscriptionAsync(publishBundleEndpoint2Subscription);

        Visualize.Endpoint2SubscriptionUnderSubscriptionBundleSetup();

        await foreach (var rule in adminClient.GetRulesAsync(Hierarchy.PublishBundleName, Hierarchy.Endpoint2SubscriptionName))
        {
            if (rule.Name == "$default")
            {
                continue;
            }

            await adminClient.CreateRuleAsync(Hierarchy.SubscriptionBundleName, Hierarchy.Endpoint2SubscriptionName,
                new CreateRuleOptions
                {
                    Action = rule.Action,
                    Filter = rule.Filter,
                    Name = rule.Name
                });
        }

        Visualize.Endpoint2SubscriptionUnderSubscriptionBundleWithRulesSetup();

        AnsiConsole.WriteLine("Press any key to continue with the migration");
        Console.ReadLine();

        await adminClient.DeleteSubscriptionAsync(Hierarchy.PublishBundleName, Hierarchy.Endpoint2SubscriptionName);

        Visualize.Endpoint2SubscriptionOnPublishBundleDeleted();

        QueueRuntimeProperties migrationQueueRuntimeProperties = await adminClient.GetQueueRuntimePropertiesAsync(Hierarchy.Endpoint2MigrationQueueName);

        var messagesInTheQueue = new Rule("Checking migration queue status")
        {
            Alignment = Justify.Center,
            Border = BoxBorder.Ascii
        };
        AnsiConsole.Write(messagesInTheQueue);
        AnsiConsole.MarkupLineInterpolated($":warning: There are currently {migrationQueueRuntimeProperties.ActiveMessageCount} messages in the migration queue.");
        AnsiConsole.WriteLine();

        QueueProperties migrationQueueProperties = await adminClient.GetQueueAsync(Hierarchy.Endpoint2MigrationQueueName);
        migrationQueueProperties.ForwardTo = Hierarchy.Endpoint2QueueName;
        await adminClient.UpdateQueueAsync(migrationQueueProperties);

        Visualize.Endpoint2MigrationQueueToEndpoint2QueueForwardingSetup();

        var messagesInTheQueueForwarding = new Rule("Checking forwarding progress in the migration queue")
        {
            Alignment = Justify.Center,
            Border = BoxBorder.Ascii
        };
        AnsiConsole.Write(messagesInTheQueueForwarding);

        do
        {
            migrationQueueRuntimeProperties = await adminClient.GetQueueRuntimePropertiesAsync(Hierarchy.Endpoint2MigrationQueueName);
            if(migrationQueueRuntimeProperties.ActiveMessageCount > 0)
            {
                AnsiConsole.MarkupLineInterpolated($":hourglass_not_done: There are still {migrationQueueRuntimeProperties.ActiveMessageCount} messages in the migration queue.");
                await Task.Delay(500);
            }
            else
            {
                AnsiConsole.MarkupLine(":hourglass_done: Everything forwarded. Moving on");
            }
        } while (migrationQueueRuntimeProperties.ActiveMessageCount > 0);

        SubscriptionProperties subscriptionBundleEndpoint2Subscription = await adminClient.GetSubscriptionAsync(Hierarchy.SubscriptionBundleName, Hierarchy.Endpoint2SubscriptionName);
        subscriptionBundleEndpoint2Subscription.ForwardTo = Hierarchy.Endpoint2SubscriptionName;
        await adminClient.UpdateSubscriptionAsync(subscriptionBundleEndpoint2Subscription);

        Visualize.Endpoint2SubscriptionOnSubscriptionBundleFowardingChanged();

        AnsiConsole.WriteLine("Press any key to continue with the migration");
        Console.ReadLine();

        await adminClient.DeleteQueueAsync(Hierarchy.Endpoint2MigrationQueueName);

        Visualize.Endpoint2MigrationQueueDeleted();

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine(":sports_medal: Migration done.");

        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine($@"Stop Endpoint 2 and change:
transport.Topology = TopicTopology.Single(""{Hierarchy.PublishBundleName}"");

to

transport.Topology = TopicTopology.Hierarchy(""{Hierarchy.PublishBundleName}"", ""{Hierarchy.SubscriptionBundleName}"");

and then start Endpoint 2.
");
    }

    static async Task DeleteExistingInfrastructure(ServiceBusAdministrationClient adminClient)
    {
        await DeleteEntityGracefully(async () => await adminClient.DeleteTopicAsync(Hierarchy.PublishBundleName));
        await DeleteEntityGracefully(async () => await adminClient.DeleteTopicAsync(Hierarchy.SubscriptionBundleName));
        await DeleteEntityGracefully(async () => await adminClient.DeleteQueueAsync(Hierarchy.Endpoint2MigrationQueueName));
        await DeleteEntityGracefully(async () => await adminClient.DeleteQueueAsync(Hierarchy.Endpoint1QueueName));
        await DeleteEntityGracefully(async () => await adminClient.DeleteQueueAsync(Hierarchy.Endpoint2QueueName));
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