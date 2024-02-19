using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

#region metrics-shim
class EmitNServiceBusMetrics : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var queueName = context.LocalQueueAddress().BaseAddress;
        var discriminator = context.InstanceSpecificQueueAddress()?.Discriminator;

        context.Pipeline.OnReceivePipelineCompleted((e, _) =>
        {
            e.TryGetMessageType(out var messageType);

            var tags = new TagList(new KeyValuePair<string, object>[]
            {
                    new(Meters.Tags.QueueName, queueName ?? ""),
                    new(Meters.Tags.EndpointDiscriminator, discriminator ?? ""),
                    new(Meters.Tags.MessageType, messageType ?? "")
            });

            Meters.ProcessingTime.Record((e.CompletedAt - e.StartedAt).TotalMilliseconds, tags);

            if (e.TryGetDeliverAt(out DateTimeOffset startTime) || e.TryGetTimeSent(out startTime))
            {
                Meters.CriticalTime.Record((e.CompletedAt - startTime).TotalMilliseconds, tags);
            }

            return Task.CompletedTask;
        });
    }
}
#endregion