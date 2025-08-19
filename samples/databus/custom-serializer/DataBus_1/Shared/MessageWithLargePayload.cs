using System;
using NServiceBus;
using NServiceBus.ClaimCheck;

[TimeToBeReceived("00:01:00")]
public class MessageWithLargePayload :
    ICommand
{
    public string SomeProperty { get; set; }
    public ClaimCheckProperty<Measurement[]> LargeData { get; set; }
}

[Serializable]
public class Measurement
{
    public DateTimeOffset Timestamp { get; set; }
    public string MeasurementName { get; set; }
    public decimal MeasurementValue { get; set; }
}