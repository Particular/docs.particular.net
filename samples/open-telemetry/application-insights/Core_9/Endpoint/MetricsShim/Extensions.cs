using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Settings;

static class Extensions
{
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

    public static void ThrowIfSendOnly(this IReadOnlySettings settings)
    {
        var isSendOnly = settings.GetOrDefault<bool>("Endpoint.SendOnly");
        if (isSendOnly)
        {
            throw new Exception("Metrics are not supported on send only endpoints.");
        }
    }
}