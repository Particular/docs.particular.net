using System;
using NServiceBus;

static class Extensions
{
    public static bool TryGetTimeSent(this ReceivePipelineCompleted completed, out DateTime timeSent)
    {
        var headers = completed.ProcessedMessage.Headers;
        if (headers.TryGetValue(Headers.TimeSent, out var timeSentString))
        {
            timeSent = DateTimeExtensions.ToUtcDateTime(timeSentString);
            return true;
        }

        timeSent = DateTime.MinValue;
        return false;
    }
}