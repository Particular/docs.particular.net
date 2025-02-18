using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Settings;

#region metrics-shim

class EmitNServiceBusMetrics : Feature
{
    public EmitNServiceBusMetrics()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        if (context.Settings.GetOrDefault<bool>("Endpoint.SendOnly"))
        {
            // there are no metrics relevant for send only endpoints yet
            return;
        }

        var queueName = context.LocalQueueAddress().BaseAddress;
        var discriminator = context.InstanceSpecificQueueAddress()?.Discriminator;

        var recoverabilitySettings = (RecoverabilitySettings)typeof(RecoverabilitySettings).GetConstructor(
              BindingFlags.NonPublic | BindingFlags.Instance,
              null, new Type[] { typeof(SettingsHolder) },
              null).Invoke(new object[] { (SettingsHolder)context.Settings });

        recoverabilitySettings.Immediate(i => i.OnMessageBeingRetried((m, _) => RecordRetry(m.Headers, queueName, discriminator, true)));
        recoverabilitySettings.Delayed(d => d.OnMessageBeingRetried((m, _) => RecordRetry(m.Headers, queueName, discriminator, false)));
        recoverabilitySettings.Failed(f => f.OnMessageSentToErrorQueue((m, _) => RecordFailure(m.Headers, queueName, discriminator)));

        context.Pipeline.OnReceivePipelineCompleted((e, _) =>
        {
            e.TryGetMessageType(out var messageType);

            var tags = CreateTags(queueName, discriminator, messageType);

            ProcessingTime.Record((e.CompletedAt - e.StartedAt).TotalMilliseconds, tags);

            if (e.TryGetDeliverAt(out DateTimeOffset startTime) || e.TryGetTimeSent(out startTime))
            {
                CriticalTime.Record((e.CompletedAt - startTime).TotalMilliseconds, tags);
            }

            return Task.CompletedTask;
        });
    }

    static Task RecordRetry(Dictionary<string, string> headers, string queueName, string discriminator, bool immediate)
    {
        headers.TryGetMessageType(out var messageType);

        var tags = CreateTags(queueName, discriminator, messageType);

        if (immediate)
        {
            ImmedidateRetries.Add(1, tags);
        }
        else
        {
            DelayedRetries.Add(1, tags);
        }
        Retries.Add(1, tags);

        return Task.CompletedTask;
    }

    static Task RecordFailure(Dictionary<string, string> headers, string queueName, string discriminator)
    {
        headers.TryGetMessageType(out var messageType);

        MessageSentToErrorQueue.Add(1, CreateTags(queueName, discriminator, messageType));

        return Task.CompletedTask;
    }

    static TagList CreateTags(string queueName, string discriminator, string messageType)
    {
        var tags = new TagList(new KeyValuePair<string, object>[] { new(Tags.QueueName, queueName) });

        if (!string.IsNullOrWhiteSpace(discriminator))
        {
            tags.Add(Tags.EndpointDiscriminator, discriminator);
        }

        if (!string.IsNullOrWhiteSpace(messageType))
        {
            tags.Add(Tags.MessageType, messageType);
        }

        return tags;
    }

    static readonly Meter NServiceBusMeter = new Meter("NServiceBus.Core", "0.1.0");

    public static readonly Counter<long> ImmedidateRetries =
        NServiceBusMeter.CreateCounter<long>("nservicebus.recoverability.immediate", description: "Number of immediate retries performed by the endpoint.");

    public static readonly Counter<long> DelayedRetries =
        NServiceBusMeter.CreateCounter<long>("nservicebus.recoverability.delayed", description: "Number of delayed retries performed by the endpoint.");

    public static readonly Counter<long> Retries =
        NServiceBusMeter.CreateCounter<long>("nservicebus.recoverability.retries", description: "Number of retries performed by the endpoint.");

    public static readonly Counter<long> MessageSentToErrorQueue =
        NServiceBusMeter.CreateCounter<long>("nservicebus.recoverability.error", description: "Number of messages sent to the error queue.");

    public static readonly Histogram<double> ProcessingTime =
        NServiceBusMeter.CreateHistogram<double>("nservicebus.messaging.processing_time", "ms", "The time in milliseconds between when the message was pulled from the queue until processed by the endpoint.");

    public static readonly Histogram<double> CriticalTime =
        NServiceBusMeter.CreateHistogram<double>("nservicebus.messaging.critical_time", "ms", "The time in milliseconds between when the message was sent until processed by the endpoint.");

    public static class Tags
    {
        public const string EndpointDiscriminator = "nservicebus.discriminator";
        public const string QueueName = "nservicebus.queue";
        public const string MessageType = "nservicebus.message_type";
    }
}
#endregion