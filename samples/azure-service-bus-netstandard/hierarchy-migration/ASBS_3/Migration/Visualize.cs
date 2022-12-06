using Spectre.Console;

internal static class Visualize
{
    public static void DeletedInfrastructure()
    {
        var sectionDescription = new Rule("Deleted infrastructure")
        {
            Alignment = Justify.Center,
            Border = BoxBorder.Ascii
        };
        AnsiConsole.Write(sectionDescription);

        var deletedTopics = new Table();
        deletedTopics.AddColumn(new TableColumn("Deleted Topics").Centered());
        deletedTopics.AddRow(Hierarchy.PublishBundleName);
        deletedTopics.AddRow(Hierarchy.SubscriptionBundleName);
        AnsiConsole.Write(deletedTopics);

        var deletedQueues = new Table();
        deletedQueues.AddColumn(new TableColumn("Deleted Queues").Centered());
        deletedQueues.AddRow(Hierarchy.Endpoint1QueueName);
        deletedQueues.AddRow(Hierarchy.Endpoint2QueueName);
        deletedQueues.AddRow(Hierarchy.Endpoint2MigrationQueueName);
        AnsiConsole.Write(deletedQueues);
    }

    /**
      bundle-to-publish-to
        |__Samples.ASBS.HierarchyMigration.Endpoint1
            |__ $default: 1=0
            |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
            --> samples.asbs.hierarchymigration.endpoint1
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
            --> samples.asbs.hierarchymigration.endpoint2

       samples.asbs.hierarchymigration.endpoint1
       samples.asbs.hierarchymigration.endpoint2
     */
    public static void TopologyBeforeMigration()
    {
        var sectionDescription = new Rule("Topology before migration")
        {
            Alignment = Justify.Center,
            Border = BoxBorder.Ascii
        };
        AnsiConsole.Write(sectionDescription);

        var topics = new Tree("Topics");
        var publishBundle = topics.AddNode(Hierarchy.PublishBundleName);
        var endpoint1Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint1Subscription.AddNode("$default: 1=0");
        endpoint1Subscription.AddNode("Event2: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event2%'");
        endpoint1Subscription.AddNode(Hierarchy.Endpoint1QueueName);
        var endpoint2Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint2Subscription.AddNode("$default: 1=0");
        endpoint2Subscription.AddNode("Event1: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event1%'");
        endpoint2Subscription.AddNode(Hierarchy.Endpoint2QueueName);
        AnsiConsole.Write(topics);

        var queues = new Tree("Queues");
        queues.AddNode(Hierarchy.Endpoint1QueueName);
        queues.AddNode(Hierarchy.Endpoint2QueueName);
        AnsiConsole.Write(queues);
    }

