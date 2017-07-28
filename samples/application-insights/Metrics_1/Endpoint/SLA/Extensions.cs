using System;
using NServiceBus;
using NServiceBus.Features;

static class Extensions
{
    public static bool TryGetTimeSent(this ReceivePipelineCompleted completed, out DateTime timeSent)
    {
        var headers = completed.ProcessedMessage.Headers;
        string timeSentString;
        if (headers.TryGetValue(Headers.TimeSent, out timeSentString))
        {
            timeSent = DateTimeExtensions.ToUtcDateTime(timeSentString);
            return true;
        }
        timeSent = DateTime.MinValue;
        return false;
    }

    public static void ThrowIfSendonly(this FeatureConfigurationContext context)
    {
        var isSendOnly = context.Settings.GetOrDefault<bool>("Endpoint.SendOnly");
        if (isSendOnly)
        {
            throw new Exception("Windows performance counters are not supported on send only endpoints.");
        }
    }
}