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
        var queueName = context.LocalQueueAddress().BaseAddress;
        var discriminator = context.InstanceSpecificQueueAddress()?.Discriminator;

        var recoverabilitySettings = (RecoverabilitySettings)typeof(RecoverabilitySettings).GetConstructor(
              BindingFlags.NonPublic | BindingFlags.Instance,
              null, [typeof(SettingsHolder)],
              null).Invoke([(SettingsHolder)context.Settings]);

        recoverabilitySettings.Immediate(i => i.OnMessageBeingRetried((m, _) => RecordRetry(m.Headers, queueName, discriminator)));
        recoverabilitySettings.Delayed(i => i.OnMessageBeingRetried((m, _) => RecordRetry(m.Headers, queueName, discriminator)));

        context.Pipeline.OnReceivePipelineCompleted((e, _) =>
        {
            e.TryGetMessageType(out var messageType);

            var tags = new TagList(
            [
                new(Tags.QueueName, queueName ?? ""),
                new(Tags.EndpointDiscriminator, discriminator ?? ""),
                new(Tags.MessageType, messageType ?? "")
            ]);

            ProcessingTime.Record((e.CompletedAt - e.StartedAt).TotalMilliseconds, tags);

            if (e.TryGetDeliverAt(out DateTimeOffset startTime) || e.TryGetTimeSent(out startTime))
            {
                CriticalTime.Record((e.CompletedAt - startTime).TotalMilliseconds, tags);
            }

            return Task.CompletedTask;
        });
    }

    static Task RecordRetry(Dictionary<string, string> headers, string queueName, string discriminator)
    {
        headers.TryGetMessageType(out var messageType);

        var tags = new TagList(
        [
            new(Tags.QueueName, queueName ?? ""),
            new(Tags.EndpointDiscriminator, discriminator ?? ""),
            new(Tags.MessageType, messageType ?? "")
        ]);

        Retries.Add(1, tags);

        return Task.CompletedTask;
    }

    static readonly Meter NServiceBusMeter = new Meter("NServiceBus.Core", "0.1.0");

    public static readonly Counter<long> Retries =
        NServiceBusMeter.CreateCounter<long>("nservicebus.messaging.retries", description: "Number of retries performed by the endpoint.");

    public static readonly Histogram<double> ProcessingTime =
        NServiceBusMeter.CreateHistogram<double>("nservicebus.messaging.processingtime", "ms", "The time in milliseconds between when the message was pulled from the queue until processed by the endpoint.");

    public static readonly Histogram<double> CriticalTime =
        NServiceBusMeter.CreateHistogram<double>("nservicebus.messaging.criticaltime", "ms", "The time in milliseconds between when the message was sent until processed by the endpoint.");

    public static class Tags
    {
        public const string EndpointDiscriminator = "nservicebus.discriminator";
        public const string QueueName = "nservicebus.queue";
        public const string MessageType = "nservicebus.message_type";
    }
}
#endregion