    /**
      bundle-to-publish-to
        |__Samples.ASBS.HierarchyMigration.Endpoint1
            |__ $default: 1=0
            |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
            --> samples.asbs.hierarchymigration.endpoint1
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
            --> samples.asbs.hierarchymigration.endpoint2

      bundle-to-subscribe-to
        |__

       samples.asbs.hierarchymigration.endpoint1
       samples.asbs.hierarchymigration.endpoint2
     */
    public static void SubscriptionBundleSetup()
    {
        var sectionDescription = new Rule($"The {Hierarchy.SubscriptionBundleName} topic has been setup")
        {
            Alignment = Justify.Center,
            Border = BoxBorder.Ascii
        };
        AnsiConsole.Write(sectionDescription);

        var topics = new Tree("Topics");
        var publishBundle = topics.AddNode(Hierarchy.PublishBundleName);
        var endpoint1Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint1Subscription.AddNode("$default: 1=0");
        endpoint1Subscription.AddNode("Event2: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event2%'");
        endpoint1Subscription.AddNode(Hierarchy.Endpoint1QueueName);
        var endpoint2Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint2Subscription.AddNode("$default: 1=0");
        endpoint2Subscription.AddNode("Event1: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event1%'");
        endpoint2Subscription.AddNode(Hierarchy.Endpoint2QueueName);
        topics.AddNode(Hierarchy.SubscriptionBundleName);
        AnsiConsole.Write(topics);

        var queues = new Tree("Queues");
        queues.AddNode(Hierarchy.Endpoint1QueueName);
        queues.AddNode(Hierarchy.Endpoint2QueueName);
        AnsiConsole.Write(queues);
    }

/**
      bundle-to-publish-to
        |__Samples.ASBS.HierarchyMigration.Endpoint1
            |__ $default: 1=0
            |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
            --> samples.asbs.hierarchymigration.endpoint1
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
            --> samples.asbs.hierarchymigration.endpoint2
        |__forwardTo-bundle-to-subscribe-to
            |__ $default: 1=1
            --> bundle-to-subscribe-to

      bundle-to-subscribe-to
        |__

       samples.asbs.hierarchymigration.endpoint1
       samples.asbs.hierarchymigration.endpoint2
     */
    public static void ForwardRuleSetup()
    {
        var sectionDescription = new Rule($"The forward rule on topic {Hierarchy.PublishBundleName} has been setup")
        {
            Alignment = Justify.Center,
            Border = BoxBorder.Ascii
        };
        AnsiConsole.Write(sectionDescription);

        var topics = new Tree("Topics");
        var publishBundle = topics.AddNode(Hierarchy.PublishBundleName);
        var endpoint1Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint1Subscription.AddNode("$default: 1=0");
        endpoint1Subscription.AddNode("Event2: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event2%'");
        endpoint1Subscription.AddNode(Hierarchy.Endpoint1QueueName);
        var endpoint2Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint2Subscription.AddNode("$default: 1=0");
        endpoint2Subscription.AddNode("Event1: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event1%'");
        endpoint2Subscription.AddNode(Hierarchy.Endpoint2QueueName);
        var forwardToSubscription = publishBundle.AddNode($"forwardTo-{Hierarchy.SubscriptionBundleName}");
        forwardToSubscription.AddNode("$default: 1=1");
        forwardToSubscription.AddNode(Hierarchy.SubscriptionBundleName);
        topics.AddNode(Hierarchy.SubscriptionBundleName);
        AnsiConsole.Write(topics);

        var queues = new Tree("Queues");
        queues.AddNode(Hierarchy.Endpoint1QueueName);
        queues.AddNode(Hierarchy.Endpoint2QueueName);
        AnsiConsole.Write(queues);
    }

/**
      bundle-to-publish-to
        |__Samples.ASBS.HierarchyMigration.Endpoint1
            |__ $default: 1=0
            |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
            --> samples.asbs.hierarchymigration.endpoint1
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
            --> samples.asbs.hierarchymigration.endpoint2
        |__forwardTo-bundle-to-subscribe-to
            |__ $default: 1=1
            --> bundle-to-subscribe-to

      bundle-to-subscribe-to
        |__

       samples.asbs.hierarchymigration.endpoint1
       samples.asbs.hierarchymigration.endpoint2
       samples.asbs.hierarchymigration.endpoint2.migration
     */
    public static void MigrationQueueSetup()
    {
        var sectionDescription = new Rule($"The migration queue {Hierarchy.Endpoint2MigrationQueueName} has been setup")
        {
            Alignment = Justify.Center,
            Border = BoxBorder.Ascii
        };
        AnsiConsole.Write(sectionDescription);

        var topics = new Tree("Topics");
        var publishBundle = topics.AddNode(Hierarchy.PublishBundleName);
        var endpoint1Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint1Subscription.AddNode("$default: 1=0");
        endpoint1Subscription.AddNode("Event2: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event2%'");
        endpoint1Subscription.AddNode(Hierarchy.Endpoint1QueueName);
        var endpoint2Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint2Subscription.AddNode("$default: 1=0");
        endpoint2Subscription.AddNode("Event1: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event1%'");
        endpoint2Subscription.AddNode(Hierarchy.Endpoint2QueueName);
        var forwardToSubscription = publishBundle.AddNode($"forwardTo-{Hierarchy.SubscriptionBundleName}");
        forwardToSubscription.AddNode("$default: 1=1");
        forwardToSubscription.AddNode(Hierarchy.SubscriptionBundleName);
        topics.AddNode(Hierarchy.SubscriptionBundleName);
        AnsiConsole.Write(topics);

        var queues = new Tree("Queues");
        queues.AddNode(Hierarchy.Endpoint1QueueName);
        queues.AddNode(Hierarchy.Endpoint2QueueName);
        queues.AddNode(Hierarchy.Endpoint2MigrationQueueName);
        AnsiConsole.Write(queues);
    }

