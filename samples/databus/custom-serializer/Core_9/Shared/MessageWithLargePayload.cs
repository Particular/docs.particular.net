using System;
using NServiceBus;

[TimeToBeReceived("00:01:00")]
public class MessageWithLargePayload :
    ICommand
{
    public string SomeProperty { get; set; }
#pragma warning disable CS0618 // Type or member is obsolete
    public DataBusProperty<Measurement[]> LargeData { get; set; }
#pragma warning restore CS0618 // Type or member is obsolete
}

[Serializable]
public class Measurement
{
    public DateTimeOffset Timestamp { get; set; }
    public string MeasurementName { get; set; }
    public decimal MeasurementValue { get; set; }
}