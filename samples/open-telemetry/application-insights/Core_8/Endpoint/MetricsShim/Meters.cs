using System.Diagnostics.Metrics;

class Meters
{
    static readonly Meter NServiceBusMeter = new Meter(
   "NServiceBus.Core",
   "0.1.0");

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