    /**
      bundle-to-publish-to
        |__Samples.ASBS.HierarchyMigration.Endpoint1
            |__ $default: 1=0
            |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
            --> samples.asbs.hierarchymigration.endpoint1
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
            --> samples.asbs.hierarchymigration.endpoint2
        |__forwardTo-bundle-to-subscribe-to
            |__ $default: 1=1
            --> bundle-to-subscribe-to

      bundle-to-subscribe-to
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            --> samples.asbs.hierarchymigration.endpoint2.migration

       samples.asbs.hierarchymigration.endpoint1
       samples.asbs.hierarchymigration.endpoint2
       samples.asbs.hierarchymigration.endpoint2.migration
     */
    public static void Endpoint2SubscriptionUnderSubscriptionBundleSetup()
    {
        var sectionDescription =
            new Rule($"The subscription {Hierarchy.Endpoint2SubscriptionName} on topic {Hierarchy.SubscriptionBundleName} has been setup")
            {
                Alignment = Justify.Center,
                Border = BoxBorder.Ascii
            };
        AnsiConsole.Write(sectionDescription);

        var topics = new Tree("Topics");
        var publishBundle = topics.AddNode(Hierarchy.PublishBundleName);
        var endpoint1Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint1Subscription.AddNode("$default: 1=0");
        endpoint1Subscription.AddNode("Event2: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event2%'");
        endpoint1Subscription.AddNode(Hierarchy.Endpoint1QueueName);
        var endpoint2Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint2Subscription.AddNode("$default: 1=0");
        endpoint2Subscription.AddNode("Event1: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event1%'");
        endpoint2Subscription.AddNode(Hierarchy.Endpoint2QueueName);
        var forwardToSubscription = publishBundle.AddNode($"forwardTo-{Hierarchy.SubscriptionBundleName}");
        forwardToSubscription.AddNode("$default: 1=1");
        forwardToSubscription.AddNode(Hierarchy.SubscriptionBundleName);
        var subscriptionBundle = topics.AddNode(Hierarchy.SubscriptionBundleName);
        var endpoint2SubscriptionUnderSubscriptionBundle = subscriptionBundle.AddNode(Hierarchy.Endpoint2SubscriptionName);
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode("$default: 1=0");
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode(Hierarchy.Endpoint2MigrationQueueName);
        AnsiConsole.Write(topics);

        var queues = new Tree("Queues");
        queues.AddNode(Hierarchy.Endpoint1QueueName);
        queues.AddNode(Hierarchy.Endpoint2QueueName);
        queues.AddNode(Hierarchy.Endpoint2MigrationQueueName);
        AnsiConsole.Write(queues);
    }

