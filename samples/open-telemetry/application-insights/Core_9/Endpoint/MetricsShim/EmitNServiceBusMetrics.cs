using System.Diagnostics.Metrics;

class EmitNServiceBusMetrics
{
    static readonly Meter NServiceBusMeter = new Meter(
           "NServiceBus.Core",
           "0.1.0");

    static readonly Histogram<decimal> CriticalTime =
        NServiceBusMeter.CreateHistogram<decimal>("nservicebus.messaging.criticaltime", description: "The critical time for messages processed by the endpoint.");
}