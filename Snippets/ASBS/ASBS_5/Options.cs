using System.IO;
using System.Text;
using System.Text.Json;
using MyNamespace;
using NServiceBus.Transport.AzureServiceBus;
using NUnit.Framework;

[TestFixture]
public class Options
{
    [Test, Explicit]
    public void Dump_Migration()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        var serializedOptions = JsonSerializer.Serialize(new MigrationTopologyOptions
#pragma warning restore CS0618 // Type or member is obsolete
        {
            QueueNameToSubscriptionNameMap = { { "Publisher", "PublisherSubscriptionName" } },
            SubscribedEventToRuleNameMap = { { typeof(NotYetMigratedEvent).FullName, "EventRuleName" } },
            TopicToPublishTo = "TopicToPublishTo",
            TopicToSubscribeOn = "TopicToSubscribeOn",
            EventsToMigrateMap = [typeof(NotYetMigratedEvent).FullName],
            PublishedEventToTopicsMap = { { typeof(MigratedEvent).FullName, "MigratedEvent" } },
            SubscribedEventToTopicsMap = { { typeof(MigratedEvent).FullName, ["MigratedEvent"] } }
        }, TopologyOptionsSerializationContext.Default.TopologyOptions);

        var builder = new StringBuilder();
        builder.AppendLine("# start" + "code migration-options");
        builder.AppendLine(serializedOptions);
        builder.AppendLine("# end" + "code migration-options");

        File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "migration-options.json"), builder.ToString());
    }

    [Test, Explicit]
    public void Dump_Options()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        var serializedOptions = JsonSerializer.Serialize(new TopologyOptions
#pragma warning restore CS0618 // Type or member is obsolete
        {
            QueueNameToSubscriptionNameMap = { { "Publisher", "PublisherSubscriptionName" } },
            PublishedEventToTopicsMap = { { typeof(SomeEvent).FullName, "some-event" } },
            SubscribedEventToTopicsMap = { { typeof(SomeEvent).FullName, ["some-event"] } }
        }, TopologyOptionsSerializationContext.Default.TopologyOptions);

        var builder = new StringBuilder();
        builder.AppendLine("# start" + "code topology-options");
        builder.AppendLine(serializedOptions);
        builder.AppendLine("# end" + "code topology-options");

        File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "topology-options.json"), builder.ToString());
    }

    [Test, Explicit]
    public void Dump_Options_Inheritance()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        var serializedOptions = JsonSerializer.Serialize(new TopologyOptions
#pragma warning restore CS0618 // Type or member is obsolete
        {
            QueueNameToSubscriptionNameMap = { { "Publisher", "PublisherSubscriptionName" } },
            PublishedEventToTopicsMap = { { typeof(SomeEvent).FullName, "some-event" } },
            SubscribedEventToTopicsMap = { { typeof(SomeEvent).FullName, ["some-event", "some-other-event"] } }
        }, TopologyOptionsSerializationContext.Default.TopologyOptions);

        var builder = new StringBuilder();
        builder.AppendLine("# start" + "code topology-options-inheritance");
        builder.AppendLine(serializedOptions);
        builder.AppendLine("# end" + "code topology-options-inheritance");

        File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "topology-options-inheritance.json"), builder.ToString());
    }
}

namespace MyNamespace
{
    class NotYetMigratedEvent;
    class MigratedEvent;

    class SomeEvent;
}