    /**
      bundle-to-publish-to
        |__Samples.ASBS.HierarchyMigration.Endpoint1
            |__ $default: 1=0
            |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
            --> samples.asbs.hierarchymigration.endpoint1
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            |__ Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
            --> samples.asbs.hierarchymigration.endpoint2
        |__forwardTo-bundle-to-subscribe-to
            |__ $default: 1=1
            --> bundle-to-subscribe-to

      bundle-to-subscribe-to
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            --> samples.asbs.hierarchymigration.endpoint2.migration

       samples.asbs.hierarchymigration.endpoint1
       samples.asbs.hierarchymigration.endpoint2
       samples.asbs.hierarchymigration.endpoint2.migration
     */
    public static void Endpoint2SubscriptionUnderSubscriptionBundleWithRulesSetup()
    {
        var sectionDescription =
            new Rule($"The subscription {Hierarchy.Endpoint2SubscriptionName} on topic {Hierarchy.SubscriptionBundleName} with the necessary rules")
            {
                Alignment = Justify.Center,
                Border = BoxBorder.Ascii
            };
        AnsiConsole.Write(sectionDescription);

        var topics = new Tree("Topics");
        var publishBundle = topics.AddNode(Hierarchy.PublishBundleName);
        var endpoint1Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint1Subscription.AddNode("$default: 1=0");
        endpoint1Subscription.AddNode("Event2: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event2%'");
        endpoint1Subscription.AddNode(Hierarchy.Endpoint1QueueName);
        var endpoint2Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint2Subscription.AddNode("$default: 1=0");
        endpoint2Subscription.AddNode("Event1: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event1%'");
        endpoint2Subscription.AddNode(Hierarchy.Endpoint2QueueName);
        var forwardToSubscription = publishBundle.AddNode($"forwardTo-{Hierarchy.SubscriptionBundleName}");
        forwardToSubscription.AddNode("$default: 1=1");
        forwardToSubscription.AddNode(Hierarchy.SubscriptionBundleName);
        var subscriptionBundle = topics.AddNode(Hierarchy.SubscriptionBundleName);
        var endpoint2SubscriptionUnderSubscriptionBundle = subscriptionBundle.AddNode(Hierarchy.Endpoint2SubscriptionName);
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode("$default: 1=0");
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode("Event1: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event1%'");
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode(Hierarchy.Endpoint2MigrationQueueName);
        AnsiConsole.Write(topics);

        var queues = new Tree("Queues");
        queues.AddNode(Hierarchy.Endpoint1QueueName);
        queues.AddNode(Hierarchy.Endpoint2QueueName);
        queues.AddNode(Hierarchy.Endpoint2MigrationQueueName);
        AnsiConsole.Write(queues);
    }

    /**
      bundle-to-publish-to
        |__Samples.ASBS.HierarchyMigration.Endpoint1
            |__ $default: 1=0
            |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
            --> samples.asbs.hierarchymigration.endpoint1
        |__forwardTo-bundle-to-subscribe-to
            |__ $default: 1=1
            --> bundle-to-subscribe-to

      bundle-to-subscribe-to
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            --> samples.asbs.hierarchymigration.endpoint2.migration

       samples.asbs.hierarchymigration.endpoint1
       samples.asbs.hierarchymigration.endpoint2
       samples.asbs.hierarchymigration.endpoint2.migration
     */
    public static void Endpoint2SubscriptionOnPublishBundleDeleted()
    {
        var sectionDescription =
            new Rule($"The subscription {Hierarchy.Endpoint2SubscriptionName} on topic {Hierarchy.PublishBundleName} has been deleted")
            {
                Alignment = Justify.Center,
                Border = BoxBorder.Ascii
            };
        AnsiConsole.Write(sectionDescription);

        var topics = new Tree("Topics");
        var publishBundle = topics.AddNode(Hierarchy.PublishBundleName);
        var endpoint1Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint1Subscription.AddNode("$default: 1=0");
        endpoint1Subscription.AddNode("Event2: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event2%'");
        endpoint1Subscription.AddNode(Hierarchy.Endpoint1QueueName);
        var forwardToSubscription = publishBundle.AddNode($"forwardTo-{Hierarchy.SubscriptionBundleName}");
        forwardToSubscription.AddNode("$default: 1=1");
        forwardToSubscription.AddNode(Hierarchy.SubscriptionBundleName);
        var subscriptionBundle = topics.AddNode(Hierarchy.SubscriptionBundleName);
        var endpoint2SubscriptionUnderSubscriptionBundle = subscriptionBundle.AddNode(Hierarchy.Endpoint2SubscriptionName);
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode("$default: 1=0");
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode("Event1: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event1%'");
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode(Hierarchy.Endpoint2MigrationQueueName);
        AnsiConsole.Write(topics);

        var queues = new Tree("Queues");
        queues.AddNode(Hierarchy.Endpoint1QueueName);
        queues.AddNode(Hierarchy.Endpoint2QueueName);
        queues.AddNode(Hierarchy.Endpoint2MigrationQueueName);
        AnsiConsole.Write(queues);
    }

