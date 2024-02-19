using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using static Meters;

static class Extensions
{
    public static void EnableMetricsShim(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.Recoverability().Immediate(i => i.OnMessageBeingRetried((m, _) => RecordRetry(m.Headers)));
        endpointConfiguration.Recoverability().Delayed(i => i.OnMessageBeingRetried((m, _) => RecordRetry(m.Headers)));
    }

    static Task RecordRetry(Dictionary<string, string> headers)
    {
        headers.TryGetMessageType(out var messageType);

        var tags = new TagList(new KeyValuePair<string, object>[]
        {
            new(Tags.MessageType, messageType ?? ""),
        });

        Meters.Retries.Add(1, tags);

        return Task.CompletedTask;
    }

    public static bool TryGetTimeSent(this ReceivePipelineCompleted completed, out DateTimeOffset timeSent)
    {
        var headers = completed.ProcessedMessage.Headers;
        if (headers.TryGetValue(Headers.TimeSent, out var timeSentString))
        {
            timeSent = DateTimeOffsetHelper.ToDateTimeOffset(timeSentString);
            return true;
        }
        timeSent = DateTimeOffset.MinValue;
        return false;
    }

    public static bool TryGetDeliverAt(this ReceivePipelineCompleted completed, out DateTimeOffset deliverAt)
    {
        var headers = completed.ProcessedMessage.Headers;
        if (headers.TryGetValue(Headers.DeliverAt, out var deliverAtString))
        {
            deliverAt = DateTimeOffsetHelper.ToDateTimeOffset(deliverAtString);
            return true;
        }
        deliverAt = DateTimeOffset.MinValue;
        return false;
    }

    public static bool TryGetMessageType(this ReceivePipelineCompleted completed, out string processedMessageType)
        => completed.ProcessedMessage.Headers.TryGetMessageType(out processedMessageType);

    internal static bool TryGetMessageType(this IReadOnlyDictionary<string, string> headers, out string processedMessageType)
    {
        if (headers.TryGetValue(Headers.EnclosedMessageTypes, out var enclosedMessageType))
        {
            processedMessageType = enclosedMessageType;
            return true;
        }
        processedMessageType = null;
        return false;
    }
}