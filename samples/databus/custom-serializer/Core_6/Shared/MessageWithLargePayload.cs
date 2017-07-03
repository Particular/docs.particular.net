using System;
using NServiceBus;

//the data bus is allowed to clean up transmitted properties older than the TTBR
[TimeToBeReceived("00:01:00")]
public class MessageWithLargePayload :
    ICommand
{
    public string SomeProperty { get; set; }
    public DataBusProperty<Measurement[]> LargeData { get; set; }
}

public class Measurement
{
    public DateTimeOffset Timestamp { get; set; }
    public string MeasurementName { get; set; }
    public decimal MeasurementValue { get; set; }
}