    /**
      bundle-to-publish-to
        |__Samples.ASBS.HierarchyMigration.Endpoint1
            |__ $default: 1=0
            |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
            --> samples.asbs.hierarchymigration.endpoint1
        |__forwardTo-bundle-to-subscribe-to
            |__ $default: 1=1
            --> bundle-to-subscribe-to

      bundle-to-subscribe-to
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            --> samples.asbs.hierarchymigration.endpoint2.migration

       samples.asbs.hierarchymigration.endpoint1
       samples.asbs.hierarchymigration.endpoint2
       samples.asbs.hierarchymigration.endpoint2.migration
        |__ samples.asbs.hierarchymigration.endpoint2
     */
    public static void Endpoint2MigrationQueueToEndpoint2QueueForwardingSetup()
    {
        var sectionDescription =
            new Rule($"The queue {Hierarchy.Endpoint2MigrationQueueName} forwarding to {Hierarchy.Endpoint2QueueName} has been setup")
            {
                Alignment = Justify.Center,
                Border = BoxBorder.Ascii
            };
        AnsiConsole.Write(sectionDescription);

        var topics = new Tree("Topics");
        var publishBundle = topics.AddNode(Hierarchy.PublishBundleName);
        var endpoint1Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint1Subscription.AddNode("$default: 1=0");
        endpoint1Subscription.AddNode("Event2: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event2%'");
        endpoint1Subscription.AddNode(Hierarchy.Endpoint1QueueName);
        var forwardToSubscription = publishBundle.AddNode($"forwardTo-{Hierarchy.SubscriptionBundleName}");
        forwardToSubscription.AddNode("$default: 1=1");
        forwardToSubscription.AddNode(Hierarchy.SubscriptionBundleName);
        var subscriptionBundle = topics.AddNode(Hierarchy.SubscriptionBundleName);
        var endpoint2SubscriptionUnderSubscriptionBundle = subscriptionBundle.AddNode(Hierarchy.Endpoint2SubscriptionName);
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode("$default: 1=0");
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode("Event1: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event1%'");
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode(Hierarchy.Endpoint2MigrationQueueName);
        AnsiConsole.Write(topics);

        var queues = new Tree("Queues");
        queues.AddNode(Hierarchy.Endpoint1QueueName);
        queues.AddNode(Hierarchy.Endpoint2QueueName);
        var migrationQueue = queues.AddNode(Hierarchy.Endpoint2MigrationQueueName);
        migrationQueue.AddNode(Hierarchy.Endpoint2QueueName);
        AnsiConsole.Write(queues);
    }

