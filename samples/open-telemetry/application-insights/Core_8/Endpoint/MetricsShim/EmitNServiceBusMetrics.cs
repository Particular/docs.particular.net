using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

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

        context.Pipeline.OnReceivePipelineCompleted((e, _) =>
        {
            e.TryGetMessageType(out var messageType);

            var tags = new TagList(new KeyValuePair<string, object>[]
            {
                    new(MeterTags.EndpointDiscriminator, discriminator ?? ""),
                    new(MeterTags.QueueName, queueName ?? ""),
                    new(MeterTags.MessageType, messageType ?? "")
            });

            ProcessingTime.Record((e.CompletedAt - e.StartedAt).TotalMilliseconds, tags);

            if (e.TryGetDeliverAt(out DateTimeOffset startTime) || e.TryGetTimeSent(out startTime))
            {
                CriticalTime.Record((e.CompletedAt - startTime).TotalMilliseconds, tags);
            }

            return Task.CompletedTask;
        });
    }

    static readonly Meter NServiceBusMeter = new Meter(
       "NServiceBus.Core",
       "0.1.0");

    static readonly Histogram<double> ProcessingTime =
        NServiceBusMeter.CreateHistogram<double>("nservicebus.messaging.processingtime", "ms", "The time in milliseconds between when the message was pulled from the queue until processed by the endpoint.");

    static readonly Histogram<double> CriticalTime =
        NServiceBusMeter.CreateHistogram<double>("nservicebus.messaging.criticaltime", "ms", "The time in milliseconds between when the message was sent until processed by the endpoint.");

    static class MeterTags
    {
        public const string EndpointDiscriminator = "nservicebus.discriminator";
        public const string QueueName = "nservicebus.queue";
        public const string MessageType = "nservicebus.message_type";
    }
}
#endregion