    /**
      bundle-to-publish-to
        |__Samples.ASBS.HierarchyMigration.Endpoint1
            |__ $default: 1=0
            |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
            --> samples.asbs.hierarchymigration.endpoint1
        |__forwardTo-bundle-to-subscribe-to
            |__ $default: 1=1
            --> bundle-to-subscribe-to

      bundle-to-subscribe-to
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            --> samples.asbs.hierarchymigration.endpoint2

       samples.asbs.hierarchymigration.endpoint1
       samples.asbs.hierarchymigration.endpoint2
       samples.asbs.hierarchymigration.endpoint2.migration
        |__ samples.asbs.hierarchymigration.endpoint2
     */
    public static void Endpoint2SubscriptionOnSubscriptionBundleFowardingChanged()
    {
        var sectionDescription =
            new Rule($"The subscription {Hierarchy.Endpoint2SubscriptionName} on topic {Hierarchy.SubscriptionBundleName} has been changed to forward to {Hierarchy.Endpoint2QueueName}")
            {
                Alignment = Justify.Center,
                Border = BoxBorder.Ascii
            };
        AnsiConsole.Write(sectionDescription);

        var topics = new Tree("Topics");
        var publishBundle = topics.AddNode(Hierarchy.PublishBundleName);
        var endpoint1Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint1Subscription.AddNode("$default: 1=0");
        endpoint1Subscription.AddNode("Event2: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event2%'");
        endpoint1Subscription.AddNode(Hierarchy.Endpoint1QueueName);
        var forwardToSubscription = publishBundle.AddNode($"forwardTo-{Hierarchy.SubscriptionBundleName}");
        forwardToSubscription.AddNode("$default: 1=1");
        forwardToSubscription.AddNode(Hierarchy.SubscriptionBundleName);
        var subscriptionBundle = topics.AddNode(Hierarchy.SubscriptionBundleName);
        var endpoint2SubscriptionUnderSubscriptionBundle = subscriptionBundle.AddNode(Hierarchy.Endpoint2SubscriptionName);
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode("$default: 1=0");
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode("Event1: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event1%'");
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode(Hierarchy.Endpoint2QueueName);
        AnsiConsole.Write(topics);

        var queues = new Tree("Queues");
        queues.AddNode(Hierarchy.Endpoint1QueueName);
        queues.AddNode(Hierarchy.Endpoint2QueueName);
        var migrationQueue = queues.AddNode(Hierarchy.Endpoint2MigrationQueueName);
        migrationQueue.AddNode(Hierarchy.Endpoint2QueueName);
        AnsiConsole.Write(queues);
    }

    /**
      bundle-to-publish-to
        |__Samples.ASBS.HierarchyMigration.Endpoint1
            |__ $default: 1=0
            |__ Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
            --> samples.asbs.hierarchymigration.endpoint1
        |__forwardTo-bundle-to-subscribe-to
            |__ $default: 1=1
            --> bundle-to-subscribe-to

      bundle-to-subscribe-to
        |__Samples.ASBS.HierarchyMigration.Endpoint2
            |__ $default: 1=0
            --> samples.asbs.hierarchymigration.endpoint2

       samples.asbs.hierarchymigration.endpoint1
       samples.asbs.hierarchymigration.endpoint2
     */
    public static void Endpoint2MigrationQueueDeleted()
    {
        var sectionDescription =
            new Rule($"he queue {Hierarchy.Endpoint2MigrationQueueName} was removed")
            {
                Alignment = Justify.Center,
                Border = BoxBorder.Ascii
            };
        AnsiConsole.Write(sectionDescription);

        var topics = new Tree("Topics");
        var publishBundle = topics.AddNode(Hierarchy.PublishBundleName);
        var endpoint1Subscription = publishBundle.AddNode(Hierarchy.Endpoint1SubscriptionName);
        endpoint1Subscription.AddNode("$default: 1=0");
        endpoint1Subscription.AddNode("Event2: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event2%'");
        endpoint1Subscription.AddNode(Hierarchy.Endpoint1QueueName);
        var forwardToSubscription = publishBundle.AddNode($"forwardTo-{Hierarchy.SubscriptionBundleName}");
        forwardToSubscription.AddNode("$default: 1=1");
        forwardToSubscription.AddNode(Hierarchy.SubscriptionBundleName);
        var subscriptionBundle = topics.AddNode(Hierarchy.SubscriptionBundleName);
        var endpoint2SubscriptionUnderSubscriptionBundle = subscriptionBundle.AddNode(Hierarchy.Endpoint2SubscriptionName);
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode("$default: 1=0");
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode("Event1: [[NServiceBus.EnclosedMessageTypes]] LIKE '%Event1%'");
        endpoint2SubscriptionUnderSubscriptionBundle.AddNode(Hierarchy.Endpoint2QueueName);
        AnsiConsole.Write(topics);

        var queues = new Tree("Queues");
        queues.AddNode(Hierarchy.Endpoint1QueueName);
        queues.AddNode(Hierarchy.Endpoint2QueueName);
        AnsiConsole.Write(queues);
    }
}